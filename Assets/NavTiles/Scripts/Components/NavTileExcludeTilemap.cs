using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Use this component to mark a tilemap for exclusion from the navigation grid.
    /// </summary>
    [RequireComponent(typeof(Tilemap))]
    [AddComponentMenu("NavTile/Nav Tile Exclude Tilemap")]
    public class NavTileExcludeTilemap : MonoBehaviour { }
}
