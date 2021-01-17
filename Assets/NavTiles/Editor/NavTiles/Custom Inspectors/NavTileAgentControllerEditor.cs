using UnityEditor;
using Snowcap.EditorPackage;
using UnityEngine;
using UnityEditorInternal;
using static Snowcap.NavTiles.NavTileAgentController;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Editor class for the NavTileAgentController.
    /// </summary>
    [CustomEditor(typeof(NavTileAgentController))]
    public class NavTileAgentControllerEditor : Editor
    {
        // The NavTiles banner displayed at the top of the inspector window.
        private static ResizingBanner _bannerTexture = new ResizingBanner(NavTileGUIDs.NAV_TILE_BANNER);

        // Tooltips.
        private const string INDEX_TOOLTIP = "The order in which the agent will traverse the waypoints.";
        private const string DELAY_TOOLTIP = "Time in seconds to wait before moving to the waypoint. If set to zero, the agent will immediately start moving after reaching the previous waypoint.";
        
        private const string DESTINATION_TYPE_TOOLTIP = "The type of target to move towards.\n\n" +
                                                        "Transform: " + TRANSFORM_TOOLTIP + "\n\n" +
                                                        "World Position: " + WORLD_POSITION_TOOLTIP + "\n\n" +
                                                        "Grid Coordinate: " + GRID_COORDINATE_TOOLTIP;
        private const string TRANSFORM_TOOLTIP = "Target transform to move towards. This is not a follow target. Think of it like a stationary waypoint.";
        private const string WORLD_POSITION_TOOLTIP = "World position to move towards.";
        private const string GRID_COORDINATE_TOOLTIP = "Grid coordinate to move towards.";
        
        private const string DESTINATION_TOOLTIP = "The waypoint's target position.";

        // Fields.
        private SerializedProperty _loopProperty;

        // Waypoints.
        private SerializedProperty _waypointsListProperty;
        private ReorderableList _waypointsReorderableList;

        private void OnEnable()
        {
            // Fields.
            _loopProperty = serializedObject.FindProperty("_loop");
            _waypointsListProperty = serializedObject.FindProperty("_waypoints");

            _waypointsReorderableList = new ReorderableList(serializedObject, _waypointsListProperty, true, false, true, true)
            {
                drawHeaderCallback = DrawWaypointsListHeader,
                drawElementCallback = DrawWaypointsListElement,
                elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                headerHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
            };
        }

        /// <summary>
        /// Draw the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _bannerTexture.Draw();

            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorHelper.DrawProperty(_loopProperty, new PropertyDrawingOptions().DoTooltip("Whether to loop the waypoints' list when the final waypoint is reached."));
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Waypoints", EditorStyles.boldLabel);
            DrawWaypointsList();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawWaypointsList()
        {
            _waypointsReorderableList.index = Mathf.Max(0, _waypointsReorderableList.index);
            _waypointsReorderableList.DoLayoutList();
        }

        /// <summary>
        /// Draw the waypoints reorderable list header.
        /// </summary>
        private void DrawWaypointsListHeader(Rect inRect)
        {
            Rect indexRect, delayRect, destinationTypeRect, destinationRect;
            GetWaypointsListRects(inRect, out indexRect, out delayRect, out destinationTypeRect, out destinationRect, true);

            // Shared labels.
            EditorGUI.LabelField(indexRect, new GUIContent("#", INDEX_TOOLTIP));
            EditorGUI.LabelField(delayRect, new GUIContent("Delay", DELAY_TOOLTIP));
            EditorGUI.LabelField(destinationTypeRect, new GUIContent("Target Type", DESTINATION_TYPE_TOOLTIP));
            EditorGUI.LabelField(destinationRect, new GUIContent("Target", DESTINATION_TOOLTIP));
        }

        /// <summary>
        /// Draw the waypoints reorderable list elements.
        /// </summary>
        private void DrawWaypointsListElement(Rect inRect, int inIndex, bool inSelected, bool inFocused)
        {
            SerializedProperty waypointProperty = _waypointsListProperty.GetArrayElementAtIndex(inIndex);
            if (waypointProperty == null) { return; }

            // Find relative properties.
            SerializedProperty delayProperty = waypointProperty.FindPropertyRelative(nameof(PathWaypoint.Delay));
            SerializedProperty destinationTypeProperty = waypointProperty.FindPropertyRelative(nameof(PathWaypoint.DestinationType));

            // Setup rects.
            inRect.height -= 2;
            Rect indexRect, delayRect, destinationTypeRect, destinationRect;
            GetWaypointsListRects(inRect, out indexRect, out delayRect, out destinationTypeRect, out destinationRect);

            // Draw waypoint info.
            int order = inIndex + 1;
            EditorGUI.LabelField(indexRect, order.ToString());
            delayProperty.floatValue = EditorGUI.FloatField(delayRect, delayProperty.floatValue);
            EditorHelper.DrawProperty(destinationTypeProperty, new PropertyDrawingOptions().DoRect(destinationTypeRect).DoHideLabel(true));

            DrawWaypointSetting(waypointProperty, destinationRect, (WaypointDestinationType)destinationTypeProperty.enumValueIndex);
        }

        /// <summary>
        /// Draw a variable setting field depending on the passed in category.
        /// </summary>
        private void DrawWaypointSetting(SerializedProperty inWaypointProperty, Rect inRect, WaypointDestinationType inDestinationType)
        {
            switch (inDestinationType)
            {
                case WaypointDestinationType.Transform:
                    SerializedProperty transformProperty = inWaypointProperty.FindPropertyRelative(nameof(PathWaypoint.TargetTransform));
                    EditorHelper.DrawProperty(transformProperty, new PropertyDrawingOptions().DoRect(inRect).DoHideLabel(true));
                    break;
                case WaypointDestinationType.WorldPosition:
                    SerializedProperty positionProperty = inWaypointProperty.FindPropertyRelative(nameof(PathWaypoint.TargetPosition));
                    EditorHelper.DrawProperty(positionProperty, new PropertyDrawingOptions().DoRect(inRect).DoHideLabel(true));
                    break;
                case WaypointDestinationType.GridCoordinate:
                    SerializedProperty coordinateProperty = inWaypointProperty.FindPropertyRelative(nameof(PathWaypoint.TargetCoordinate));
                    EditorHelper.DrawProperty(coordinateProperty, new PropertyDrawingOptions().DoRect(inRect).DoHideLabel(true));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Initialize the rects required to display the reorderable list of waypoints.
        /// </summary>
        private void GetWaypointsListRects(Rect inRect, out Rect outIndexRect, out Rect outDelayRect, out Rect outDestinationTypeRect, out Rect outDestinationRect, bool header = false)
        {
            int padding = 10;
            int indent = header ? 14 : 0;

            // Rect sizes.
            float indexWidth = inRect.height * 1.25f;
            float delayWidth = inRect.height * 2.25f;
            float destinationTypeWidth = inRect.height * 6f;

            // Flexible destination rect.
            float destinationWidth = inRect.width - indexWidth - delayWidth - destinationTypeWidth;
            destinationWidth -= padding * 3;
            destinationWidth -= indent;

            float x = inRect.x;

            outIndexRect = new Rect(x, inRect.y, indexWidth, inRect.height);
            x += indent + padding + indexWidth;
            outDelayRect = new Rect(x, inRect.y, delayWidth, inRect.height);
            x += padding + delayWidth;
            outDestinationTypeRect = new Rect(x, inRect.y, destinationTypeWidth, inRect.height);
            x += padding + destinationTypeWidth;
            outDestinationRect = new Rect(x, inRect.y, destinationWidth, inRect.height);
        }
    }   
}
