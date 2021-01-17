using Snowcap.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Manages all links between tiles and their corresponding NavTile2D object.
    /// </summary>
    [Serializable]
    public class NavLinkManager
    {
        /// <summary>
        /// Dictionary to hold data about links between a tile and their navigation info.
        /// </summary>
        [Serializable]
        private class NavLinkDictionary : SerializableDictionary<TileBase, NavLink>
        {
            public override void OnAfterDeserialize()
            {
                for (int i = 0; i < _keys.Count; i++)
                {
                    if (_keys[i] == null || _values[i] == null)
                    {
                        _keys.RemoveAt(i);
                        _values.RemoveAt(i);
                    }
                }

                base.OnAfterDeserialize();
            }
        }

        [SerializeField]
        private NavLinkDictionary _navLinkDictionary;

        /// <summary>
        /// Gets all NavLink pairs currently registered.
        /// </summary>
        /// <returns>List of all pairs of NavLinks.</returns>
        public List<KeyValuePair<TileBase, NavLink>> GetAllPairs()
        {
            return _navLinkDictionary.ToList();
        }

        public bool ContainsTileLink(TileBase inTile)
        {
            return _navLinkDictionary.ContainsKey(inTile);
        }

        /// <summary>
        /// Gets the index of a NavTileArea linked to a generic tile. This takes NavTiles and links into account.
        /// </summary>
        /// <param name="inTile">Tile to get the area index for. Passing null will return null.</param>
        /// <returns>The area index. -1 if no area is found.</returns>
        public static int GetNavTileAreaIndexForTile(TileBase inTile)
        {
            if (inTile == null)
                return -1;

            if (inTile is IHasNavTileArea)
            {
                return (inTile as IHasNavTileArea).GetNavTileAreaIndex();
            }

            return NavTileManager.Instance.LinkManager.GetLinkedAreaIndex(inTile);
        }

        /// <summary>
        /// Gets the NavTileArea from a generic tile. This takes NavTiles and links into account.
        /// </summary>
        /// <param name="inTile">Tile to get the area for. Passing null will return null.</param>
        /// <returns>Null if no NavTileArea is linked to the tile.</returns>
        public static NavTileArea GetNavTileAreaForTile(TileBase inTile)
        {
            return NavTileManager.Instance.AreaManager.GetAreaByID(GetNavTileAreaIndexForTile(inTile));
        }

        /// <summary>
        /// Gets the linked NavTile if it is stored, otherwise returns null.
        /// </summary>
        /// <param name="inTile">Tile to find a link with.</param>
        /// <returns>The NavLink linked to the tile or null if no link was found.</returns>
        public NavLink GetLinkedTile(TileBase inTile)
        {
            if (inTile == null)
                return null;

            NavLink navTile = null;

            if (_navLinkDictionary.TryGetValue(inTile, out navTile))
            {
                return navTile;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove a NavLink if it is present.
        /// </summary>
        /// <param name="inTile">Tile to remove the link from.</param>
        public void RemoveNavLink(TileBase inTile)
        {
            if (_navLinkDictionary.ContainsKey(inTile))
            {
                _navLinkDictionary.Remove(inTile);
            }
        }

        /// <summary>
        /// Add a NavLink to the collection. Ignores null and duplicates.
        /// </summary>
        /// <param name="inTile">Tile to link with.</param>
        /// <param name="inLink">Linked NavTile object.</param>
        public void AddNavLink(TileBase inTile, NavLink inLink)
        {
            if (inTile != null && inLink != null)
            {
                if (_navLinkDictionary.ContainsKey(inTile))
                {
                    Debug.LogWarning("Tried to add already present tile.", inLink);
                    return;
                }

                _navLinkDictionary.Add(inTile, inLink);
            }
            else
            {
                Debug.LogWarning("Tried to add null tile to NavLinkManager.", inLink);
            }
        }

        /// <summary>
        /// Gets the linked NavTileArea if it is stored, otherwise returns null.
        /// </summary>
        /// <param name="inTile">Tile to find the linked area with.</param>
        /// <returns>The NavTileArea index linked to the tile or -1 if no link was found.</returns>
        public int GetLinkedAreaIndex(TileBase inTile)
        {
            NavLink link = GetLinkedTile(inTile);

            if (link == null)
                return -1;

            return link.AreaIndex;
        }

        /// <summary>
        /// Finds and stores all NavLink assets in the resource folder.
        /// </summary>
        public List<NavLink> RefreshAllNavLinks()
        {
            NavLinkDictionary previousDictionary = _navLinkDictionary;
            _navLinkDictionary = new NavLinkDictionary();
            List<NavLink> duplicateLinks = new List<NavLink>();
            
            UnityEngine.Object[] linkObjects = Resources.LoadAll("", typeof(NavLink));

            foreach (UnityEngine.Object linkObj in linkObjects)
            {
                NavLink link = (NavLink)linkObj;

                // Ignore null values.
                if (link.LinkedTile == null)
                    continue;

                // Ignore duplicates.
                if (_navLinkDictionary.ContainsKey(link.LinkedTile))
                {
                    duplicateLinks.Add(link);
                    Debug.LogWarning("Duplicate NavLink found, a tile can only be linked once", link);
                    continue;
                }

                // Remove found links from previous dictionary to have the non-resource folder links left.
                if (previousDictionary.ContainsKey(link.LinkedTile))
                {
                    previousDictionary.Remove(link.LinkedTile);
                }

                _navLinkDictionary.Add(link.LinkedTile, link);
            }

            // Add the non-resource folder links to the new dictionary.
            foreach (var linkPair in previousDictionary)
            {
                if (linkPair.Key != null && linkPair.Value != null && !_navLinkDictionary.ContainsKey(linkPair.Key))
                {
                    _navLinkDictionary.Add(linkPair.Key, linkPair.Value);
                }
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(NavTileManager.Instance);
#endif

            return duplicateLinks;
        }
    }
}
