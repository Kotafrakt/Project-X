using System.Collections.Generic;
using UnityEngine;
using Snowcap.Utilities;
using System.Linq;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// A heap for NavNodes. This also keeps track of added nodes in a dictionary for O(1) look up.
    /// </summary>
    public class NavNodeHeap : DynamicHeap<PathfindingNode>
    {
        /// <summary>
        /// The initial capacity for the underlying list. 
        /// Making this too small will trigger a lot of resizes.
        /// Making this too big will require a big memory allocation for a possibly small path.
        /// </summary>
        private const int INITIAL_LIST_CAPACITY = 128;

        // Hashset would be beter but need .NET 4.7.2 for a retrieve function.
        /// <summary>
        /// Dictionary to keep track of added nodes to prevent an O(n) operation for Contains.
        /// </summary>
        private Dictionary<Vector2Int, PathfindingNode> _registeredItems = new Dictionary<Vector2Int, PathfindingNode>();

        public NavNodeHeap() : base(INITIAL_LIST_CAPACITY) { }

        /// <summary>
        /// Add an item to the heap and registered items.
        /// </summary>
        /// <param name="inItem">Node to add.</param>
        public override void Add(PathfindingNode inItem)
        {
            // No double items allowed.
            if (this.Contains(inItem))
                return;

            _registeredItems.Add(inItem.Coordinate, inItem);

            base.Add(inItem);
        }

        /// <summary>
        /// Get the first node in the heap. 
        /// This is the node with the lowest total cost.
        /// </summary>
        /// <returns>The first node with the lowest cost.</returns>
        public override PathfindingNode RemoveFirst()
        {
            PathfindingNode removedNode = base.RemoveFirst();
            _registeredItems.Remove(removedNode.Coordinate);
            return removedNode;
        }

        /// <summary>
        /// Checks whether this node is already present in de collection using the dictionary.
        /// </summary>
        /// <param name="inItem">Node to check for.</param>
        /// <returns>Whether this node is already in the collection.</returns>
        public override bool Contains(PathfindingNode inItem)
        {
            return _registeredItems.ContainsKey(inItem.Coordinate);
        }
         
        /// <summary>
        /// Gets a node if it is present in the collection. Otherwise null.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to get the node for.</param>
        /// <returns>The found node or null if none found.</returns>
        public PathfindingNode GetExistingNode(Vector2Int inCoordinate)
        {
            PathfindingNode node = null;

            if (_registeredItems.TryGetValue(inCoordinate, out node))
                return node;

            return null;
        }

        /// <summary>
        /// Get all registered coordinates.
        /// </summary>
        /// <returns>List of node coordinates in the collection.</returns>
        public List<Vector2Int> GetCoordinateList()
        {
            return _registeredItems.Keys.ToList();
        }
    }
}
