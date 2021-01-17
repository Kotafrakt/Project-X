using System;
using System.Collections;
using System.Collections.Generic;
using Snowcap.Extensions;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snowcap.NavTiles
{
    /// <summary>
    /// NavTileAgent class. Functions as the agent which can traverse over a NavTile baked grid.
    /// 
    /// Not inheritable due to a custom inspector. Change the source for custom behavior.
    /// </summary>
    [AddComponentMenu("NavTile/Nav Tile Agent")]
    public sealed class NavTileAgent : MonoBehaviour
    {
        /// <summary>
        /// Settings which correspond with an area on the grid.
        /// </summary>
        [System.Serializable]
        public class AreaSettings
        {
            // The area index the settings correspond to.
            public int AreaIndex;
            // The speed the agent will travel on the specific area.
            public float Speed = -1;
            // An animation trigger to be activated when entering the specific area.
            public string AnimationTriggerName = "";

            /// <summary>
            /// Constructor for AreaSettings.
            /// </summary>
            /// <param name="inAreaIndex">The index of the relevant area as seen in the AREA tab in the Navigation2D window.</param>
            /// <param name="inSpeed">The speed of the agent in this area. Default is -1, which means it will use the default speed of the agent.</param>
            /// <param name="inAnimationTriggerName">The animation trigger which will be activated when entering this area. Default is empty.</param>
            public AreaSettings(int inAreaIndex, float inSpeed = -1, string inAnimationTriggerName = "")
            {
                AreaIndex = inAreaIndex;
                Speed = inSpeed;
                AnimationTriggerName = inAnimationTriggerName;
            }
        }

        /// <summary>
        /// Enum to specify the Path Status of this agent.
        /// 
        /// NotAvailable = This agent is not calculating or traversing a path.
        /// Calculating = This agent is calculating its path.
        /// Traversing = This agent is traversing its path.
        /// </summary>
        public enum EPathStatus
        {
            NotAvailable,
            Calculating,
            Traversing
        }

        /// <summary>
        /// Enum to specify the Movement Status of this agent.
        /// 
        /// Traversing = This agent is moving along the path.
        /// Waiting = This agent is currently standing still, but waiting for another agent to move before starting to traverse again.
        /// Stationary = This agent is standing still and has no intention of moving again.
        /// </summary>
        public enum EMovementStatus
        {
            Traversing,
            Waiting,
            Stationary
        }

        /// <summary>
        /// Enum to specify how to handle the Conflict Handling for this agent.
        /// 
        /// Disabled                :   This agent does not care about conflicts. 
        ///                             It does not write to the grid about tiles it occupies and won't prevent to collide with any other agents.
        /// 
        /// AbortOnObstruction      :   This agent will abort its path entirely when an obstruction is encountered.
        /// 
        /// WaitOnTraversingAgent   :   This agent will stop for a traversing, and once the tile is free, start moving again. 
        ///                             With other obstructions, the path is aborted.
        /// 
        /// WaitOnWaitingAgent      :   This agent will stop for traversing and waiting agents, and once the tile is free, start moving again. 
        ///                             In case the agent encounters a stationary agent, or it encounters an agent which is ultimately waiting on this agent or some other form of circular conflict, 
        ///                             the path is aborted.
        /// 
        /// Note that these conflicts layer. So e.g. if WaitOnWaitingAgent is activated, It will also wait on traversing agents, before abort on obstruction.
        /// </summary>
        public enum EConflictOptions
        {
            Disabled               = 0,
            AbortOnObstruction     = 1,
            WaitOnTraversingAgent  = 2,
            WaitOnWaitingAgent     = 3
        }

        /// <summary>
        /// Enum to specify the conflict status of this agent.
        /// 
        /// This is used to properly handle conflict handling on execution.
        /// 
        /// Success = This agent has not encountered an obstruction.
        /// Processing = This agent is in the middle of handling the conflict found. It will need to be resolved before traversing again.
        /// Abort = The conflict can't be solved, and the path will be aborted.
        /// </summary>
        enum EConflictStatus
        {
            Success,
            Processing,
            Abort
        }

        public enum EAbortReason
        {
            None,
            EncounteredObstruction,
            EncounteredStationaryAgent,
            EncounteredWaitingAgent,
            EncounteredCircularConflict
        }

        public bool AutoTraversePath = true;

        // The area mask for this agent. Specifies which areas this agent can walk on.
        [SerializeField]
        private int _areaMask = 1;
        public int AreaMask { get { return _areaMask; } }

        // The agent type of this agent. Used for handling conflicts.
        [SerializeField]
        private int _agentType = 0;
        public int AgentType { get { return _agentType; }}

        // The default speed for this agent on any tile. Can be overridden per area.
        [SerializeField]
        private float _speed = 3;
        public float Speed { get { return _speed; } }

        // Whether diagonal movement is allowed for this agent.
        [SerializeField]
        private bool _diagonalAllowed = false;

        // Whether diagonal movement is allowed on tiles adjacent to a non-walkable tile.
        [SerializeField]
        private bool _cutCorners = false;

        // Whether the tile cost should be ignored and all tiles are uniform. Non-walkable tiles keep being non-walkable
        [SerializeField]
        private bool _ignoreTileCost = false;

        // The conflict handling setting for this agent.
        [SerializeField]
        private EConflictOptions _conflictOption = EConflictOptions.Disabled;

        // How long to wait before moving after waiting for an agent encountered as a conflict. In seconds.
        [SerializeField]
        private float _waitAfterFreeTile = 0;

        // Whether an obstruction was encountered during traversing.
        private bool _didEncounterObstruction = false;
        // How long it has been since the obstruction.
        private float _obstructionTimer = 0;

        // The tile position this agent is moving towards.
        [SerializeField]
        private Vector2Int _targetPos;
        public Vector2Int TargetPos
        {
            get { return _targetPos; }
            set { _targetPos = value; }
        }

        // Status of the Path. See the corresponding enum for a broad description of the options.
        private EPathStatus _pathStatus;
        public EPathStatus PathStatus { get { return _pathStatus; } }

        // Status of Movement. See the corresponding enum for a broad description of the options.
        private EMovementStatus _movementStatus;
        public EMovementStatus MovementStatus { get { return _movementStatus; } }

        // Whether the agent should display its calculated path with gizmos for debugging purposes.
        [SerializeField]
        private bool _debugEnabled = false;

        // Color of the debug line to visualize the calculated path for this agent.
        [SerializeField]
        private Color _debugLineColor = Color.blue;

        // The animator linked to this agent to trigger animations from.
        [SerializeField]
        public Animator LinkedAnimator;

        // Whether the directional movement parameters shouldn't be reset when the agent stops moving.
        [SerializeField]
        private bool _preserveAnimDirection = false;

        // The animation float parameter to set for speed.
        [SerializeField]
        [Tooltip("Set the animator float parameter to set speed for")]
        private string _animationSpeedParameter = "";

        // The animation float parameter to set for normalized horizontal movement.
        [SerializeField]
        [Tooltip("Set the animator float parameter to set normalized horizontal movement for")]
        private string _animationHorizontalParameter = "";

        // The animation float parameter to set for normalized vertical movement.
        [SerializeField]
        [Tooltip("Set the animator float parameter to set normalized vertical movement for")]
        private string _animationVerticalParameter = "";

        // List of settings for this agent per available area
        [SerializeField]
        private List<AreaSettings> _areaSettingsList = new List<AreaSettings>();
        public List<AreaSettings> AreaSettingsList { get { return _areaSettingsList; } }

        // Callback triggered when the area walked on has changed.
        public NavTileAreaUnityEvent OnAreaChangeUnityEvent = new NavTileAreaUnityEvent();

        public UnityEvent OnPathNotFoundUnityEvent = new UnityEvent();

        // Callback triggered when a path is found.
        public NavTilePathFoundEvent OnPathFoundUnityEvent = new NavTilePathFoundEvent();

        // Callback triggered when the path is aborted.
        public NavTilePathAbortedEvent OnPathAbortedUnityEvent = new NavTilePathAbortedEvent();

        // Callback triggered when the path is finished.
        public UnityEvent OnPathFinishedUnityEvent = new UnityEvent();

        // Coroutine which can be used to cancel the traversing of the path.
        private Coroutine _followingPathCoroutine;

        // The path calculated or being traversed.
        private NavTilePath _path;
        public NavTilePath Path
        {
            get { return _path; }
            set { _path = value; }
        }

        // The tile this agent is occupying on the grid.
        private Vector2Int _occupyingTilePosition;
        // The tile this agent is waiting for in case of an obstruction.
        private TileData _waitingForTile;
        public TileData WaitingForTile { get { return _waitingForTile; } }

        private Vector2Int _previousPosition;

        // The current index in the path the agent is at during traversing.
        private int _currentPathNodeIndex;

#if UNITY_EDITOR
        // Visualizer component to visualize how this agent's path was calculated.
        private NavTilePathVisualizer visualizer;
#endif

        /// <summary>
        /// Unity's Start method. Initializes the NavTileAgent.
        /// </summary>
        private void Start()
        {
            OnAreaChangeUnityEvent.AddListener(ChangeAnimationOnArea);
            OnPathAbortedUnityEvent.AddListener(OnPathAbortSetAnimationParameters);

            // Initialize Conflict handling varaibles when enabled.
            if (_conflictOption != EConflictOptions.Disabled)
            {
                if (!NavTileManager.Instance.SurfaceManager.IsDataInitialized || 
                    NavTileManager.Instance.SurfaceManager.Data.HasNoTiles ||
                    NavTileManager.Instance.SurfaceManager.Grid == null)
                {
                    Debug.LogWarning("Data is not initialized for this scene. Please bake in the navigation 2D window.");
                    return;
                }
                
                Vector2Int currentTilePosition = NavTileManager.Instance.SurfaceManager.Grid.WorldToCell(transform.position).GetVector2Int();
                NavTileManager.Instance.SurfaceManager.Data.GetTileData(currentTilePosition).OccupyingAgents.Add(this);

                _occupyingTilePosition = currentTilePosition;
            }

            _pathStatus = EPathStatus.NotAvailable;
            _movementStatus = EMovementStatus.Stationary;
        }

        /// <summary>
        /// Move the agent to a World position.
        /// </summary>
        /// <param name="inTargetPosition">The position to move to as a world position.</param>
        public void MoveToPosition(Vector3 inTargetPosition, params Vector2Int[] inAvoidTiles)
        {
            if (NavTileManager.Instance.SurfaceManager.Grid == null)
            {
                Debug.LogWarning("There is no (or multiple) grid object(s) found in the scene. Path can't be calculated without one.");
                OnPathNotFoundUnityEvent.Invoke();
                return;
            }

            MoveToPosition(NavTileManager.Instance.SurfaceManager.Grid.WorldToCell(inTargetPosition).GetVector2Int(), inAvoidTiles);
        }

        /// <summary>
        /// Moves the agent to a tile position
        /// </summary>
        /// <param name="inTargetPosition">The position to move to as a tile position.</param>
        public async void MoveToPosition(Vector2Int inTargetPosition, params Vector2Int[] inAvoidTiles)
        {
            // Check for early exits or possible errors first

            if (NavTileManager.Instance.SurfaceManager.Grid == null)
            {
                Debug.LogWarning("There is no (or multiple) grid object(s) found in the scene. Path can't be calculated without one.");
                OnPathNotFoundUnityEvent.Invoke();
                return;
            }

            if (!NavTileManager.Instance.SurfaceManager.IsDataInitialized || NavTileManager.Instance.SurfaceManager.Data.HasNoTiles)
            {
                Debug.LogWarning("Data is not initialized for this scene. Please bake in the navigation 2D window.");
                OnPathNotFoundUnityEvent.Invoke();
                return;
            }

            Vector2Int startCoordinate = NavTileManager.Instance.SurfaceManager.Grid.WorldToCell(this.transform.position).GetVector2Int();

            if (startCoordinate == inTargetPosition)
            {
                Debug.LogWarning("Trying to find a path to the start node. Calculating a path is redundant.");
                OnPathNotFoundUnityEvent.Invoke();
                return;
            }

            if (NavTileManager.Instance.SurfaceManager.Data.GetTileData(inTargetPosition) == null)
            {
                Debug.LogWarning(string.Format("Coordinate {0} is not a tile on the grid, can't calculate path.", inTargetPosition));
                OnPathNotFoundUnityEvent.Invoke();
                return;
            }

            // Start calculating a path.
            _pathStatus = EPathStatus.Calculating;

            // Intialize parameters to find a path.
            FindPathInput input = new FindPathInput(startCoordinate, inTargetPosition, _areaMask, this._diagonalAllowed, this._cutCorners, this._ignoreTileCost, inAvoidTiles);

#if UNITY_EDITOR
            input.Visualizer = visualizer;
#endif

            // Get a path based on the input and the Pipeline Settings in the Navigation2D window.
            _path = await NavTileManager.Instance.GetPath(input);

            // If no path is found, return.
            if (_path == null || _path.Count == 0)
            {
                Debug.LogWarning("Path could not be found.", gameObject);

                _pathStatus = EPathStatus.NotAvailable;

                OnPathNotFoundUnityEvent.Invoke();

                return;
            }
            
            OnPathFound();       
        }

        /// <summary>
        /// Coroutine for following a given path.
        /// </summary>
        /// <param name="inPath">The path that should be followed.</param>
        private IEnumerator FollowPath(NavTilePath inPath)
        {
            if (inPath == null)
                yield break;

            // Prepare path traversal.
            _currentPathNodeIndex = 0;
            PathNode currentNode = new PathNode(transform.position);
            PathNode nextNode = inPath[_currentPathNodeIndex];

            _pathStatus = EPathStatus.Traversing;
            _movementStatus = EMovementStatus.Traversing;

            float timeInPath = 0;
            float timeInCurrentNode = 0;
            float currentSpeed = GetAreaSpeed(nextNode.GetAreaIndex());
            OnAreaChangeUnityEvent.Invoke(nextNode.GetAreaIndex());

            float timeToNextNode = inPath.GetDurationBetweenNodes(currentNode, nextNode, currentSpeed);

            // Initial animation triggers.
            SetAnimationParameters(nextNode.WorldPosition - currentNode.WorldPosition, currentSpeed);

            // Do conflict handling if enabled
            if (_conflictOption != EConflictOptions.Disabled)
            {
                // Check if there is a conflict before starting to traverse.
                EConflictStatus conflictStatus;
                EAbortReason abortReason;

                do
                {
                    conflictStatus = HandleConflict(currentNode, nextNode, out abortReason);

                    // In case the conflict is not completely resolved, yield the coroutine and check again next frame.
                    if (conflictStatus == EConflictStatus.Processing)
                    {
                        _movementStatus = EMovementStatus.Waiting;
                        yield return null;
                    }
                    else if (conflictStatus == EConflictStatus.Abort) // In case the conflict cannot be resolved, abort path traversing.
                    {
                        _pathStatus = EPathStatus.NotAvailable;
                        _movementStatus = EMovementStatus.Stationary;

                        OnPathAbortedUnityEvent.Invoke(abortReason, nextNode.TilePosition);
                        yield break;
                    }
                } while (conflictStatus == EConflictStatus.Processing); // If this is false, the 'do-while' will stop and thus a conflict has not been encountered.
            }

            // Start traversing.
            while (true)
            {
                // Check if the agent will reach the next node in this frame or not.

                // If so, start walking towards the subsequent node.
                if (timeInCurrentNode + Time.deltaTime >= timeToNextNode)
                {
                    // Switch to next node.
                    _currentPathNodeIndex++;

                    // Goal reached?
                    if (_currentPathNodeIndex == inPath.Count)
                    {
                        transform.position = nextNode.WorldPosition;

                        _pathStatus = EPathStatus.NotAvailable;
                        _movementStatus = EMovementStatus.Stationary;

                        // Reset animation parameters.
                        ResetAnimationParameters();

                        OnPathFinishedUnityEvent.Invoke();
                        yield break;
                    }


                    currentNode = nextNode;
                    nextNode = inPath[_currentPathNodeIndex];

                    // The 'previous' position is the tile which the agent is walking from.
                    _previousPosition = currentNode.TilePosition;

                    // Check if next tile is still walkable.
                    if (!NavTileManager.Instance.SurfaceManager.Data.IsTileWalkable(nextNode.TilePosition, AreaMask))
                    {
                        OnPathAbortedUnityEvent.Invoke(EAbortReason.EncounteredObstruction, nextNode.TilePosition);
                        yield break;
                    }

                    // Check for conflicts if enabled.
                    if (_conflictOption != EConflictOptions.Disabled)
                    {
                        // Check if there is a conflict on the next node.
                        EConflictStatus conflictStatus;
                        EAbortReason abortReason;

                        do
                        {
                            conflictStatus = HandleConflict(currentNode, nextNode, out abortReason);

                            // In case the conflict is not completely resolved, yield the coroutine and check again next frame.
                            if (conflictStatus == EConflictStatus.Processing)
                            {
                                _movementStatus = EMovementStatus.Waiting;
                                yield return null;
                            }
                            else if (conflictStatus == EConflictStatus.Abort) // In case the conflict cannot be resolved, abort path traversing.
                            {
                                _pathStatus = EPathStatus.NotAvailable;
                                _movementStatus = EMovementStatus.Stationary;

                                OnPathAbortedUnityEvent.Invoke(abortReason, nextNode.TilePosition);
                                yield break;
                            }
                        } while (conflictStatus == EConflictStatus.Processing); // If this is false, the 'do-while' will stop and thus a conflict has not been encountered.

                        // Set movementstatus just in case it has been changed due to a conflict.
                        _movementStatus = EMovementStatus.Traversing;
                    }

                    // Check whether the agent has changed areas.
                    int nextAreaIndex = nextNode.GetAreaIndex();
                    if (currentNode.GetAreaIndex() != nextAreaIndex)
                    {
                        currentSpeed = GetAreaSpeed(nextAreaIndex);
                        OnAreaChangeUnityEvent.Invoke(nextAreaIndex);
                    }

                    // Calculate the time which might be left over from the previous node to already move in towards the subsequent node.
                    // This prevents the agent to first exactly stop on a tile before starting to move again, creating jittering.
                    timeInCurrentNode += Time.deltaTime - timeToNextNode;
                    timeToNextNode = inPath.GetDurationBetweenNodes(currentNode, nextNode, currentSpeed);

                    transform.position = Vector3.Lerp(currentNode.WorldPosition, nextNode.WorldPosition, timeInCurrentNode / timeToNextNode);

                    // Set animation parameters
                    SetAnimationParameters(nextNode.WorldPosition - currentNode.WorldPosition, currentSpeed);

                    yield return null;
                }
                else // The next node has not been reached, so keep moving towards it.
                {
                    timeInPath += Time.deltaTime;
                    timeInCurrentNode += Time.deltaTime;

                    transform.position = Vector3.Lerp(currentNode.WorldPosition, nextNode.WorldPosition, timeInCurrentNode / timeToNextNode);
                    yield return null;
                }
            }
        }

        /// <summary>
        /// Function to check whether there is a conflict for this agent with the given parameters.
        /// </summary>
        /// <param name="inCurrentNode">The current node the agent is on.</param>
        /// <param name="inNextNode">The next node the agent might travel to.</param>
        /// <returns>A conflict status detailing how the conflict is handled.</returns>
        private EConflictStatus HandleConflict(PathNode inCurrentNode, PathNode inNextNode, out EAbortReason outAbortReason)
        {
            outAbortReason = EAbortReason.None;

            // Check if the next tile is free.
            TileData nextNodeTileData = NavTileManager.Instance.SurfaceManager.Data.GetTileData(inNextNode.TilePosition);

            NavTileAgent obstructingAgent;

            // Check if the next tile is obstructed.
            if (nextNodeTileData.IsObstructed(this, out obstructingAgent) || (_diagonalAllowed && IsMovingDiagonally(inCurrentNode.TilePosition, inNextNode.TilePosition) && IsPerpendicularObstructed(inCurrentNode.TilePosition, inNextNode.TilePosition, out obstructingAgent)))
            {
                // If so, set the position to be on the center of the tile.
                // The result of this function will stop moving the agent for at least one frame, so we correct it here instead.
                transform.position = inCurrentNode.WorldPosition;

                // If the conflict options is WaitOnWaitingAgent or higher, and the obstructing agent is waiting,
                // check if that agent isn't waiting indefinitely on this agent, itself or if there is some other circular conflict.
                if (_conflictOption >= EConflictOptions.WaitOnWaitingAgent && obstructingAgent.MovementStatus == EMovementStatus.Waiting)
                {
                    return CheckWaitingOccuypingAgents(inNextNode, obstructingAgent, new List<NavTileAgent>() { obstructingAgent }, ref outAbortReason);
                }

                // If the conflict option is WaitingOnTraversingAgent and the obstructing agent is traversing,
                // set the timer to be used later and specify which tile this agent is waiting for.  
                if (_conflictOption >= EConflictOptions.WaitOnTraversingAgent && obstructingAgent.MovementStatus == EMovementStatus.Traversing)
                {
                    _didEncounterObstruction = true;
                    _obstructionTimer = _waitAfterFreeTile;

                    _waitingForTile = NavTileManager.Instance.SurfaceManager.Data.GetTileData(inNextNode.TilePosition);

                    return EConflictStatus.Processing;
                }
                
                // If this agent should abort on any obstruction, or there has been no result from the previous if's, abort the path.
                if (_conflictOption >= EConflictOptions.AbortOnObstruction || obstructingAgent.MovementStatus != EMovementStatus.Traversing)
                {
                    outAbortReason = EAbortReason.EncounteredObstruction;
                    return EConflictStatus.Abort;
                }
            }
            else // No obstruction was found.
            {
                // Check if there was an obstruction since the last movement.
                if (_didEncounterObstruction && _conflictOption >= EConflictOptions.WaitOnTraversingAgent)
                {
                    // If so, start a timer.
                    _obstructionTimer -= Time.deltaTime;

                    if (_obstructionTimer > 0)
                        return EConflictStatus.Processing;
                }

                // Update the tiles this agent should be occupying.
                UpdateTileOccupancy(inCurrentNode.TilePosition, inNextNode.TilePosition);
            }

            // No conflicts were found and everything is handled. Return a Success.
            return EConflictStatus.Success;
        }

        /// <summary>
        /// Recursive function to check agents which this agent is waiting on.
        /// 
        /// There are a few situations where the agent should abort waiting.
        /// 
        /// 1. When this agent is waiting on an agent which is ultimately waiting for this agent.
        /// 2. When this agent is waiting for an agent which is involved in a circular conflict either itself, or some subsequent agent.
        /// 3. When this agent is waiting for a waiting agent which is ultimately waiting for a stationary agent.
        /// 4. When this agent is waiting for an agent who might be waiting, but its conflict handling setting is lower and therefor will never move again.
        /// 
        /// Some of these options can be solved just by letting others solve their conflicts first.
        /// </summary>
        /// <param name="inNextNode">The next node this agent might traverse to.</param>
        /// <param name="inObstructingAgent">The obstructing agent encountered.</param>
        /// <param name="inCheckedAgents">All Checked agents including the obstructed agent.</param>
        private EConflictStatus CheckWaitingOccuypingAgents(PathNode inNextNode, NavTileAgent inObstructingAgent, List<NavTileAgent> inCheckedAgents, ref EAbortReason refAbortReason)
        {
            // Get all agents occupying the tile which the obstructing agent is waiting for, sorted per MovementStatus.
            Dictionary<EMovementStatus, List<NavTileAgent>> sortedAgents = inObstructingAgent.WaitingForTile.GetSortedOccupyingAgents(inObstructingAgent);

            List<NavTileAgent> agents;

            // Check if there are any stationary agents. If so, abort this path.
            if (sortedAgents.TryGetValue(EMovementStatus.Stationary, out agents))
            {
                refAbortReason = EAbortReason.EncounteredStationaryAgent;
                return EConflictStatus.Abort;
            }
            else if (sortedAgents.TryGetValue(EMovementStatus.Waiting, out agents)) // Check if there are any waiting agents.
            {
                foreach (NavTileAgent agent in agents)
                {
                    // Is the agent found this agent or has it already been checked? Abort path
                    if (agent == this || inCheckedAgents.Contains(agent))
                    {
                        refAbortReason = EAbortReason.EncounteredCircularConflict;
                        return EConflictStatus.Abort;
                    }

                    // Add the agent to the list
                    inCheckedAgents.Add(agent);

                    // Do this again.
                    return CheckWaitingOccuypingAgents(inNextNode, agent, inCheckedAgents, ref refAbortReason);
                }
            }
            else // No entries of waiting or stationary agents.
            {
                // Set the timer to be used later and specify which tile this agent is waiting for.  
                _didEncounterObstruction = true;
                _obstructionTimer = _waitAfterFreeTile;

                _waitingForTile = NavTileManager.Instance.SurfaceManager.Data.GetTileData(inNextNode.TilePosition);
            }

            return EConflictStatus.Processing;
        }

        /// <summary>
        /// Updates the relevant tiles with the agent standing on it.
        /// </summary>
        /// <param name="inFreeTile">The tile position which is gonna be free.</param>
        /// <param name="inOccupiedTile">The tile position which is going to be occupied by the agent.</param>
        private void UpdateTileOccupancy(Vector2Int inFreeTilePosition, Vector2Int inOccupiedTilePosition)
        {
            TileData currentNodeTileData = NavTileManager.Instance.SurfaceManager.Data.GetTileData(inFreeTilePosition);
            TileData nextNodeTileData = NavTileManager.Instance.SurfaceManager.Data.GetTileData(inOccupiedTilePosition);

            currentNodeTileData.OccupyingAgents.Remove(this);
            nextNodeTileData.OccupyingAgents.Add(this);

            _occupyingTilePosition = inOccupiedTilePosition;
        }

        /// <summary>
        /// Called when a Path is found.
        /// </summary>
        private void OnPathFound()
        {
            OnPathFoundUnityEvent.Invoke(_path);

            if (AutoTraversePath)
            {
                StartFollowingPath();
            }
        }

        /// <summary>
        /// Checks whether the difference between the tiles and return true if it's a diagonal difference.
        /// </summary>
        private bool IsMovingDiagonally(Vector2Int inCurrentTilePosition, Vector2Int inNextTilePosition)
        {
            return (Mathf.Abs(inCurrentTilePosition.x - inNextTilePosition.x) == 1 && Mathf.Abs(inCurrentTilePosition.y - inNextTilePosition.y) == 1);
        }

        /// <summary>
        /// Checks whether the perpendicular tiles of a diagonal movement are obstructed.
        /// 
        /// E.g. in the case of AB tiles, where an agent traverses over tiles A, positions B1 and B2 get checked for an obstructing agent.
        ///                     BA 
        /// </summary>
        private bool IsPerpendicularObstructed(Vector2Int inCurrentTilePosition, Vector2Int inNextNodeTilePosition, out NavTileAgent outObstructingAgent)
        {
            outObstructingAgent = null;

            Vector2Int horizontalPosition = new Vector2Int(inCurrentTilePosition.x, inNextNodeTilePosition.y);
            Vector2Int verticalPosition = new Vector2Int(inNextNodeTilePosition.x, inCurrentTilePosition.y);

            TileData horizontalTileData = NavTileManager.Instance.SurfaceManager.Data.GetTileData(horizontalPosition);
            TileData verticalTileData = NavTileManager.Instance.SurfaceManager.Data.GetTileData(verticalPosition);

            NavTileAgent obstructingAgent;

            // Check whether there is an agent which is walking across the two relevant tiles.
            if (horizontalTileData.IsObstructed(this, out obstructingAgent) && obstructingAgent.GetPreviousPositionInPath() == verticalPosition
                || verticalTileData.IsObstructed(this, out obstructingAgent) && obstructingAgent.GetPreviousPositionInPath() == horizontalPosition)
            {
                outObstructingAgent = obstructingAgent;

                return true;
            }

            return false;
        }

        /// <summary>
        /// The last tile that was reached by the agent.
        /// </summary>
        public Vector2Int GetPreviousPositionInPath()
        {
            return _previousPosition;
        }

        /// <summary>
        /// Stops the current path if available, and start traversing the new one.
        /// </summary>
        public void StartFollowingPath()
        {
            if (_followingPathCoroutine != null)
            {
                StopCoroutine(_followingPathCoroutine);

                _pathStatus = EPathStatus.NotAvailable;
                _movementStatus = EMovementStatus.Stationary;
                UpdateTileOccupancy(_occupyingTilePosition, NavTileManager.Instance.SurfaceManager.Grid.WorldToCell(transform.position).GetVector2Int());
            }

            _followingPathCoroutine = StartCoroutine(FollowPath(_path));
        }

        /// <summary>
        /// Method to cancel the current path.
        /// </summary>
        [ContextMenu("Cancel Path")]
        public void CancelPath()
        {
            if (_followingPathCoroutine != null)
            {
                StopCoroutine(_followingPathCoroutine);

                _pathStatus = EPathStatus.NotAvailable;
                _movementStatus = EMovementStatus.Stationary;
                UpdateTileOccupancy(_occupyingTilePosition, NavTileManager.Instance.SurfaceManager.Grid.WorldToCell(transform.position).GetVector2Int());
            }

            _path = null;
        }

        /// <summary>
        /// Method to change the animation with a trigger coupled with an area.
        /// </summary>
        /// <param name="inNewArea">The area which is encountered.</param>
        private void ChangeAnimationOnArea(int inNewArea)
        {
            if (LinkedAnimator != null)
            {
                string triggerName = GetAreaAnimationTrigger(inNewArea);
                if (!string.IsNullOrEmpty(triggerName))
                    LinkedAnimator.SetTrigger(triggerName);
            }
        }

        /// <summary>
        /// Sets the parameters in the linked animator based on the selected fields.
        /// </summary>
        /// <param name="inMovementVector">Movement direction of the agent, will be normalized.</param>
        /// <param name="inSpeed">Current speed of the agent.</param>
        public void SetAnimationParameters(Vector3 inMovementVector, float inSpeed)
        {
            if (LinkedAnimator != null)
            {
                Vector3 localMovement = NavTileManager.Instance.SurfaceManager.Grid.transform.worldToLocalMatrix.MultiplyVector(inMovementVector);
                
                localMovement.z = 0;
                localMovement.Normalize();

                if (!string.IsNullOrEmpty(_animationSpeedParameter))
                    LinkedAnimator.SetFloat(_animationSpeedParameter, inSpeed);

                if (!string.IsNullOrEmpty(_animationHorizontalParameter))
                    LinkedAnimator.SetFloat(_animationHorizontalParameter, localMovement.x);

                if (!string.IsNullOrEmpty(_animationVerticalParameter))
                    LinkedAnimator.SetFloat(_animationVerticalParameter, localMovement.y);
            }
        }

        /// <summary>
        /// Sets only the speed parameter of the animator.
        /// </summary>
        /// <param name="inSpeed">The value that is going to be set in the animator.</param>
        public void SetAnimationSpeedParameter(float inSpeed)
        {
            if (LinkedAnimator != null && !string.IsNullOrEmpty(_animationSpeedParameter))
            {
                LinkedAnimator.SetFloat(_animationSpeedParameter, inSpeed);
            }
        }

        /// <summary>
        /// Resets animation parameters based on the _preserveAnimDirection variable.
        /// </summary>
        public void ResetAnimationParameters()
        {
            if (_preserveAnimDirection)
                SetAnimationSpeedParameter(0f);
            else
                SetAnimationParameters(Vector3.zero, 0f);
        }

        private void OnPathAbortSetAnimationParameters(EAbortReason inAbortReason, Vector2Int inAbortPosition)
        {
            ResetAnimationParameters();
        }

#if UNITY_EDITOR
        public void SetDebugVisualizer(NavTilePathVisualizer inVisualizer)
        {
            this.visualizer = inVisualizer;
        }
#endif

        /// <summary>
        /// Getter for the area settings using the area index.
        /// </summary>
        /// <param name="inAreaIndex">The area index used to retrieve the settings.</param>
        /// <returns>The settings corresponding to the area index.</returns>
        private AreaSettings GetAreaSettingsByID(int inAreaIndex)
        {
            return _areaSettingsList.Find((s) => s.AreaIndex == inAreaIndex);
        }

        /// <summary>
        /// Get the agent's speed on the area with the given index.
        /// </summary>
        public float GetAreaSpeed(int inAreaIndex)
        {
            AreaSettings settings = GetAreaSettingsByID(inAreaIndex);
            return (settings != null && settings.Speed != -1) ? settings.Speed : Speed;
        }

        /// <summary>
        /// Get the name of the animation trigger to play upon entering the area with the given index.
        /// </summary>
        public string GetAreaAnimationTrigger(int inAreaIndex)
        {
            AreaSettings settings = GetAreaSettingsByID(inAreaIndex);
            return (settings != null) ? settings.AnimationTriggerName : string.Empty;
        }

        /// <summary>
        /// Unity's OnDisable Method. Also called OnDestroy.
        /// </summary>
        private void OnDisable() 
        {
            // If conflict handling is enabled, remove this agent from the tile it is occupying.
            if (_conflictOption != EConflictOptions.Disabled)
            {
                NavTileManager.Instance.SurfaceManager.Data.GetTileData(_occupyingTilePosition).OccupyingAgents.Remove(this);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_debugEnabled || _path == null || _path.Count == 0 || _currentPathNodeIndex == _path.Count || MovementStatus == EMovementStatus.Stationary)
                return;
            
            float discRadius = 0.1f;

            Gizmos.color = _debugLineColor;
            Handles.color = _debugLineColor;

            Vector3 nextNodePos = _path[_currentPathNodeIndex].WorldPosition;

            Gizmos.DrawLine(transform.position, nextNodePos);
            Handles.DrawSolidDisc(transform.position, Vector3.back, discRadius);
            Handles.DrawSolidDisc(nextNodePos, Vector3.back, discRadius);

            for (int i = _currentPathNodeIndex; i < _path.Count - 1; i++)
            {
                Vector3 from = new Vector3(_path[i].WorldPosition.x, _path[i].WorldPosition.y);
                Vector3 to = new Vector3(_path[i + 1].WorldPosition.x, _path[i + 1].WorldPosition.y);
                Gizmos.DrawLine(from, to);
                Handles.DrawSolidDisc(_path[i + 1].WorldPosition, Vector3.back, discRadius);
            }
        }
#endif
    }
}
