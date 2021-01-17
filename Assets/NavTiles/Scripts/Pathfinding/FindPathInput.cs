using System.Collections.Generic;
using UnityEngine;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Wrapper for variables needed for pathfinding.
    /// </summary>
    public class FindPathInput
    {
        private Vector2Int _startCoordinate;
        /// <summary>
        /// The starting coordinate of the requested path.
        /// </summary>
        public Vector2Int StartCoordinate
        {
            get
            {
                return _startCoordinate;
            }
        }

        private Vector2Int _targetCoordinate;
        /// <summary>
        /// The target coordinate of the requested path.
        /// </summary>
        public Vector2Int TargetCoordinate
        {
            get
            {
                return _targetCoordinate;
            }
        }

        private bool _diagonalAllowed;
        /// <summary>
        /// Is diagonal movement allowed for this path.
        /// </summary>
        public bool DiagonalAllowed
        {
            get
            {
                return _diagonalAllowed;
            }
        }

        private bool _cutCorners;
        /// <summary>
        /// Is it allowed to cut corners. 
        /// This happens when moving diagonally past a non-walkable tile.
        /// </summary>
        public bool CutCorners
        {
            get
            {
                return _cutCorners;
            }
        }

        private bool _ignoreTileCost;
        /// <summary>
        /// Should tile cost be ignored.
        /// </summary>
        public bool IgnoreTileCost
        {
            get
            {
                return _ignoreTileCost;
            }
        }

        private int _areaMask;
        /// <summary>
        /// Area mask to check walkable and non-walkable tiles for.
        /// </summary>
        public int AreaMask
        {
            get
            {
                return _areaMask;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Optional visualizer component to visualize examined nodes.
        /// </summary>
        public NavTilePathVisualizer Visualizer;
#endif

        private HashSet<Vector2Int> _positionToAvoid = new HashSet<Vector2Int>();
        /// <summary>
        /// Grid positions to handle as non-walkable regardless of the tile's data.
        /// </summary>
        public HashSet<Vector2Int> PositionsToAvoid
        {
            get
            {
                return _positionToAvoid;
            }
        }

        /// <summary>
        /// Constructor of a FindPathInput.
        /// </summary>
        /// <param name="inStartCoordinate">Start coordinate of the path.</param>
        /// <param name="inTargetCoordinate">Target coordinate of the path.</param>
        /// <param name="inAreaMask">Area mask to check walkable and non-walkable tiles for.</param>
        /// <param name="inDiagonalAllowed">Is diagonal movement allowed for this path?</param>
        /// <param name="inCutCorners">Is it allowed to cut corners?</param>
        /// <param name="inIgnoreTileCost">Should tile cost be ignored?</param>
        /// <param name="inPositionsToAvoid">Grid positions to handle as non-walkable regardless of the tile's data.</param>
        public FindPathInput(Vector2Int inStartCoordinate, Vector2Int inTargetCoordinate, int inAreaMask = 1, bool inDiagonalAllowed = false, bool inCutCorners = false, bool inIgnoreTileCost = false, params Vector2Int[] inPositionsToAvoid)
        {
            this._startCoordinate = inStartCoordinate;
            this._targetCoordinate = inTargetCoordinate;
            this._areaMask = inAreaMask;
            this._diagonalAllowed = inDiagonalAllowed;
            this._cutCorners = inCutCorners;
            this._ignoreTileCost = inIgnoreTileCost;

            foreach (Vector2Int tilePosition in inPositionsToAvoid)
            {
                _positionToAvoid.Add(tilePosition);
            }
        }
    }
}
