using Snowcap.Extensions;
using UnityEngine;

namespace Snowcap.NavTiles
{
    public class PathNode
    {
        /// <summary>
        /// World position within the node.
        /// </summary>
        public Vector3 WorldPosition;

        /// <summary>
        /// Grid coordinate of the node.
        /// </summary>
        public Vector2Int TilePosition;

        /// <summary>
        /// Constructs a new path node from a world position.
        /// </summary>
        /// <param name="inWorldPosition">The world position within a tile.</param>
        public PathNode(Vector3 inWorldPosition)
        {
            WorldPosition = inWorldPosition;
            TilePosition = NavTileManager.Instance.SurfaceManager.Grid.WorldToCell(inWorldPosition).GetVector2Int();
        }

        /// <summary>
        /// Constructs a new path node from a tile coordinate. The world position will be its center.
        /// </summary>
        /// <param name="inTilePosition">Coordinate of the tile on the navigation grid.</param>
        public PathNode(Vector2Int inTilePosition)
        {
            TilePosition = inTilePosition;
            WorldPosition = NavTileManager.Instance.SurfaceManager.GridInfo.GetCenterOfCell(inTilePosition);
        }

        /// <summary>
        /// Retrieves the current area index on this tile position.
        /// -1 if no TileData is found at this position.
        /// </summary>
        /// <returns>The area index on this tile position. -1 if no TileData is found.</returns>
        public int GetAreaIndex()
        {
            TileData tileData = NavTileManager.Instance.SurfaceManager.Data.GetTileData(TilePosition);

            if (tileData != null)
                return tileData.AreaIndex;

            return -1;
        }
    }
}
