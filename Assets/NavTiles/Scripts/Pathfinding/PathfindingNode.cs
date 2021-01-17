using UnityEngine;
using Snowcap.Utilities;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// This is used during pathfinding, holding costs and important data.
    /// </summary>
    public class PathfindingNode : IHeapItem<PathfindingNode>
    {
        /// <summary>
        /// Node reference to which this node is linked.
        /// Used for retracing.
        /// </summary>
        public PathfindingNode ParentNode;

        private int _gCost;
        /// <summary>
        /// Movement cost of the node.
        /// Includes movement penalty (weight).
        /// </summary>
        public int GCost
        {
            get
            {
                return _gCost;
            }
            set
            {
                _gCost = value;
            }
        }

        private int _hCost;
        /// <summary>
        /// Heuristic cost of the node.
        /// </summary>
        public int HCost
        {
            get
            {
                return _hCost;
            }
            set
            {
                _hCost = value;
            }
        }

        public int FCost
        {
            get
            {
                return GCost + HCost;
            }
        }

        private Vector2Int _coordinate;
        /// <summary>
        /// Coordinate of the node on the grid.
        /// </summary>
        public Vector2Int Coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        private int _heapIndex;
        /// <summary>
        /// Index of this node within the heap.
        /// </summary>
        public int HeapIndex
        {
            get
            {
                return _heapIndex;
            }
            set
            {
                _heapIndex = value;
            }
        }

        private int _tileCost;
        /// <summary>
        /// Cost associated with the tile data at this coordinate.
        /// </summary>
        public int TileCost
        {
            get
            {
                return _tileCost;
            }
            set
            {
                _tileCost = value;
            }
        }

        private AdditionalJPSPlusData _additionalData;
        /// <summary>
        /// Data used for jump point search+ pathfinding.
        /// </summary>
        public AdditionalJPSPlusData AdditionalTileData
        {
            get
            {
                return _additionalData;
            }
            set
            {
                _additionalData = value;
            }
        }

        /// <summary>
        /// Constructs a new empty PathfindingNode.
        /// </summary>
        /// <param name="inCoordinate">Coordinate of the node.</param>
        public PathfindingNode(Vector2Int inCoordinate)
        {
            this._coordinate = inCoordinate;
        }

        public int CompareTo(PathfindingNode inNodeToCompare)
        {
            int compare = FCost.CompareTo(inNodeToCompare.FCost);

            if (compare == 0)
                compare = HCost.CompareTo(inNodeToCompare.HCost);

            // Flip the compare because we want to sort from lowest to highest.
            return -compare;
        }
    }
}
