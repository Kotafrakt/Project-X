using UnityEngine;
using UnityEditor;
using Snowcap.EditorPackage;

namespace Snowcap.NavTiles
{
    public partial class NavTileWindow : EditorWindow
    {
        /// <summary>
        /// Enum with the different tabs in the window.
        /// </summary>
        public enum SettingsTab
        {
            Areas,
            Agents,
            Pipeline,
            Bake,
            Links
        }

        // Prefix for variables saved to EditorPrefs.
        private const string KEY_PREFIX = nameof(NavTileWindow) + "_";

        // ScriptableObject that holds data for the NavTileWindow.
        public NavTileManager NavTileManagerReference;
        // SerializedObject representation of the NavTileManager.
        private SerializedObject _navTileSerializedObject;

        private SettingsTab _previousTab;
        private SettingsTab _activeTab;
        private Vector2 _scrollPos;

        // Settings tabs and their tooltips.
        private GUIContent[] _tabButtons =
        {
            new GUIContent("Areas", "NavTile area settings."),
            new GUIContent("Agents", "NavTile agent settings."),
            new GUIContent("Pipeline", "NavTile pipeline options."),
            new GUIContent("Bake", "NavTile bake options."),
            new GUIContent("Links", "Overview of NavLinks.")
        };

        private static NavTileWindow _window;
        private static NavTileWindow Window
        {
            get
            {
                if (_window == null) { _window = EditorWindow.GetWindow(typeof(NavTileWindow)) as NavTileWindow; }
                return _window;
            }
        }

        // The NavTiles banner displayed at the top of the inspector.
        private static ResizingBanner _bannerTexture = new ResizingBanner(NavTileGUIDs.NAV_TILE_BANNER);

        /// <summary>
        /// Holds the grid's settings and updates the gridInfo when those settings are updated.
        /// </summary>
        private class GridSettings
        {
            public GridSettings(Grid inGrid)
            {
                Set(inGrid);
            }

            /// <summary>
            /// Updates the grid settings.
            /// </summary>
            public void Set(Grid inGrid)
            {
                this.cellSize = inGrid.cellSize;
                this.cellGap = inGrid.cellGap;
                this.cellLayout = inGrid.cellLayout;
                this.cellSwizzle = inGrid.cellSwizzle;
                this.worldPosition = inGrid.transform.position;
                this.worldScale = inGrid.transform.lossyScale;
                this.worldRotation = inGrid.transform.rotation;

                // Update gridInfo, since grid settings changed.
                NavTileManager.Instance.SurfaceManager.ReInitializeGridInfo();
            }

            // Grid settings.
            public Vector3 cellSize;
            public Vector3 cellGap;
            public GridLayout.CellLayout cellLayout;
            public GridLayout.CellSwizzle cellSwizzle;

            // Transform settings.
            public Vector3 worldPosition;
            public Vector3 worldScale;
            public Quaternion worldRotation;
        }
        private GridSettings _gridSettings;

        /// <summary>
        /// Opens the NavTileSettings window.
        /// </summary>
        [MenuItem("Window/AI/NavTile Settings")]
        private static void ShowWindow()
        {
            Window.titleContent = new GUIContent("NavTile Settings", EditorHelper.LoadTexture(NavTileGUIDs.NAV_TILE_WINDOW_ICON), "Customize settings for NavTiles.");
            Window.minSize = new Vector2(300f, 100f);
            Window.ShowTab();
        }

        /// <summary>
        /// Initialization performed when the SerializedObject is reloaded.
        /// Called on play mode change, window open and Unity startup.
        /// </summary>
        private void OnEnable()
        {
            RefreshSerializedObject();

            InitializePipelineTab();
            InitializeBakeTab();
        }
        
        /// <summary>
        /// Refresh the NavTile2D SerializedObject in case we lost the reference.
        /// </summary>
        private void RefreshSerializedObject()
        {
            if (NavTileManagerReference == null)
            {
                NavTileManagerReference = NavTileManager.Instance;
            }
            if (_navTileSerializedObject == null)
            {
                _navTileSerializedObject = new SerializedObject(NavTileManagerReference);
            }
        }

        /// <summary>
        /// Draw contents of the window.
        /// </summary>
        private void OnGUI()
        {
            // Check if grid settings changed, and reinitialize gridInfo if they did.
            UpdateGridSettings();

            // Draw banner.
            EditorGUILayout.Space();
            _bannerTexture.Draw();

            // Draw tab toolbar.
            EditorGUILayout.Space();
            DoTabToolbar();
            EditorGUILayout.Space();

            _navTileSerializedObject.Update();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            // Show the opened tab.
            switch (_activeTab)
            {
                case SettingsTab.Areas:
                    DoAreasTab();
                    break;
                case SettingsTab.Agents:
                    DoAgentsTab();
                    break;
                case SettingsTab.Pipeline:
                    DoPipelineTab();
                    break;
                case SettingsTab.Bake:
                    DoBakeTab();
                    break;
                case SettingsTab.Links:
                    DoLinksTab();
                    break;
            }

            EditorGUILayout.EndScrollView();

            _navTileSerializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws a toolbar with tabs for each mode.
        /// </summary>
        private void DoTabToolbar()
        {
            EditorHelper.BeginFlexibleHorizontal();
            _activeTab = (SettingsTab)GUILayout.Toolbar((int)_activeTab, _tabButtons, "LargeButton", GUI.ToolbarButtonSize.FitToContents);

            if (_activeTab != _previousTab)
            {
                OnTabChanged(_activeTab);
                _previousTab = _activeTab;
            }
            EditorHelper.EndFlexibleHorizontal();
        }

        /// <summary>
        /// Called when switching to a different tab.
        /// </summary>
        private void OnTabChanged(SettingsTab inTab)
        {
            switch (inTab)
            {
                case SettingsTab.Pipeline:
                    OnLoadPipelineTab();
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            // Repaint the window as long as it is visible.
            // This is done to instantly refresh the window when an undo/redo is performed while the window is not in focus.
            Window.Repaint();
        }

        private void OnBecameVisible()
        {
            // Subscribe to duringSceneGui in order to display handles while settings window is visible.
            SceneView.duringSceneGui += OnSceneGUI;
            SceneView.RepaintAll();
        }

        private void OnBecameInvisible()
        {
            // Unsubscribe from duringSceneGui when settings window is not visible.
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.RepaintAll();
        }

        private void OnSceneGUI(SceneView inSceneview)
        {
            // Draw debug handles.
            NavTileDebugVisualizer.DrawSceneViewHandles(NavTileManager.Instance.SurfaceManager);
            SceneView.RepaintAll();
        }

        /// <summary>
        /// Determines whether grid settings changed, and updates gridInfo if they did.
        /// </summary>
        private void UpdateGridSettings()
        {
            Grid grid = NavTileManager.Instance.SurfaceManager.Grid;
            if (grid == null)
                return;

            if (_gridSettings == null)
            {
                _gridSettings = new GridSettings(grid);
            }

            if (grid.cellSize != _gridSettings.cellSize ||
                grid.cellGap != _gridSettings.cellGap ||
                grid.cellLayout != _gridSettings.cellLayout ||
                grid.cellSwizzle != _gridSettings.cellSwizzle ||
                grid.transform.position != _gridSettings.worldPosition ||
                grid.transform.lossyScale != _gridSettings.worldScale ||
                grid.transform.rotation != _gridSettings.worldRotation)
            {
                _gridSettings.Set(grid);
            }
        }
    }
}
