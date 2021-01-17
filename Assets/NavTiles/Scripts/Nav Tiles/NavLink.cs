using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Holds info of a navigation area linked to a tile. Assets should be located in resources folder.
    /// </summary>
    [CreateAssetMenu(fileName = "New NavLink", menuName = "NavLink", order = 359)]
    public class NavLink : ScriptableObject, IHasNavTileArea
    {
        /// <summary>
        /// Tile which the selected area is linked to.
        /// </summary>
        public TileBase LinkedTile;
        /// <summary>
        /// Area linked to the tile.
        /// </summary>
        public NavTileArea Area { get { return NavTileManager.Instance.AreaManager.GetAreaByID(AreaIndex); } }
        [HideInInspector]
        public int AreaIndex;

#if UNITY_EDITOR
        /// <summary>
        /// Called when a different tile is selected in the inspector.
        /// </summary>
        /// <param name="inPreviousTile">Previously selected tile.</param>
        /// <param name="inNewTile">Newly selected tile.</param>
        public void OnTileChanged(TileBase inPreviousTile, TileBase inNewTile)
        {
            NavLinkManager manager = NavTileManager.Instance.LinkManager;
            NavLink presentLink = manager.GetLinkedTile(inPreviousTile);

            // Remove any present data.
            if (presentLink == this)
            {
                // Tile present, remove it.
                manager.RemoveNavLink(inPreviousTile);
            }

            // Add new data.
            if (inNewTile != null)
            {
                manager.AddNavLink(inNewTile, this);
            }

            // Save dictionary and changed tile.
            EditorUtility.SetDirty(NavTileManager.Instance);
            EditorUtility.SetDirty(this);
        }
#endif

        public int GetNavTileAreaIndex()
        {
            return AreaIndex;
        }
    }
}
