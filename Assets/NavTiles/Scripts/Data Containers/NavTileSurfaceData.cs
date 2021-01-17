using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Snowcap.Utilities;
using System.Linq;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Holds scene-related NavTile data.
    /// Each scene has his own data object holding all NavTile information for that scene.
    /// </summary>
    [Serializable]
    public class NavTileSurfaceData : ScriptableObject
    {
        /// <summary>
        /// Used to determine the current bake type so the user knows which is currently baked.
        /// </summary>
        public enum BakeState
        {
            Unbaked,
            Standard,
            AdditionalData
        }

        /// <summary>
        /// Dictionary to hold and serialize data about NavTiles. This is the main NavTile data storage.
        /// </summary>
        [Serializable]
        public class TileDictionary : SerializableConcurrentDictionary<Vector2Int, TileData> { }

        [SerializeField]
        [HideInInspector]
        private TileDictionary _tiles;
        /// <summary>
        /// The tiles dictionary holding NavTile information (read-only).
        /// </summary>
        public TileDictionary Tiles
        {
            get
            {
                if (_tiles == null)
                {
                    _tiles = new TileDictionary();
                }

                return _tiles;
            }
        }

        /// <summary>
        /// Tracks the current bake type.
        /// </summary>
        public BakeState CurrentBakeState = BakeState.Unbaked;

        [SerializeField]
        [HideInInspector]
        private Vector2IntRect _gridBounds;
        /// <summary>
        /// Grid bounds of the NavTile data. Updates automatically (read-only).
        /// </summary>
        public Vector2IntRect GridBounds { get { return _gridBounds; } }

        /// <summary>
        /// Whether or not the dictionary is empty.
        /// Uses a lock-free version using an enumerator.
        /// </summary>
        public bool HasNoTiles { get { return !Tiles.Any(); } }

        /// <summary>
        /// Get the total amount of tiles.
        /// Uses a lock-free version using an enumerator.
        /// </summary>
        public int TileAmount { get { return Tiles.Skip(0).Count(); } }

        /// <summary>
        /// Resets and initializes the tile dictionary.
        /// </summary>
        public void InitTilesDictionary()
        {
            _tiles = new TileDictionary();
        }

        /// <summary>
        /// Saves the asset to disk. Only in editor.
        /// </summary>
        public void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }

        /// <summary>
        /// Tries to add a tile to the collection.
        /// Fails when the priority of the tile is lower than the already present one.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to add or change area index for.</param>
        /// <param name="inAreaIndex">Index of the area to add to the coordinate.</param>
        public void TryAddTile(Vector2Int inCoordinate, int inAreaIndex)
        {
            if (!NavTileAreaManager.IsAreaIndexValid(inAreaIndex))
                return;

            if (Tiles.ContainsKey(inCoordinate))
            {
                NavTileArea area = NavTileManager.Instance.AreaManager.GetAreaByID(inAreaIndex);

                TileData presentData = Tiles[inCoordinate];

                if (presentData.Area.Priority <= area.Priority)
                {
                    // Override if priority is higher
                    presentData.AreaIndex = inAreaIndex;
                }
            }
            else
            {
                Tiles[inCoordinate] = new TileData(inAreaIndex);

                UpdateBounds(inCoordinate);
            }
        }

        /// <summary>
        /// Overrides a tile regardless of priority.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to override area index for.</param>
        /// <param name="inAreaIndex">Area index to override with, -1 will remove any present tile area.</param>
        public void OverrideTile(Vector2Int inCoordinate, int inAreaIndex)
        {
            if (inAreaIndex == -1)
            {
                RemoveTile(inCoordinate);
                return;
            }

            if (!NavTileAreaManager.IsAreaIndexValid(inAreaIndex))
                return;

            if (Tiles.ContainsKey(inCoordinate))
            {
                Tiles[inCoordinate].AreaIndex = inAreaIndex;
            }
            else
            {
                Tiles[inCoordinate] = new TileData(inAreaIndex);
            }
        }

        /// <summary>
        /// Removes tile info from the collection if it exists.
        /// Tile becomes unwalkable.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to remove area info for.</param>
        /// <returns>True if removal was succesful.</returns>
        public bool RemoveTile(Vector2Int inCoordinate)
        {
            TileData removedData;
            return Tiles.TryRemove(inCoordinate, out removedData);
        }

        /// <summary>
        /// Refreshes a tile to the area with the highest priority.
        /// This can be used if a tile was previously overridden and has to be reset.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to remove area info for.</param>
        public void RefreshTile(Vector2Int inCoordinate)
        {
            TileBase[] tiles = NavTileManager.Instance.SurfaceManager.GetAllTilesAtPosition(inCoordinate);

            int highestAreaIndex = -1;

            for (int i = 0; i < tiles.Length; i++)
            {
                int areaIndex = NavLinkManager.GetNavTileAreaIndexForTile(tiles[i]);
                NavTileArea navArea = NavTileManager.Instance.AreaManager.GetAreaByID(areaIndex);

                if (navArea == null)
                    continue;

                if (highestAreaIndex == -1 || 
                    navArea.Priority > NavTileManager.Instance.AreaManager.GetAreaByID(highestAreaIndex).Priority)
                {
                    highestAreaIndex = areaIndex;
                }
            }

            if (highestAreaIndex >= 0)
            {
                OverrideTile(inCoordinate, highestAreaIndex);
            }
            else
            {
                RemoveTile(inCoordinate);
            }
        }

        /// <summary>
        /// Gets the area info of a tile at a coordinate. 
        /// Can return null if no tile is found.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to get data from.</param>
        /// <returns>TileData at the specified coordinate. Null if no data was found.</returns>
        public TileData GetTileData(Vector2Int inCoordinate)
        {
            TileData tileData = null;
            if (Tiles.TryGetValue(inCoordinate, out tileData))
                return tileData;

            return null;
        }

        /// <summary>
        /// Checks if a tile is walkable at a coordinate. 
        /// Null tiles are marked as unwalkable.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to check walkability.</param>
        /// <param name="inAreaMask">Area mask to check with.</param>
        /// <returns>Whether or not the tile is walkable.</returns>
        public bool IsTileWalkable(Vector2Int inCoordinate, int inAreaMask)
        {
            TileData theTile = GetTileData(inCoordinate);

            if (theTile == null)
                return false;

            return theTile.IsWalkable(inAreaMask);
        }

        /// <summary>
        /// Update bounds based on the added coordinate.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to extend the bounds with.</param>
        private void UpdateBounds(Vector2Int inCoordinate)
        {
            if (_gridBounds == null)
            {
                _gridBounds = new Vector2IntRect(inCoordinate);
            }
            else
            {
                _gridBounds.UpdateRect(inCoordinate);
            }
        }

        /// <summary>
        /// Clear the tile dictionary and save.
        /// </summary>
        public void ClearTiles()
        {
            Tiles.Clear();
            _gridBounds = null;
            Save();
        }
    }
}
