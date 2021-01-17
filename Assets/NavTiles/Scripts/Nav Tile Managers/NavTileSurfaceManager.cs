using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.SceneManagement;
using Snowcap.Extensions;
using System;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Scene-related data holder for the grid.
    /// </summary>
    public class NavTileSurfaceManager
    {
        private NavTileSurfaceData _data;
        /// <summary>
        /// Object holding all the NavTile data for the current scene.
        /// This can be used if you want to dynamically change the NavTile grid or get information from it.
        /// </summary>
        public NavTileSurfaceData Data
        {
            get
            {
                if (_data == null)
                {
                    _data = GetData(false);

                    if (_data == null)
                        Debug.LogWarning("NavTile data is requested but not present for this scene. Please bake.");
                }

                return _data;
            }
        }

        /// <summary>
        /// Checks if data is present for this scene without a warning.
        /// </summary>
        public bool IsDataInitialized
        {
            get
            {
                if (_data == null)
                    _data = GetData(false);

                return _data != null;
            }
        }

        private Grid _grid;
        /// <summary>
        /// The grid object associated with the scene and the NavTile data.
        /// This can be used to transform position from the world to the NavTile grid or vice versa.
        /// </summary>
        public Grid Grid
        {
            get
            {
                if (_grid == null)
                {
#if UNITY_EDITOR
                    if (BuildPipeline.isBuildingPlayer)
                        return null;
#endif
                    Grid[] grids = GameObject.FindObjectsOfType<Grid>();
                    if (grids.Length != 1)
                        return null;

                    _grid = grids[0];

                    ReInitializeGridInfo();
                }

                return _grid;
            }
        }

        private GridInfo _gridInfo;
        /// <summary>
        /// This custom gridinfo holder is used to do multithreaded calculations.
        /// This is because Unity's grid is not thread safe.
        /// </summary>
        public GridInfo GridInfo
        {
            get
            {
                if (_gridInfo == null)
                    ReInitializeGridInfo();

                return _gridInfo;
            }
        }

        public void ReInitializeGridInfo()
        {
            _gridInfo = new GridInfo(Grid);
        }

        private List<Tilemap> _navTileMaps;
        /// <summary>
        /// All tilemap objects used to create NavTile data.
        /// This excludes all the tilemaps marked with the NavTileExcludeTilemap component.
        /// </summary>
        public List<Tilemap> NavTileMaps
        {
            get
            {
                if (_navTileMaps == null)
                    GetAllTilemaps();

                return _navTileMaps;
            }
        }

        /// <summary>
        /// Gets the NavTile data associated with the scene or creates if not found.
        /// </summary>
        public void InitDataForCurrentScene()
        {
            _data = GetData(true);
        }

        public void OnEnable()
        {
#if UNITY_EDITOR
            EditorSceneManager.sceneOpened += OnSceneChanged;
#else
            SceneManager.activeSceneChanged += OnSceneChanged;
#endif
        }

        public void OnDisable()
        {
#if UNITY_EDITOR
            EditorSceneManager.sceneOpened -= OnSceneChanged;
#else
            SceneManager.activeSceneChanged -= OnSceneChanged;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// This function is used to switch data and grid references on scene changes.
        /// </summary>
        private void OnSceneChanged(Scene inScene, OpenSceneMode inMode)
        {
            _gridInfo = null;
            _data = GetData(false);
        }
#else
        private void OnSceneChanged(Scene inPreviousScene, Scene inNextScene)
        {
            _gridInfo = null;
            _data = GetData(false);
        }
#endif

        /// <summary>
        /// Bakes all tiles into the NavTile dictionary for the current scene.
        /// </summary>
        public void Bake(IPathfindingAlgorithm inAlgorithm)
        {
            GridInfo.InitializeGridInfo(Grid);

            InitDataForCurrentScene();

            if (GetAllTilemaps())
            {
                GenerateNavTileData();

                Data.CurrentBakeState = NavTileSurfaceData.BakeState.Standard;

                if (inAlgorithm is JPSPlus)
                {
                    (inAlgorithm as JPSPlus).GenerateAdditionalTileData(Data);
                    Data.CurrentBakeState = NavTileSurfaceData.BakeState.AdditionalData;
                }

#if UNITY_EDITOR
                EditorUtility.DisplayProgressBar("Saving Baked NavData", $"Saving NavData", 0.5f);
#endif

                Data.Save();

#if UNITY_EDITOR
                EditorUtility.ClearProgressBar();
#endif
            }
        }

        /// <summary>
        /// Find all tilemaps in the scene and exclude tilemaps marked with NavTile2DExclude.
        /// </summary>
        /// <returns>False if no tilemaps were found.</returns>
        private bool GetAllTilemaps()
        {
            _navTileMaps = GameObject.FindObjectsOfType<Tilemap>().ToList();

            _navTileMaps.RemoveAll((map) => map.GetComponent<NavTileExcludeTilemap>() != null);

            if (_navTileMaps.Count == 0)
            {
                Debug.LogWarning("No tilemaps found in the active scene.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Populate the NavTile data with tiles.
        /// </summary>
        public void GenerateNavTileData()
        {
            Data.ClearTiles();
            Data.InitTilesDictionary();

            for (int i = 0; i < NavTileMaps.Count; i++)
            {
                Tilemap aTilemap = NavTileMaps[i];

#if UNITY_EDITOR
                EditorUtility.DisplayProgressBar("Baking NavTiles", $"Baking {aTilemap.name}", (float)i / (NavTileMaps.Count - 1));
#endif

                // Compress bounds to speed up baking. If extended bounds are needed, remove this call.
                aTilemap.CompressBounds();

                BoundsInt bounds = aTilemap.cellBounds;

                // Fetch all tiles for current tilemap.
                TileBase[] tileBases = aTilemap.GetTilesBlock(bounds);
                int sliceSize = bounds.size.x * bounds.size.z;

                for (int j = 0; j < tileBases.Length; j++)
                {
                    TileBase aTile = tileBases[j];

                    // Get the area index for this tile.
                    // -1 will be returned if this tile is not integrated with NavTile in any way.
                    int tileAreaIndex = NavLinkManager.GetNavTileAreaIndexForTile(aTile);

                    // Skip non-integrated tiles.
                    if (tileAreaIndex == -1)
                        continue;

                    // Calculate 3D coordinate based on 1D array position.
                    int y = j / sliceSize;
                    int leftOver = j - (y * sliceSize);
                    int z = leftOver % bounds.size.z;
                    int x = leftOver / bounds.size.z;

                    Vector3Int localCoordinate = new Vector3Int(bounds.x + x, bounds.y + y, bounds.z + z);
                    Vector2Int navCoordinate = localCoordinate.GetVector2Int();

                    Data.TryAddTile(navCoordinate, tileAreaIndex);
                }
            }

#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif

            if (Data.HasNoTiles)
                Debug.LogWarning("No nav tiles were found on the tilemaps. Please convert tiles to nav tiles in order to enable pathfinding.");
        }

        /// <summary>
        /// Gets all tiles from all tilemaps at a position including all tiles in z direction.
        /// A Tile can be null if it is not set.
        /// </summary>
        /// <param name="inPosition">Position to get the tiles from.</param>
        /// <returns>An array of Tiles including null tiles.</returns>
        public TileBase[] GetAllTilesAtPosition(Vector2Int inPosition)
        {
            List<TileBase> tiles = new List<TileBase>();

            foreach (Tilemap aTilemap in NavTileMaps)
            {
                for (int i = aTilemap.cellBounds.z; i < aTilemap.cellBounds.z + aTilemap.cellBounds.size.z; i++)
                {
                    Debug.Log(new Vector3Int(inPosition.x, inPosition.y, i));
                    tiles.Add(aTilemap.GetTile(new Vector3Int(inPosition.x, inPosition.y, i)));
                }
            }

            return tiles.ToArray();
        }

        /// <summary>
        /// Gets the data associated with the scene in the resources folder next to the scene.
        /// </summary>
        /// <param name="inCreateNewIfNotPresent">Creates a new data asset if it was not found.</param>
        /// <returns>The data asset associated with the scene. Can be null if not found and not created.</returns>
        private NavTileSurfaceData GetData(bool inCreateNewIfNotPresent)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            NavTileSurfaceData data = Resources.Load<NavTileSurfaceData>($"NavData-{sceneName}");

            if (data != null)
            {
                return data;
            }
#if UNITY_EDITOR
            else if (inCreateNewIfNotPresent)
            {
                return CreateNewDataForCurrentScene();
            }
#endif
            else
            {
                return null;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Creates a new NavTile data asset for the current scene.
        /// This asset will be located in the resources folder next to the scene in its corresponding folder.
        /// </summary>
        /// <returns>The newly created data asset.</returns>
        private NavTileSurfaceData CreateNewDataForCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;

            string sceneFolderPath = currentScene.path.Replace($"/{sceneName}.unity", "");
            string resourcesFolderPath = sceneFolderPath + "/Resources";
            string assetPath = resourcesFolderPath + $"/NavData-{sceneName}.asset";

            if (!AssetDatabase.IsValidFolder(resourcesFolderPath))
                AssetDatabase.CreateFolder(sceneFolderPath, "Resources");

            NavTileSurfaceData data = ScriptableObject.CreateInstance<NavTileSurfaceData>();
            AssetDatabase.CreateAsset(data, assetPath);

            Debug.Log("NavTile data asset succesfully created.", data);

            return data;
        }
#endif
    }
}
