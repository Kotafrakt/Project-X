using System.Collections.Generic;
using UnityEngine;

namespace Snowcap.NavTiles
{
    public class NavTilePath : List<PathNode>
    {
        /// <summary>
        /// Area mask used to calculate the path.
        /// Currently used for smoothing.
        /// </summary>
        public int AreaMask;

        /// <summary>
        /// Adds a path node to the end of the path.
        /// </summary>
        /// <param name="inTilePosition">Grid coordinate of the node.</param>
        public void Add(Vector2Int inTilePosition)
        {
            Add(new PathNode(inTilePosition));
        }

        /// <summary>
        /// Adds a path node to the end of the path.
        /// </summary>
        /// <param name="inWorldPosition">World position within a tile.</param>
        public void Add(Vector3 inWorldPosition)
        {
            Add(new PathNode(inWorldPosition));
        }

        /// <summary>
        /// Gets the time between two nodes when traversing with an average speed.
        /// </summary>
        /// <param name="inNodeOne">First node to calculate with.</param>
        /// <param name="inNodeTwo">Second node to calculate with.</param>
        /// <param name="inSpeed">Average speed used to traverse the distance.</param>
        /// <returns>Time in seconds to go from the first node to the second with specified speed.</returns>
        public float GetDurationBetweenNodes(PathNode inNodeOne, PathNode inNodeTwo, float inSpeed)
        {
            return (inNodeOne.WorldPosition - inNodeTwo.WorldPosition).magnitude / inSpeed;
        }
    }
}
