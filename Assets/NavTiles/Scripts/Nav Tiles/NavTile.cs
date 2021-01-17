using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// A Tile which holds extra navigation information.
    /// </summary>
    [CreateAssetMenu(fileName = "New NavTile", menuName = "NavTile", order = 358)]
    public class NavTile : Tile, IHasNavTileArea
    {
        /// <summary>
        /// The index of the area used for this tile.
        /// </summary>
        public int AreaIndex;

        /// <summary>
        /// Area information based on the area index of the tile (read-only).
        /// </summary>
        public NavTileArea Area { get { return NavTileManager.Instance.AreaManager.GetAreaByID(AreaIndex); } }

        public override bool StartUp(Vector3Int inPosition, ITilemap inTilemap, GameObject inGameObject)
        {
            return base.StartUp(inPosition, inTilemap, inGameObject);
        }

        public override void RefreshTile(Vector3Int inPosition, ITilemap inTilemap)
        {
            base.RefreshTile(inPosition, inTilemap);
        }

        public override void GetTileData(Vector3Int inPosition, ITilemap inTilemap, ref UnityEngine.Tilemaps.TileData inTileData)
        {
            base.GetTileData(inPosition, inTilemap, ref inTileData);
        }

        /// <summary>
        /// Gets the set area index implementing the IHasNavTileArea interface.
        /// </summary>
        public int GetNavTileAreaIndex()
        {
            return AreaIndex;
        }

        /// <summary>
        /// Copies data from a normal tile to the NavTile.
        /// </summary>
        /// <param name="inTile">Tile to copy data from.</param>
        public void CreateFromTile(Tile inTile)
        {
            sprite = inTile.sprite;
            color = inTile.color;
            colliderType = inTile.colliderType;
            flags = inTile.flags;
            name = inTile.name;
        }
    }
}
