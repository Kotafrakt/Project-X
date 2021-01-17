using UnityEditor;
using Snowcap.EditorPackage;
using UnityEngine;
using UnityEditorInternal;
using System.Linq;
using System;
using AnimatorController = UnityEditor.Animations.AnimatorController;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Editor class for the NavTileAgent.
    /// </summary>
    [CustomEditor(typeof(NavTileAgent))]
    [CanEditMultipleObjects]
    public class NavTileAgentEditor : Editor
    {
        private enum AreaSettingsCategory { Speed, AnimationTriggerName }

        // The NavTiles banner displayed at the top of the inspector window.
        private static ResizingBanner _bannerTexture = new ResizingBanner(NavTileGUIDs.NAV_TILE_BANNER);

        // Inspector interaction variables.
        private static bool _areaSettingsFoldoutOpen = false;
        private static bool _animationSettingsFoldoutOpen = false;
        private static bool _pathCallbacksFoldoutOpen = false;

        // EditorPrefs keys for inspector interaction variables.
        private const string KEY_PREFIX = nameof(NavTileAgentEditor) + "_";

        private const string AREA_SETTINGS_FOLDOUT_OPEN_KEY = KEY_PREFIX + nameof(_areaSettingsFoldoutOpen);
        private const string ANIMATION_SETTINGS_FOLDOUT_OPEN_KEY = KEY_PREFIX + nameof(_animationSettingsFoldoutOpen);
        private const string PATH_CALLBACKS_FOLDOUT_OPEN_KEY = KEY_PREFIX + nameof(_pathCallbacksFoldoutOpen);

        // Tooltips.
        private const string AUTO_TRAVERSE_PATH_TOOLTIP = "When enabled, the agent starts walking as soon as a path is found. When disabled, only the OnPathFound event is fired.";
        private const string SPEED_ANIM_PARAM_TOOLTIP = "The animator's float parameter to pass the agent's speed into.";
        private const string HORIZONTAL_ANIM_PARAM_TOOLTIP = "The animator's float parameter to pass the agent's normalized horizontal movement direction into.";
        private const string VERTICAL_ANIM_PARAM_TOOLTIP = "The animator's float parameter to pass the agent's normalized vertical movement direction into.";

        // Fields.
        private SerializedProperty _autoTraversePathProperty;
        private SerializedProperty _areaMaskProperty;
        private SerializedProperty _agentTypeProperty;
        private SerializedProperty _speedProperty;
        private SerializedProperty _diagonalAllowedProperty;
        private SerializedProperty _cutCornersProperty;
        private SerializedProperty _ignoreTileCostProperty;
        private SerializedProperty _conflictOptionProperty;
        private SerializedProperty _waitAfterFreeTileProperty;
        private SerializedProperty _debugEnabledProperty;
        private SerializedProperty _debugLineColorProperty;

        // Animation settings.
        private SerializedProperty _animatorProperty;
        private SerializedProperty _preserveAnimDirectionProperty;
        private SerializedProperty _horizontalAnimParamProperty;
        private SerializedProperty _verticalAnimParamProperty;
        private SerializedProperty _speedAnimParamProperty;
        private ReorderableList _areaAnimationsReorderableList;

        // Area settings.
        private SerializedProperty _onAreaChangeUnityEvent;
        private SerializedProperty _areaSettingsListProperty;
        private ReorderableList _areaSpeedsReorderableList;

        // Path callbacks.
        private SerializedProperty _onPathFoundUnityEventProperty;
        private SerializedProperty _onPathNotFoundUnityEventProperty;
        private SerializedProperty _onPathAbortedUnityEventProperty;
        private SerializedProperty _onPathFinishedUnityEventProperty;

        /// <summary>
        /// The script being inspected.
        /// </summary>
        private NavTileAgent Target { get { return (NavTileAgent) target; } }

        private GUIContent[] _animationTriggers;
        private GUIContent[] _animationFloatParameters;

        /// <summary>
        /// Contains the GUIContent info for the area settings dropdown options.
        /// </summary>
        private static readonly GUIContent[] AREA_SETTINGS_DROPDOWN_OPTIONS = new GUIContent[]
        {
            new GUIContent
            (
                "Speed",
                "The character's speed when traversing an area. By default this is the same as the character's speed. Click the checkbox to override."
            ),
            new GUIContent
            (
                "Animation Trigger Name",
                "The animation trigger to call when entering the area."
            )
        };

        private void OnEnable()
        {
            // Fields.
            _autoTraversePathProperty = serializedObject.FindProperty("AutoTraversePath");
            _areaMaskProperty = serializedObject.FindProperty("_areaMask");
            _agentTypeProperty = serializedObject.FindProperty("_agentType");
            _speedProperty = serializedObject.FindProperty("_speed");
            _diagonalAllowedProperty = serializedObject.FindProperty("_diagonalAllowed");
            _cutCornersProperty = serializedObject.FindProperty("_cutCorners");
            _ignoreTileCostProperty = serializedObject.FindProperty("_ignoreTileCost");
            _conflictOptionProperty = serializedObject.FindProperty("_conflictOption");
            _waitAfterFreeTileProperty = serializedObject.FindProperty("_waitAfterFreeTile");
            _debugEnabledProperty = serializedObject.FindProperty("_debugEnabled");
            _debugLineColorProperty = serializedObject.FindProperty("_debugLineColor");

            // Animation settings.
            _animatorProperty = serializedObject.FindProperty("LinkedAnimator");
            _preserveAnimDirectionProperty = serializedObject.FindProperty("_preserveAnimDirection");
            _horizontalAnimParamProperty = serializedObject.FindProperty("_animationHorizontalParameter");
            _verticalAnimParamProperty = serializedObject.FindProperty("_animationVerticalParameter");
            _speedAnimParamProperty = serializedObject.FindProperty("_animationSpeedParameter");

            // Area settings.
            _onAreaChangeUnityEvent = serializedObject.FindProperty("OnAreaChangeUnityEvent");
            _areaSettingsListProperty = serializedObject.FindProperty("_areaSettingsList");

            // Path callbacks.
            _onPathFoundUnityEventProperty = serializedObject.FindProperty("OnPathFoundUnityEvent");
            _onPathNotFoundUnityEventProperty = serializedObject.FindProperty("OnPathNotFoundUnityEvent");
            _onPathAbortedUnityEventProperty = serializedObject.FindProperty("OnPathAbortedUnityEvent");
            _onPathFinishedUnityEventProperty = serializedObject.FindProperty("OnPathFinishedUnityEvent");

            _areaAnimationsReorderableList = new ReorderableList(serializedObject, _areaSettingsListProperty, false, false, false, false)
            {
                drawHeaderCallback = DrawAreaAnimationsListHeader,
                drawElementCallback = DrawAreaAnimationsListElement,
                elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                footerHeight = 0
            };

            _areaSpeedsReorderableList = new ReorderableList(serializedObject, _areaSettingsListProperty, false, false, false, false)
            {
                drawHeaderCallback = DrawAreaSpeedsListHeader,
                drawElementCallback = DrawAreaSpeedsListElement,
                elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                footerHeight = 0
            };

            // Default animator to self if possible.
            for (int i = 0; i < targets.Length; i++)
            {
                NavTileAgent anAgent = (NavTileAgent)targets[i];

                if (anAgent.LinkedAnimator == null)
                    anAgent.LinkedAnimator = anAgent.GetComponent<Animator>();
            }

            UpdateAreaSettingsList();
            LoadEditorPrefs();
        }

        private void OnDestroy()
        {
            SaveEditorPrefs();
        }

        /// <summary>
        /// Load cached EditorPrefs.
        /// </summary>
        private void LoadEditorPrefs()
        {
            if (EditorPrefs.HasKey(AREA_SETTINGS_FOLDOUT_OPEN_KEY))
                _areaSettingsFoldoutOpen = EditorPrefs.GetBool(AREA_SETTINGS_FOLDOUT_OPEN_KEY);
            if (EditorPrefs.HasKey(ANIMATION_SETTINGS_FOLDOUT_OPEN_KEY))
                _animationSettingsFoldoutOpen = EditorPrefs.GetBool(ANIMATION_SETTINGS_FOLDOUT_OPEN_KEY);
            if (EditorPrefs.HasKey(PATH_CALLBACKS_FOLDOUT_OPEN_KEY))
                _pathCallbacksFoldoutOpen = EditorPrefs.GetBool(PATH_CALLBACKS_FOLDOUT_OPEN_KEY);
        }

        /// <summary>
        /// Save EditorPrefs so they can be reloaded on recompile and playmode switching.
        /// </summary>
        private void SaveEditorPrefs()
        {
            EditorPrefs.SetBool(AREA_SETTINGS_FOLDOUT_OPEN_KEY, _areaSettingsFoldoutOpen);
            EditorPrefs.SetBool(ANIMATION_SETTINGS_FOLDOUT_OPEN_KEY, _animationSettingsFoldoutOpen);
            EditorPrefs.SetBool(PATH_CALLBACKS_FOLDOUT_OPEN_KEY, _pathCallbacksFoldoutOpen);
        }

        /// <summary>
        /// Draw the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _bannerTexture.Draw();

            EditorGUILayout.LabelField("Behavior", EditorStyles.boldLabel);
            EditorHelper.DrawProperty(_autoTraversePathProperty, new PropertyDrawingOptions().DoTooltip(AUTO_TRAVERSE_PATH_TOOLTIP));

            EditorGUILayout.Space();

            // Fields.
            EditorGUILayout.LabelField("Pathfinding", EditorStyles.boldLabel);
            DrawAreaMask();

            EditorGUI.BeginChangeCheck();
            _agentTypeProperty.intValue = EditorGUILayout.Popup("Agent Type", _agentTypeProperty.intValue, NavTileManager.Instance.AgentManager.NamedAgents.ToArray());

            EditorHelper.DrawProperty(_speedProperty);

            if (EditorGUI.EndChangeCheck())
                _speedProperty.floatValue = Mathf.Max(0, _speedProperty.floatValue);

            EditorHelper.DrawProperty(_diagonalAllowedProperty);
            if (!_diagonalAllowedProperty.boolValue) { _cutCornersProperty.boolValue = false; }
            EditorHelper.DrawProperty(_cutCornersProperty, new PropertyDrawingOptions().DoReadOnly(!_diagonalAllowedProperty.boolValue));
            EditorHelper.DrawProperty(_ignoreTileCostProperty);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Conflict Handling", EditorStyles.boldLabel);
            if (NavTileManager.Instance.PipelineManager.AlgorithmType == typeof(AStar).AssemblyQualifiedName)
            {
                _conflictOptionProperty.enumValueIndex = EditorGUILayout.Popup("Conflict Option", _conflictOptionProperty.enumValueIndex, _conflictOptionProperty.enumDisplayNames);

                if ((NavTileAgent.EConflictOptions)_conflictOptionProperty.enumValueIndex >= NavTileAgent.EConflictOptions.WaitOnTraversingAgent)
                {
                    EditorHelper.DrawProperty(_waitAfterFreeTileProperty);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("The selected pathfinding algorithm does not support conflict handling. To utilize conflict handling, select A* in the pipeline options.", MessageType.Warning);
                if (GUILayout.Button("Open Pipeline Options"))
                {
                    NavTileWindow.OpenPipelineTab();
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorHelper.DrawProperty(_debugEnabledProperty, new PropertyDrawingOptions().DoLabel("Display Calculated Path").DoTooltip("Whether the calculated path should be displayed using gizmos."));
            if (_debugEnabledProperty.boolValue)
            {
                EditorHelper.DrawProperty(_debugLineColorProperty, new PropertyDrawingOptions().DoLabel("Path Color"));
            }

            EditorGUILayout.Space();

            // Animation settings.
            if (_animationSettingsFoldoutOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_animationSettingsFoldoutOpen, "Animation Settings"))
            {
                EditorGUI.indentLevel++;
                DrawAnimationSettings();
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            // Area settings.
            if (_areaSettingsFoldoutOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_areaSettingsFoldoutOpen, "Area Settings"))
            {
                EditorGUI.indentLevel++;
                DrawAreaSettings();
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            // Path callbacks.
            if (_pathCallbacksFoldoutOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_pathCallbacksFoldoutOpen, "Path Callbacks"))
            {
                EditorGUI.indentLevel++;
                DrawPathCallbacks();
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            SaveEditorPrefs();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Updates the area settings list to match the area mask.
        /// </summary>
        private void UpdateAreaSettingsList()
        {
            for (int maskIndex = 31; maskIndex >= 0; maskIndex--)
            {
                bool isInMask = ((1 << maskIndex) & _areaMaskProperty.intValue) != 0;
                int listIndex = Target.AreaSettingsList.FindIndex((item) => item.AreaIndex == maskIndex);
                bool isInList = listIndex != -1;
                if (isInMask && !isInList)
                {
                    _areaSettingsListProperty.arraySize++;
                    SerializedProperty anElement = _areaSettingsListProperty.GetArrayElementAtIndex(_areaSettingsListProperty.arraySize - 1);
                    SerializedProperty areaIndexProperty = anElement.FindPropertyRelative("AreaIndex");
                    SerializedProperty speedProperty = anElement.FindPropertyRelative("Speed");
                    SerializedProperty animationTriggerProperty = anElement.FindPropertyRelative("AnimationTriggerName");

                    areaIndexProperty.intValue = maskIndex;
                    speedProperty.floatValue = -1f;
                    animationTriggerProperty.stringValue = string.Empty;
                }
                else if (!isInMask && isInList)
                {
                    _areaSettingsListProperty.DeleteArrayElementAtIndex(listIndex);
                }
            }

            // Sort areas by index.
            serializedObject.ApplyModifiedProperties();
            for (int i = 0; i < targets.Length; i++)
            {
                ((NavTileAgent)targets[i]).AreaSettingsList.Sort((j, k) => j.AreaIndex.CompareTo(k.AreaIndex));
            }
            serializedObject.Update();
        }

        /// <summary>
        /// Draws the area mask and updates the area settings list if the mask is changed.
        /// </summary>
        private void DrawAreaMask()
        {
            if (EditorHelper.DrawCompressedMask(_areaMaskProperty, NavTileManager.Instance.AreaManager.AllAreaNames, "Walkable Areas"))
            {
                UpdateAreaSettingsList();
            }
        }

        /// <summary>
        /// Draw the animation settings dropdown section.
        /// </summary>
        private void DrawAnimationSettings()
        {
            if (serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.HelpBox("Multi object editing is not supported for animation settings.", MessageType.Warning);
                return;
            }

            EditorHelper.DrawProperty(_animatorProperty);
            EditorGUILayout.Space();

            InitAnimationTriggers();
            InitAnimationFloatParameters();

            DrawAnimationFloatParamPopup(_speedAnimParamProperty, new GUIContent("Speed Parameter", SPEED_ANIM_PARAM_TOOLTIP));
            DrawAnimationFloatParamPopup(_horizontalAnimParamProperty, new GUIContent("Horizontal Parameter", HORIZONTAL_ANIM_PARAM_TOOLTIP));
            DrawAnimationFloatParamPopup(_verticalAnimParamProperty, new GUIContent("Vertical Parameter", VERTICAL_ANIM_PARAM_TOOLTIP));
            EditorHelper.DrawProperty(_preserveAnimDirectionProperty, new PropertyDrawingOptions()
                                                                    .DoLabel("Preserve Direction")
                                                                    .DoTooltip("Whether the movement parameters should be preserved when the agent stops moving.")
                                                                    .DoReadOnly(_animatorProperty.objectReferenceValue == null));
            EditorGUILayout.Space();

            if (_areaSettingsListProperty.arraySize == 0)
            {
                EditorGUILayout.HelpBox("Cannot display area based animation settings because agent has no walkable areas in its area mask.", MessageType.Warning);
            }
            else
            {
                _areaAnimationsReorderableList.index = Mathf.Max(0, _areaAnimationsReorderableList.index);
                _areaAnimationsReorderableList.DoLayoutList();
            }
        }

        /// <summary>
        /// Draw the area settings dropdown section.
        /// </summary>
        private void DrawAreaSettings()
        {
            if (serializedObject.isEditingMultipleObjects)
            {
                EditorGUILayout.HelpBox("Multi object editing is not supported for area settings.", MessageType.Warning);
                return;
            }

            if (_areaSettingsListProperty.arraySize == 0)
            {
                EditorGUILayout.HelpBox("Cannot display area settings because agent has no walkable areas in its area mask.", MessageType.Warning);
            }
            else
            {
                EditorHelper.DrawProperty(_onAreaChangeUnityEvent);
                EditorGUILayout.Space();

                _areaSpeedsReorderableList.index = Mathf.Max(0, _areaSpeedsReorderableList.index);
                _areaSpeedsReorderableList.DoLayoutList();
            }
        }

        /// <summary>
        /// Draw the path callbacks dropdown section.
        /// </summary>
        private void DrawPathCallbacks()
        {
            EditorHelper.DrawProperty(_onPathFoundUnityEventProperty);
            EditorHelper.DrawProperty(_onPathNotFoundUnityEventProperty);
            EditorHelper.DrawProperty(_onPathAbortedUnityEventProperty);
            EditorHelper.DrawProperty(_onPathFinishedUnityEventProperty);
        }

        /// <summary>
        /// Draw header for the area speeds reorderable list.
        /// </summary>
        private void DrawAreaSpeedsListHeader(Rect inRect)
        {
            DrawAreaSettingsListHeader(inRect, AreaSettingsCategory.Speed);
        }

        /// <summary>
        /// Draw header for the area animations reorderable list.
        /// </summary>
        private void DrawAreaAnimationsListHeader(Rect inRect)
        {
            DrawAreaSettingsListHeader(inRect, AreaSettingsCategory.AnimationTriggerName);
        }

        /// <summary>
        /// Draw the reorderable list header with a variable category label.
        /// </summary>
        private void DrawAreaSettingsListHeader(Rect inRect, AreaSettingsCategory inCategory)
        {
            int previousIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect maskIDRect, colorRect, areaNameRect;
            EditorHelper.VariableRect settingsRect;
            GetAreaSettingsListRects(inRect, out maskIDRect, out colorRect, out areaNameRect, out settingsRect);
            Rect areaLabelRect = new Rect(colorRect.x, colorRect.y, colorRect.width + areaNameRect.width, colorRect.height);

            // Shared labels.
            EditorGUI.LabelField(maskIDRect, new GUIContent("#", "The area's unique ID."));
            EditorGUI.LabelField(areaLabelRect, "Area");

            // Setting label.
            EditorGUI.LabelField(settingsRect.Full, AREA_SETTINGS_DROPDOWN_OPTIONS[(int)inCategory]);

            EditorGUI.indentLevel = previousIndentLevel;
        }

        /// <summary>
        /// Draw element of the area speeds reorderable list.
        /// </summary>
        private void DrawAreaSpeedsListElement(Rect inRect, int inIndex, bool inSelected, bool inFocused)
        {
            DrawAreaSettingsListElement(inRect, inIndex, inSelected, inFocused, AreaSettingsCategory.Speed);
        }

        /// <summary>
        /// Draw element of the area animations reorderable list.
        /// </summary>
        private void DrawAreaAnimationsListElement(Rect inRect, int inIndex, bool inSelected, bool inFocused)
        {
            DrawAreaSettingsListElement(inRect, inIndex, inSelected, inFocused, AreaSettingsCategory.AnimationTriggerName);
        }

        /// <summary>
        /// Draw the reorderable list elements with a variable setting field depending on the passed in category.
        /// </summary>
        private void DrawAreaSettingsListElement(Rect inRect, int inIndex, bool inSelected, bool inFocused, AreaSettingsCategory inCategory)
        {
            int previousIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            SerializedProperty areaSettingsProperty = _areaSettingsListProperty.GetArrayElementAtIndex(inIndex);
            if (areaSettingsProperty == null) { return; }

            int areaIndex = areaSettingsProperty.FindPropertyRelative("AreaIndex").intValue;
            NavTileArea area = NavTileManager.Instance.AreaManager.AllAreas[areaIndex];

            inRect.height -= 2;
            Rect maskIDRect, colorRect, areaNameRect;
            EditorHelper.VariableRect settingsRect;
            GetAreaSettingsListRects(inRect, out maskIDRect, out colorRect, out areaNameRect, out settingsRect);

            // Draw area info.
            EditorGUI.LabelField(maskIDRect, areaIndex.ToString());
            EditorHelper.DrawRectWithOutline(colorRect, area.Color, new Color(area.Color.r / 4f, area.Color.g / 4f, area.Color.b / 4f), 1);
            EditorGUI.LabelField(areaNameRect, area.Name);

            DrawAreaSetting(areaSettingsProperty, settingsRect, inCategory);

            EditorGUI.indentLevel = previousIndentLevel;
        }

        /// <summary>
        /// Draw a variable setting field depending on the passed in category.
        /// </summary>
        private void DrawAreaSetting(SerializedProperty inAreaSettingsProperty, EditorHelper.VariableRect inSettingsRect, AreaSettingsCategory inCategory)
        {
            switch (inCategory)
            {
                case AreaSettingsCategory.Speed:
                    DrawSpeedSetting(inAreaSettingsProperty, inSettingsRect);
                    break;
                case AreaSettingsCategory.AnimationTriggerName:
                    DrawAnimationTriggerNameSetting(inAreaSettingsProperty, inSettingsRect);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draw a speed setting field in the passed in rect.
        /// </summary>
        private void DrawSpeedSetting(SerializedProperty inAreaSettingsProperty, EditorHelper.VariableRect inSettingsRect)
        {
            SerializedProperty speedProperty = inAreaSettingsProperty.FindPropertyRelative(nameof(NavTileAgent.AreaSettings.Speed));

            // Draw speed override checkbox.
            bool useCustomSpeed = EditorGUI.Toggle(inSettingsRect.MiniPrefix, speedProperty.floatValue != -1);
            if (useCustomSpeed && speedProperty.floatValue == -1) { speedProperty.floatValue = Target.Speed; }

            // Draw speed input field.
            using (new EditorGUI.DisabledScope(!useCustomSpeed))
            {
                float inputSpeed = EditorGUI.FloatField(inSettingsRect.FlexibleSuffix, useCustomSpeed ? speedProperty.floatValue : Target.Speed);
                speedProperty.floatValue = useCustomSpeed ? Mathf.Max(0, inputSpeed) : -1;
            }
        }

        /// <summary>
        /// Draw an animation setting field in the passed in rect.
        /// </summary>
        private void DrawAnimationTriggerNameSetting(SerializedProperty inAreaSettingsProperty, EditorHelper.VariableRect inSettingsRect)
        {
            SerializedProperty animationTriggerNameProperty = inAreaSettingsProperty.FindPropertyRelative(nameof(NavTileAgent.AreaSettings.AnimationTriggerName));

            int selectedIndex = Mathf.Max(0, Array.FindIndex(_animationTriggers, x => string.Equals(x.text, animationTriggerNameProperty.stringValue)));

            if (selectedIndex == 0 && !string.IsNullOrEmpty(animationTriggerNameProperty.stringValue))
            {
                animationTriggerNameProperty.stringValue = string.Empty;
            }

            EditorGUI.BeginChangeCheck();

            int newIndex = 0;
            using (new EditorGUI.DisabledScope(_animatorProperty.objectReferenceValue == null))
            {
                newIndex = EditorGUI.Popup(inSettingsRect.Full, GUIContent.none, selectedIndex, _animationTriggers);
            }

            if (EditorGUI.EndChangeCheck())
            {
                animationTriggerNameProperty.stringValue = _animationTriggers[newIndex].text;
            }
        }

        /// <summary>
        /// Initialize the rects required to display the reorderable list of areas in the area mask, along with a rect for a variable setting.
        /// </summary>
        private void GetAreaSettingsListRects(Rect inRect, out Rect outMaskIDRect, out Rect outColorRect, out Rect outAreaNameRect, out EditorHelper.VariableRect outSettingsRect)
        {
            // ID & color rect.
            float maskIDWidth = inRect.height * 1.25f;
            float colorWidth = inRect.height;

            // Settings rect.
            float settingsWidth = inRect.width - EditorGUIUtility.labelWidth + 6; // perfect align :^)

            // Flexible area name label.
            float remainingWidth = inRect.width - maskIDWidth - colorWidth - settingsWidth;
            float padding = 4;
            float areaNameWidth = remainingWidth - padding;
            
            float x = inRect.x;

            outMaskIDRect = new Rect(x, inRect.y, maskIDWidth, inRect.height);
            x += maskIDWidth;
            outColorRect = new Rect(x, inRect.y, colorWidth, inRect.height);
            x += colorWidth;
            outAreaNameRect = new Rect(x + padding, inRect.y, areaNameWidth, inRect.height);
            x += areaNameWidth + padding;
            outSettingsRect = new EditorHelper.VariableRect(new Rect(x, inRect.y, settingsWidth, inRect.height));
        }

        private void InitAnimationTriggers()
        { 
            Animator referencedAnimator = (Animator)_animatorProperty.objectReferenceValue;
            GUIContent[] noneContent = { new GUIContent("None") };

            if (referencedAnimator == null ||
                referencedAnimator.runtimeAnimatorController == null)
            {
                _animationTriggers = noneContent;
                return;
            }

            AnimatorController linkedAnimatorController = referencedAnimator.runtimeAnimatorController as AnimatorController;

            _animationTriggers = noneContent.Concat(from parameter in linkedAnimatorController.parameters
                                    where parameter.type == UnityEngine.AnimatorControllerParameterType.Trigger
                                    select new GUIContent(parameter.name)).ToArray();
        }

        private void InitAnimationFloatParameters()
        {
            Animator referencedAnimator = (Animator)_animatorProperty.objectReferenceValue;
            GUIContent[] noneContent = { new GUIContent("None") };

            if (referencedAnimator == null ||
                referencedAnimator.runtimeAnimatorController == null)
            {
                _animationFloatParameters = noneContent;
                return;
            }

            AnimatorController linkedAnimatorController = referencedAnimator.runtimeAnimatorController as AnimatorController;

            _animationFloatParameters = noneContent.Concat(from parameter in linkedAnimatorController.parameters
                                                    where parameter.type == UnityEngine.AnimatorControllerParameterType.Float
                                                    select new GUIContent(parameter.name)).ToArray();
        }

        /// <summary>
        /// Draw a float parameter popup field for the attached animator, if present. Otherwise draw a greyed out popup field.
        /// </summary>
        private void DrawAnimationFloatParamPopup(SerializedProperty inPropertyToSet, GUIContent inLabel)
        {
            int selectedIndex = Mathf.Max(0, Array.FindIndex(_animationFloatParameters, x => string.Equals(x.text, inPropertyToSet.stringValue)));

            if (selectedIndex == 0 && !string.IsNullOrEmpty(inPropertyToSet.stringValue))
            {
                inPropertyToSet.stringValue = string.Empty;
            }

            EditorGUI.BeginChangeCheck();

            int newIndex = 0;
            using (new EditorGUI.DisabledScope(_animatorProperty.objectReferenceValue == null))
            {
                newIndex = EditorGUILayout.Popup(inLabel, selectedIndex, _animationFloatParameters);
            }

            if (EditorGUI.EndChangeCheck())
            {
                inPropertyToSet.stringValue = _animationFloatParameters[newIndex].text;
            }
        }
    }   
}
