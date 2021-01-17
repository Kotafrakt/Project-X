using System.Collections.ObjectModel;
using Snowcap.EditorPackage;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Snowcap.NavTiles
{
    public partial class NavTileWindow
    {
        private SerializedProperty _agentManager;
        private SerializedProperty _agents;

        private SerializedProperty _agentTypeMatrixKeys;
        private SerializedProperty _agentTypeMatrixValues;

        private ReorderableList _agentsList = null;
        private bool _duplicateAgents = false;

        /// <summary>
        ///  Static function to open the window and show the agents settings from any location.
        /// </summary>
        public static void OpenAgentsTab()
        {
            ShowWindow();
            if (Window == null) { return; }
            Window._activeTab = SettingsTab.Agents;
        }

        /// <summary>
        /// Displays the contents of the Agent tab.
        /// </summary>
        private void DoAgentsTab()
        {
            if (_agents == null || _agentsList == null)
            {
                InitAgents();
            }

            _duplicateAgents = false;
            _agentsList.index = Mathf.Max(0, _agentsList.index);
            _agentsList.DoLayoutList();

            if (_duplicateAgents)
            {
                EditorGUILayout.HelpBox("Duplicate agent names are not supported. Please remove duplicate entries.", MessageType.Error);
            }

            EditorGUILayout.Space();

            DrawAgentMatrix();
        }

        /// <summary>
        /// Initializes the variables needed for the Agent tab.
        /// </summary>
        private void InitAgents()
        {
            if (_agentManager == null)
            {
                _agentManager = _navTileSerializedObject.FindProperty("_agentManager");
            }

            if (_agents == null)
            {
                _agents = _agentManager.FindPropertyRelative("_agents");
            }

            if (_agentTypeMatrixKeys == null || _agentTypeMatrixValues == null)
            {
                _agentTypeMatrixKeys = _agentManager.FindPropertyRelative("_conflictMatrix").FindPropertyRelative("_keys");
                _agentTypeMatrixValues = _agentManager.FindPropertyRelative("_conflictMatrix").FindPropertyRelative("_values");
            }

            if (_agentsList == null)
            {
                _agentsList = new ReorderableList(_navTileSerializedObject, _agents, false, false, true, true)
                {
                    drawHeaderCallback = DrawAgentsListHeader,
                    drawElementCallback = DrawAgentsListElement,
                    elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                };
            }
        }

        /// <summary>
        /// Draws the header of the reorderable list.
        /// </summary>
        private void DrawAgentsListHeader(Rect inRect)
        {
            EditorGUI.LabelField(inRect, "Agent Types");
        }

        /// <summary>
        /// Draws a single element of the reorderable list. 
        /// </summary>
        private void DrawAgentsListElement(Rect inRect, int inIndex, bool inSelected, bool inFocused)
        {
            if (_agents == null)
                return;

            SerializedProperty agentProperty = _agents.GetArrayElementAtIndex(inIndex);
            
            if (agentProperty == null)
                return;

            inRect.height -= 2;
            inRect.y += 1;
            Rect idRect, nameRect;
            GetAgentsListRects(inRect, out idRect, out nameRect);

            EditorGUI.LabelField(idRect, inIndex.ToString());

            bool readOnly = inIndex == 0;

            // Highlight duplicate agent names.
            bool duplicateEntry = NavTileManager.Instance.AgentManager.IsDuplicateEntry(agentProperty.stringValue);
            Texture2D prevTex = EditorStyles.textField.normal.background;
            if (duplicateEntry)
            {
                _duplicateAgents = true;
                EditorStyles.textField.normal.background = null;
                EditorStyles.textField.active.background = null;
                EditorStyles.textField.focused.background = null;
                Color backgroundColor = new Color(1f, 0f, 0f, inIndex == 0 ? .25f : .45f);
                Color outlineColor = new Color(backgroundColor.r / 2f, 0f, 0f, 1f);
                EditorHelper.DrawRectWithOutline(nameRect, backgroundColor, outlineColor, 1);
            }
            EditorHelper.DrawProperty(agentProperty, new PropertyDrawingOptions().DoRect(nameRect).DoHideLabel(true).DoReadOnly(readOnly));
            EditorStyles.textField.normal.background = prevTex;
            EditorStyles.textField.active.background = prevTex;
            EditorStyles.textField.focused.background = prevTex;
        }

        /// <summary>
        /// Returns the size of the fields shown in one element.
        /// </summary>
        private void GetAgentsListRects(Rect inRect, out Rect outIDRect, out Rect outNameRect)
        {
            float idWidth = EditorGUIUtility.singleLineHeight * 1.5f;
            float nameWidth = inRect.width - idWidth;

            float x = inRect.x;

            outIDRect = new Rect(x, inRect.y, idWidth - 4, inRect.height);
            x += idWidth;
            outNameRect = new Rect(x, inRect.y, nameWidth - 4, inRect.height);
        }

        /// <summary>
        /// Draws the Conflict Matrix showing the filled out agents.
        /// </summary>
        private void DrawAgentMatrix()
        {
            ReadOnlyCollection<string> Agents = NavTileManagerReference.AgentManager.Agents;

            const int checkboxSize = 16;
            int labelSize = 0;
            const int indent = 10;

            int numLayers = 0;
            for (int i = 0; i < Agents.Count; i++)
                if (Agents[i] != "")
                    numLayers++;

            // Find the longest label.
            for (int i = 0; i < Agents.Count; i++)
            {
                var textDimensions = GUI.skin.label.CalcSize(new GUIContent(Agents[i]));
                if (labelSize < textDimensions.x)
                    labelSize = (int)textDimensions.x;
            }

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Agent Conflict Matrix", EditorStyles.boldLabel);

            // Inspired by:
            // https://github.com/Unity-Technologies/UnityCsReference/blob/9034442437e6b5efe28c51d02e978a96a3ce5439/Editor/Mono/Inspector/PhysicsManagerInspector.cs
            var scrollStatePosOffset = 115;
            var topLabelRect = GUILayoutUtility.GetRect(checkboxSize + labelSize, labelSize);
            var topLeft = new Vector2(topLabelRect.x, topLabelRect.y);
            var y = 0;
            for (int i = 0; i < Agents.Count; i++)
            {
                if (Agents[i] != "")
                {
                    var translate = new Vector3(labelSize + indent + checkboxSize * (numLayers - y) + topLeft.x - _scrollPos.x + scrollStatePosOffset, topLeft.y - _scrollPos.y + scrollStatePosOffset, 0);
                    GUI.matrix = Matrix4x4.TRS(translate, Quaternion.Euler(0, 0, 90), Vector3.one);
                    GUI.Label(new Rect(_scrollPos.x, _scrollPos.y, labelSize, checkboxSize), Agents[i], "RightLabel");
                    y++;
                }
            }

            GUI.matrix = Matrix4x4.identity;
            y = 0;
            for (int i = 0; i < Agents.Count; i++)
            {
                if (Agents[i] != "")
                {
                    int x = 0;
                    var r = GUILayoutUtility.GetRect(indent + checkboxSize * Agents.Count + labelSize, checkboxSize);
                    GUI.Label(new Rect(r.x + indent, r.y, labelSize, checkboxSize), Agents[i], "RightLabel");
                    for (int j = Agents.Count - 1; j >= 0; j--)
                    {
                        if (Agents[j] != "")
                        {
                            if (x < numLayers - y)
                            {
                                var tooltip = new GUIContent("", Agents[i] + "/" + Agents[j]);

                                bool newVal = NavTileManagerReference.AgentManager.GetValue(i, j);

                                bool toggle = GUI.Toggle(new Rect(labelSize + indent + r.x + x * checkboxSize, r.y, checkboxSize, checkboxSize), newVal, tooltip);

                                // Check if the value changed. Only then try and get the serialized entry and save the change.
                                // This helps a lot with performance.
                                if (toggle != newVal)
                                {
                                    int correspondingHash = NavTileManagerReference.AgentManager.GetMatrixHash(i, j);

                                    int index;

                                    if (!NavTileManagerReference.AgentManager.GetIndexOfHashInKeysList(correspondingHash, out index))
                                    {
                                        _agentTypeMatrixKeys.InsertArrayElementAtIndex(index);
                                        _agentTypeMatrixKeys.GetArrayElementAtIndex(index).intValue = correspondingHash;

                                        _agentTypeMatrixValues.InsertArrayElementAtIndex(index);
                                    }

                                    SerializedProperty value = _agentTypeMatrixValues.GetArrayElementAtIndex(index);

                                    value.boolValue = toggle;
                                }
                            }
                            x++;
                        }
                    }
                    y++;
                }
            }

            EditorGUILayout.HelpBox("A ticked box represents two agent types that will wait for each other when traversing a path.", MessageType.Info);

            EditorGUILayout.EndVertical();
        }
    }
}
