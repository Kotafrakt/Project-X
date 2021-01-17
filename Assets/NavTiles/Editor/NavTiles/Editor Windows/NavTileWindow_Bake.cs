using UnityEngine;
using UnityEditor;
using Snowcap.EditorPackage;
using static Snowcap.NavTiles.NavTileDebugVisualizer;

namespace Snowcap.NavTiles
{
    public partial class NavTileWindow
    {
        private GUIStyle _bakeLabelStyle = null;
        private GUIStyle BakeLabelStyle
        {
            get
            {
                if (_bakeLabelStyle == null)
                {
                    _bakeLabelStyle = new GUIStyle(EditorStyles.miniBoldLabel);
                    _bakeLabelStyle.alignment = TextAnchor.MiddleRight;
                }
                return _bakeLabelStyle;
            }
        }

        private void InitializeBakeTab()
        {
            LoadDebugVisualizerEditorPrefs();
        }

        /// <summary>
        /// Controls and displays all Bake settings.
        /// </summary>
        private void DoBakeTab()
        {
            EditorGUI.BeginChangeCheck();

            // Determine debug mode.
            DisplayingOptions = (DisplayOptions)EditorGUILayout.EnumPopup("Debug Mode", DisplayingOptions);

            // Display relevant debug options for each mode.
            switch (DisplayingOptions)
            {
                case DisplayOptions.Areas:
                    DoAreasOptions();
                    break;
                case DisplayOptions.None:
                default:
                    break;
            }

            // Save debug options if they were changed.
            if (EditorGUI.EndChangeCheck())
            {
                SaveDebugVisualizerEditorPrefs();
                SceneView.RepaintAll();
            }

            // Display "Clear" and "Bake" buttons.
            EditorHelper.BeginFlexibleHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Width(EditorHelper.MEDIUM_BUTTON_WIDTH)))
            {
                NavTileManagerReference.SurfaceManager.Data.ClearTiles();
                NavTileManagerReference.SurfaceManager.Data.CurrentBakeState = NavTileSurfaceData.BakeState.Unbaked;
            }
            if (GUILayout.Button("Bake", GUILayout.Width(EditorHelper.MEDIUM_BUTTON_WIDTH)))
            {
                NavTileManagerReference.SurfaceManager.Bake(NavTileManagerReference.PipelineManager.Algorithm);
            }
            EditorGUILayout.EndHorizontal();

            // Determine current bake state.
            NavTileSurfaceData.BakeState currentBakeState;
            if (NavTileManagerReference.SurfaceManager.IsDataInitialized)
                currentBakeState = NavTileManagerReference.SurfaceManager.Data.CurrentBakeState;
            else
                currentBakeState = NavTileSurfaceData.BakeState.Unbaked;

            // Display current bake state.
            EditorGUILayout.LabelField("Bake State: " + GetBakeStateDisplayText(currentBakeState), BakeLabelStyle);

            // Display rebake warning if the grid has to be rebaked.
            if (NavTileManagerReference.PipelineManager.Algorithm.GetRequiredBakeState() != currentBakeState)
            {
                EditorGUILayout.HelpBox("A grid rebake is necessary.", MessageType.Warning);
            }
        }

        /// <summary>
        /// Display debug options for Areas.
        /// </summary>
        private void DoAreasOptions()
        {
            AreaGizmoSize = EditorGUILayout.Slider("Tile Gizmo Size", AreaGizmoSize, 0f, 1f);
            AreaGizmoAlpha = EditorGUILayout.Slider("Tile Gizmo Alpha", AreaGizmoAlpha, 0f, 1f);
        }

        /// <summary>
        /// Returns a readable string representation of the passed in enum value.
        /// </summary>
        private string GetBakeStateDisplayText(NavTileSurfaceData.BakeState inBakeState)
        {
            switch (inBakeState)
            {
                case NavTileSurfaceData.BakeState.Unbaked:
                    return "Unbaked";
                case NavTileSurfaceData.BakeState.Standard:
                    return "Baked";
                case NavTileSurfaceData.BakeState.AdditionalData:
                    return "Baked for JPS+";
                default:
                    return "Undefined";
            }
        }
    }
}
