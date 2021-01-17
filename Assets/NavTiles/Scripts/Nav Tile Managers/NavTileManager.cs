using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Asset that stores general data of NavTiles and holds references to other parts of NavTiles.
    /// </summary>
    public partial class NavTileManager : ScriptableObject
    {
        #region Singleton
        private static NavTileManager _instance = null;
        /// <summary>
        /// Singleton instance used to access all information of NavTiles.
        /// Will automatically be created if not found in the Resources folder.
        /// </summary>
        public static NavTileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (NavTileManager)Resources.Load("NavTiles/NavTileManager");
                }
#if UNITY_EDITOR
                if (_instance == null)
                {
                    //CheckCompatibility();

                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    if (!AssetDatabase.IsValidFolder("Assets/Resources/NavTiles"))
                    {
                        AssetDatabase.CreateFolder("Assets/Resources", "NavTiles");
                    }

                    _instance = CreateInstance<NavTileManager>();
                    _instance.Initialize();
                    AssetDatabase.CreateAsset(_instance, "Assets/Resources/NavTiles/NavTileManager.asset");
                }
#endif
                return _instance;
            }
        }

        #endregion

        [SerializeField]
        [HideInInspector]
        private NavTileSurfaceManager _surfaceManager = new NavTileSurfaceManager();
        /// <summary>
        /// Manager of all scene related data like Grid and NavTile data.
        /// </summary>
        public NavTileSurfaceManager SurfaceManager
        {
            get
            {
                return _surfaceManager;
            }
        }

        [SerializeField]
        [HideInInspector]
        private NavLinkManager _linkManager = new NavLinkManager();
        /// <summary>
        /// Manager of all NavLinks.
        /// </summary>
        public NavLinkManager LinkManager
        {
            get
            {
                return _linkManager;
            }
        }

        [SerializeField]
        [HideInInspector]
        private NavTileAreaManager _areaManager = new NavTileAreaManager();
        /// <summary>
        /// Manager of all NavTile areas.
        /// </summary>
        public NavTileAreaManager AreaManager
        {
            get
            {
                return _areaManager;
            }
        }

        [SerializeField]
        [HideInInspector]
        private NavTilePipelineManager _pipelineManager = new NavTilePipelineManager();
        /// <summary>
        /// Manager of pipeline operations.
        /// </summary>
        public NavTilePipelineManager PipelineManager
        {
            get
            {
                return _pipelineManager;
            }
        }

        [SerializeField]
        [HideInInspector]
        private NavTileAgentManager _agentManager = new NavTileAgentManager();
        /// <summary>
        /// Manager of all agent information.
        /// Used for conflict handling.
        /// </summary>
        public NavTileAgentManager AgentManager
        {
            get
            {
                return _agentManager;
            }
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;

            SurfaceManager.OnEnable();
        }
        
        private void OnDisable()
        {
            SurfaceManager.OnDisable();
        }

        /// <summary>
        /// First time initialization.
        /// </summary>
        private void Initialize()
        {
            _areaManager.InitializeDefaultNavTileAreas();
            _agentManager.InitializeDefaultAgents();
        }

        /// <summary>
        /// Calculates a path through the pipeline using the specified input.
        /// It's an async operation since it can be done multithreaded if specified in the Pipeline tab.
        /// </summary>
        /// <param name="inInput">Data holder for all path find variables.</param>
        /// <returns>A task with a NavTilePath as output when the calculation is finished.</returns>
        public async Task<NavTilePath> GetPath(FindPathInput inInput)
        {
            return await PipelineManager.GetPath(inInput);
        }

        /// <summary>
        /// Checks the API compatibility of the project and prompts to change it if not set correctly.
        /// </summary>
        /// <returns>Whether or not the compatibility is correctly set afterwards.</returns>
        private static bool CheckCompatibility()
        {
#if UNITY_EDITOR
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            if (PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup) == ApiCompatibilityLevel.NET_4_6)
            {
                return true;
            }

            bool allowedToChangeSettings = EditorUtility.DisplayDialog("NavTiles Setup", "NavTiles requires the latest .NET API compatility level " +
                                           "which is not currently set. Do you want to change this now? If ignored, this will most likely cause errors.", "Yes", "No");

            if (allowedToChangeSettings)
            {
                PlayerSettings.SetApiCompatibilityLevel(buildTargetGroup, ApiCompatibilityLevel.NET_4_6);
                return true;
            }

            return false;
#else
            return true;
#endif
        }
    }
}
