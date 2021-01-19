using Snowcap.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// A basic controller for the NavTileAgent. 
    /// Used for simple use cases and as an example.
    /// </summary>
    [RequireComponent(typeof(NavTileAgent))]
    [AddComponentMenu("NavTile/Nav Tile Agent Controller")]
    public class NavTileAgentController : MonoBehaviour
    {
        public enum WaypointDestinationType
        {
            Transform,
            WorldPosition,
            GridCoordinate
        }
        
        [System.Serializable]
        public class PathWaypoint
        {
            public float Delay;
            public WaypointDestinationType DestinationType;
            public Transform TargetTransform;
            public Vector3 TargetPosition;
            public Vector2Int TargetCoordinate;

            public Vector2Int GetTargetCoordinate()
            {
                switch (DestinationType)
                {
                    case WaypointDestinationType.Transform:
                        TargetPosition = TargetTransform.position;
                        goto case WaypointDestinationType.WorldPosition;
                    case WaypointDestinationType.WorldPosition:
                        TargetCoordinate = NavTileManager.Instance.SurfaceManager.Grid.WorldToCell(TargetPosition).GetVector2Int();
                        goto case WaypointDestinationType.GridCoordinate;
                    case WaypointDestinationType.GridCoordinate:
                        return TargetCoordinate;
                    default:
                        return Vector2Int.zero;
                }
            }
        }

        /// <summary>
        /// All waypoints for the agent to traverse over.
        /// </summary>
        [SerializeField]
        public List<PathWaypoint> _waypoints = new List<PathWaypoint>();

        /// <summary>
        /// Whether the agent should loop the waypoint, e.g. go from last waypoint to the first.
        /// </summary>
        [SerializeField]
        private bool _loop = false;

        /// <summary>
        /// Agent that this controller has control over.
        /// </summary>
        private NavTileAgent _agent;

        /// <summary>
        /// The index of the waypoint the agent will move to next.
        /// </summary>
        private int _nextWaypointIndex;

        /// <summary>
        /// Coroutine for waiting between waypoints.
        /// </summary>
        private Coroutine _waitingCoroutine;

        private void Awake()
        {
            _agent = GetComponent<NavTileAgent>();

            _agent.AutoTraversePath = true;
        }

        private void Start()
        {
            _agent.OnPathFinishedUnityEvent.AddListener(MoveToNextWaypoint);
            _agent.OnPathAbortedUnityEvent.AddListener(MoveToNextWaypoint);
            _agent.OnPathNotFoundUnityEvent.AddListener(MoveToNextWaypoint);

            StartMoving();
        }

        private void OnDestroy()
        {
            _agent.OnPathFinishedUnityEvent.RemoveListener(MoveToNextWaypoint);
            _agent.OnPathAbortedUnityEvent.RemoveListener(MoveToNextWaypoint);
            _agent.OnPathNotFoundUnityEvent.RemoveListener(MoveToNextWaypoint);
        }

        /// <summary>
        /// Starts the movement from the first waypoint.
        /// </summary>
        public void StartMoving()
        {
            _nextWaypointIndex = 0;
            MoveToNextWaypoint();
        }

        /// <summary>
        /// Stops movement of the agent.
        /// </summary>
        public void StopMoving()
        {
            if (_waitingCoroutine != null)
            {
                StopCoroutine(_waitingCoroutine);
            }

            _agent.CancelPath();
        }

        private void MoveToNextWaypoint(NavTileAgent.EAbortReason inArg0, Vector2Int inArg1)
        {
            MoveToNextWaypoint();
        }

        /// <summary>
        /// Tries to start the move to the next waypoint.
        /// </summary>
        private void MoveToNextWaypoint()
        {
            if (_nextWaypointIndex >= _waypoints.Count)
            {
                // Done with all waypoints.
                if (_waypoints.Count >= 2 && _loop)
                {
                    _nextWaypointIndex = 0;
                }
                else
                {
                    return;
                }
            }

            if (_waitingCoroutine != null)
            {
                StopCoroutine(_waitingCoroutine);
            }

            _waitingCoroutine = StartCoroutine(MoveToNextWaypointDelayed());
        }

        /// <summary>
        /// Starts the move to the next waypoint with the entered delay for that waypoint.
        /// </summary>
        private IEnumerator MoveToNextWaypointDelayed()
        {
            yield return new WaitForSeconds(_waypoints[_nextWaypointIndex].Delay);

            if (_nextWaypointIndex < _waypoints.Count)
            {
                _agent.MoveToPosition(_waypoints[_nextWaypointIndex].GetTargetCoordinate());

                _nextWaypointIndex++;
            }
        }
    }
}
