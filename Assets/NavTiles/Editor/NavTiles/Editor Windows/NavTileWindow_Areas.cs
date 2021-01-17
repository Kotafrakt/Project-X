using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Snowcap.EditorPackage;

namespace Snowcap.NavTiles
{
    public partial class NavTileWindow
    {
        private SerializedProperty _areas;
        private ReorderableList _areasList = null;
        private bool _duplicateAreas = false;

        /// <summary>
        ///  Static function to open the window and show the areas settings from any location.
        /// </summary>
        public static void OpenAreasTab()
        {
            ShowWindow();
            if (Window == null) { return; }
            Window._activeTab = SettingsTab.Areas;
        }

        /// <summary>
        /// Controls and displays all Area settings.
        /// </summary>
        private void DoAreasTab()
        {
            if (_areas == null || _areasList == null)
                InitAreas();

            _duplicateAreas = false;
            _areasList.index = Mathf.Max(0, _areasList.index);
            _areasList.DoLayoutList();

            if (_duplicateAreas)
            {
                EditorGUILayout.HelpBox("Duplicate area names are not supported. Please remove duplicate entries.", MessageType.Error);
            }
        }

        /// <summary>
        /// Initializes the values needed to draw the reorderable list for the areas.
        /// </summary>
        private void InitAreas()
        {
            if (_areas == null)
            {
                SerializedProperty areaManager = _navTileSerializedObject.FindProperty("_areaManager");
                _areas = areaManager.FindPropertyRelative("_areas");
            }

            if (_areasList == null)
            {
                _areasList = new ReorderableList(NavTileManagerReference.AreaManager.AllAreas, typeof(NavTileArea), false, false, false, false)
                {
                    drawHeaderCallback = DrawAreasListHeader,
                    drawElementCallback = DrawAreasListElement,
                    elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    footerHeight = 0
                };
            }
        }

        /// <summary>
        /// Draws the header of the reorderable list for the areas.
        /// </summary>
        private void DrawAreasListHeader(Rect inRect)
        {
            Rect idRect, colorRect, nameRect, costRect, priorityRect;
            GetAreasListRects(inRect, out idRect, out colorRect, out nameRect, out costRect, out priorityRect);
            EditorGUI.LabelField(idRect, new GUIContent("#", "The area's unique ID."));
            EditorGUI.LabelField(nameRect, "Name");
            EditorGUI.LabelField(costRect, new GUIContent("Cost", "The pathfinding cost for the area type."));
            EditorGUI.LabelField(priorityRect, new GUIContent("Priority", "Determines which NavTileArea is used for overlapping tiles. The highest priority tile determines the NavTileArea for that spot on the grid."));
        }

        /// <summary>
        /// Draws an element of the reorderable list for the areas.
        /// </summary>
        private void DrawAreasListElement(Rect inRect, int inIndex, bool inSelected, bool inFocused)
        {
            SerializedProperty areaProperty = _areas.GetArrayElementAtIndex(inIndex);
            if (areaProperty == null) { return; }
            SerializedProperty areaColorProperty = areaProperty.FindPropertyRelative(nameof(NavTileArea.Color));
            SerializedProperty areaNameProperty = areaProperty.FindPropertyRelative(nameof(NavTileArea.Name));
            SerializedProperty areaCostProperty = areaProperty.FindPropertyRelative(nameof(NavTileArea.Cost));
            SerializedProperty areaPriorityProperty = areaProperty.FindPropertyRelative(nameof(NavTileArea.Priority));

            inRect.height -= 2;
            inRect.y += 1;
            Rect idRect, colorRect, nameRect, costRect, priorityRect;
            GetAreasListRects(inRect, out idRect, out colorRect, out nameRect, out costRect, out priorityRect);
            
            // Display area info.
            EditorGUI.LabelField(idRect, inIndex.ToString());

            bool readOnly;
            switch (inIndex)
            {
                case 0:
                case 1:
                    readOnly = true;
                    break;
                default:
                    readOnly = false;
                    break;
            }

            areaColorProperty.colorValue = EditorGUI.ColorField(colorRect, GUIContent.none, areaColorProperty.colorValue, false, false, false);

            // Highlight duplicate area names.
            bool duplicateEntry = NavTileManager.Instance.AreaManager.IsDuplicateEntry(areaNameProperty.stringValue);
            Texture2D prevTex = EditorStyles.textField.normal.background;
            if (duplicateEntry)
            {
                _duplicateAreas = true;
                EditorStyles.textField.normal.background = null;
                EditorStyles.textField.active.background = null;
                EditorStyles.textField.focused.background = null;
                Color backgroundColor = new Color(1f, 0f, 0f, inIndex == 0 ? .25f : .45f);
                Color outlineColor = new Color(backgroundColor.r / 2f, 0f, 0f, 1f);
                EditorHelper.DrawRectWithOutline(nameRect, backgroundColor, outlineColor, 1);
            }
            EditorHelper.DrawProperty(areaNameProperty, new PropertyDrawingOptions().DoRect(nameRect).DoHideLabel(true).DoReadOnly(readOnly));
            EditorStyles.textField.normal.background = prevTex;
            EditorStyles.textField.active.background = prevTex;
            EditorStyles.textField.focused.background = prevTex;

            areaCostProperty.intValue = Mathf.Max(0, EditorGUI.IntField(costRect, areaCostProperty.intValue));
            EditorHelper.DrawProperty(areaPriorityProperty, new PropertyDrawingOptions().DoRect(priorityRect).DoHideLabel(true));
        }

        /// <summary>
        /// Initializes the rects required to draw the elements of the reorderable list for the areas.
        /// </summary>
        private void GetAreasListRects(Rect inRect, out Rect outIDRect, out Rect outColorRect, out Rect outNameRect, out Rect outCostRect, out Rect outPriorityRect)
        {
            float idWidth = EditorGUIUtility.singleLineHeight * 1.5f;
            float colorWidth = EditorGUIUtility.singleLineHeight * 2f;
            float costWidth = EditorGUIUtility.singleLineHeight * 4;
            float priorityWidth = EditorGUIUtility.singleLineHeight * 4;
            float nameWidth = inRect.width - idWidth - colorWidth - costWidth - priorityWidth;

            float x = inRect.x;

            outIDRect = new Rect(x, inRect.y, idWidth - 4, inRect.height);
            x += idWidth;
            outColorRect = new Rect(x, inRect.y, colorWidth - 4, inRect.height);
            x += colorWidth;
            outNameRect = new Rect(x, inRect.y, nameWidth - 4, inRect.height);
            x += nameWidth;
            outCostRect = new Rect(x, inRect.y, costWidth - 4, inRect.height);
            x += costWidth;
            outPriorityRect = new Rect(x, inRect.y, priorityWidth, inRect.height);
        }
    }
}
