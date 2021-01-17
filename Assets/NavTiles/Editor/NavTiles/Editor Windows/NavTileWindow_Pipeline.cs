using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;
using UnityEngine;
using Snowcap.EditorPackage;
using UnityEditorInternal;

namespace Snowcap.NavTiles
{
    public partial class NavTileWindow
    {
        private static int _currentAlgorithmIndex = 0;

        private List<Type> _pathingAlgorithms;
        private string[] _pathingAlgorithmNames;
        private List<Type> _smoothingAlgorithms;
        private GUIContent[] _smoothingAlgorithmNames;

        private SerializedProperty _pipelineManagerProperty;
        private SerializedProperty _multiThreadingEnabledProperty;
        private SerializedProperty _numberOfThreadsProperty;
        private SerializedProperty _algorithmTypeProperty;
        private SerializedProperty _enabledSmoothingAlgorithmsProperty;

        private ReorderableList _smoothingAlgorithmsReorderableList;

        private GUIContent _indexContent = new GUIContent("#", "The order in which the smoothing algorithms will be applied.");
        private GUIContent _algorithmNameContent = new GUIContent("Smoothing Algorithm", "The name of the smoothing algorithm to apply.");

        private const string ALGORITHM_SELECTION_WINDOW_TITLE = "Algorithm Selection";

        /// <summary>
        ///  Static function to open the window and show the pipeline settings from any location.
        /// </summary>
        public static void OpenPipelineTab()
        {
            ShowWindow();
            if (Window == null) { return; }
            Window._activeTab = SettingsTab.Pipeline;
        }

        /// <summary>
        /// Initializes the Pipeline variables.
        /// </summary>
        private void InitializePipelineTab()
        {
            _pipelineManagerProperty = _navTileSerializedObject.FindProperty("_pipelineManager");
            _multiThreadingEnabledProperty = _pipelineManagerProperty.FindPropertyRelative(nameof(NavTilePipelineManager.MultiThreadingEnabled));
            _numberOfThreadsProperty = _pipelineManagerProperty.FindPropertyRelative(nameof(NavTilePipelineManager.NumberOfThreads));
            _algorithmTypeProperty = _pipelineManagerProperty.FindPropertyRelative(nameof(NavTilePipelineManager.AlgorithmType));
            _enabledSmoothingAlgorithmsProperty = _pipelineManagerProperty.FindPropertyRelative(nameof(NavTilePipelineManager.EnabledSmoothingAlgorithms));

            _smoothingAlgorithmsReorderableList = new ReorderableList(_navTileSerializedObject, _enabledSmoothingAlgorithmsProperty, true, false, true, true)
            {
                drawHeaderCallback = DrawSmoothingAlgorithmsListHeader,
                drawElementCallback = DrawSmoothingAlgorithmsListElement,
                elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
            };
        }

        /// <summary>
        /// Called when the Pipeline tab is opened or reloaded.
        /// </summary>
        private void OnLoadPipelineTab()
        {
            _pathingAlgorithms = NavTileManagerReference.PipelineManager.GetPathfindingAlgorithms();

            _pathingAlgorithmNames = _pathingAlgorithms.Select(x => (Activator.CreateInstance(x) as IPathfindingAlgorithm).GetName()).ToArray();

            _smoothingAlgorithms = NavTileManagerReference.PipelineManager.GetSmoothingAlgorithms();
            _smoothingAlgorithmNames = _smoothingAlgorithms.Select(x => new GUIContent((Activator.CreateInstance(x) as INavTilePathModifier).GetName())).ToArray();
        }

        /// <summary>
        /// Controls and displays all Pipeline settings.
        /// </summary>
        private void DoPipelineTab()
        {
            EditorHelper.DrawProperty(_multiThreadingEnabledProperty, new PropertyDrawingOptions().DoLabel("Multi-Threaded").DoTooltip("Whether to use multi-threading for agent path calculation."));
            if (_multiThreadingEnabledProperty.boolValue)
            {
                GUIContent threadsLabel = new GUIContent("Number Of Threads", "The number of additional threads to run besides the main thread.");
                _numberOfThreadsProperty.intValue = Mathf.Max(EditorGUILayout.IntField(threadsLabel, _numberOfThreadsProperty.intValue), 1);
            }

            EditorGUILayout.Space();
            
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Pathfinding", EditorStyles.boldLabel);

            // Reload tab if the pathing algorithms cannot be found.
            if (_pathingAlgorithms == null)
            {
                OnLoadPipelineTab();
            }

            // In case the order changes of the classes, the selected Type is saved in NavTile2D.
            // If this has been set before, set the index of the popup to match the previously selected algorithm.
            if (!string.IsNullOrEmpty(_algorithmTypeProperty.stringValue))
            {
                _currentAlgorithmIndex = Mathf.Max(_pathingAlgorithms.FindIndex(x => _algorithmTypeProperty.stringValue == x.AssemblyQualifiedName), 0);
            }
            else
            {
                _currentAlgorithmIndex = 0;
            }

            // Algorithm selection dropdown window.
            _currentAlgorithmIndex = EditorGUILayout.Popup("Algorithm", _currentAlgorithmIndex, _pathingAlgorithmNames);
            _algorithmTypeProperty.stringValue = _pathingAlgorithms[_currentAlgorithmIndex].AssemblyQualifiedName;

            if (GUILayout.Button("Select Best Possible Algorithm"))
            {
                SetBestAlgorithm();
            }

            if (_algorithmTypeProperty.stringValue.Equals(typeof(JPS).AssemblyQualifiedName))
            {
                EditorGUILayout.HelpBox(GetAlgorithmName<JPS>() + " will only work if a uniform cost grid is used. If you use any area with a cost other than 0, choose " + GetAlgorithmName<AStar>() + "." +
                                        "\n\nIf a uniform cost grid is used, this warning can be ignored.", MessageType.Warning);
            }
            else if (_algorithmTypeProperty.stringValue.Equals(typeof(JPSPlus).AssemblyQualifiedName))
            {
                EditorGUILayout.HelpBox(GetAlgorithmName<JPSPlus>() + " will only work if there is no diagonal movement, " +
                                        "no hexagonal grid, no dynamically changing grid and no custom areas used (other than the default two)." +
                                        "\n\nIf all these requirements are met, this warning can be ignored.", MessageType.Warning);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Smoothing Algorithms", EditorStyles.boldLabel);
            _smoothingAlgorithmsReorderableList.index = Mathf.Max(0, _smoothingAlgorithmsReorderableList.index);
            _smoothingAlgorithmsReorderableList.DoLayoutList();

            EditorGUILayout.HelpBox("Smoothing algorithms are applied in a sequence. Only use multiple if you're using custom smoothing algorithms.", MessageType.Info);

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Callback for drawing the smoothing algorithm reorderable list header.
        /// </summary>
        private void DrawSmoothingAlgorithmsListHeader(Rect inRect)
        {
            Rect indexRect, algorithmNameRect;
            GetSmoothingAlgorithmsListRects(inRect, out indexRect, out algorithmNameRect);
            int indent = 12;
            Rect indentedAlgorithmNameRect = new Rect(algorithmNameRect.x + indent, algorithmNameRect.y, algorithmNameRect.width - indent, algorithmNameRect.height);

            EditorGUI.LabelField(indexRect, _indexContent);
            EditorGUI.LabelField(indentedAlgorithmNameRect, _algorithmNameContent);
        }

        /// <summary>
        /// Callback for drawing the smoothing algorithm reorderable list element.
        /// </summary>
        private void DrawSmoothingAlgorithmsListElement(Rect inRect, int inIndex, bool inSelected, bool inFocused)
        {
            SerializedProperty smoothingAlgorithmProperty = _enabledSmoothingAlgorithmsProperty.GetArrayElementAtIndex(inIndex);
            if (smoothingAlgorithmProperty == null) { return; }
            int smoothingAlgorithmNameIndex = _smoothingAlgorithms.FindIndex(x => x.FullName == smoothingAlgorithmProperty.stringValue);

            inRect.height -= 2;
            Rect indexRect, algorithmNameRect;
            GetSmoothingAlgorithmsListRects(inRect, out indexRect, out algorithmNameRect);

            // Draw algorithm info.
            int order = inIndex + 1;
            int displayedAlgorithmIndex = Mathf.Max(0, smoothingAlgorithmNameIndex);
            EditorGUI.LabelField(indexRect, order.ToString());
            smoothingAlgorithmNameIndex = EditorGUI.Popup(algorithmNameRect, displayedAlgorithmIndex, _smoothingAlgorithmNames);
            smoothingAlgorithmProperty.stringValue = _smoothingAlgorithms[smoothingAlgorithmNameIndex].FullName;
        }

        /// <summary>
        /// Returns the sizes of the elements in one row of the reorderable list.
        /// </summary>
        private void GetSmoothingAlgorithmsListRects(Rect inRect, out Rect outIndexRect, out Rect outAlgorithmNameRect)
        {
            float indexWidth = inRect.height * 1.25f;
            float algorithmNameWidth = inRect.width - indexWidth;

            float x = inRect.x;

            outIndexRect = new Rect(x, inRect.y, indexWidth, inRect.height);
            x += indexWidth;
            outAlgorithmNameRect = new Rect(x, inRect.y, algorithmNameWidth, inRect.height);
        }

        /// <summary>
        /// Dialogue to select the best fitting algorithm.
        /// </summary>
        private void SetBestAlgorithm()
        {
            int dialogResult;

            if (!IsUniformCostGridUsed())
            {
                SetAlgorithm<AStar>("One or more areas have a cost other than 0. Only " + GetAlgorithmName<AStar>() + " supports areas with a non-zero cost. The cost of an area can be changed in Areas.");
                return;
            }

            // Check if only 2 areas are used.
            if (NavTileManager.Instance.AreaManager.UsedAreas.Count > 2)
            {
                dialogResult = EditorUtility.DisplayDialogComplex(ALGORITHM_SELECTION_WINDOW_TITLE, "You appear to have custom areas defined. Are you using any of these custom areas?", "Yes", "No", "Cancel");
                if (dialogResult == 2)
                    return;

                if (dialogResult == 0)
                {
                    SetAlgorithm<JPS>(GetAlgorithmName<JPS>() + " supports custom area types and is faster than " + GetAlgorithmName<AStar>() + ".");
                    return;
                }
            }

            dialogResult = EditorUtility.DisplayDialogComplex(ALGORITHM_SELECTION_WINDOW_TITLE, "Does the application support diagonal movement?", "Yes", "No", "Cancel");
            if (dialogResult == 2)
                return;

            bool diagonalAllowed = dialogResult == 0;

            if (diagonalAllowed == true)
            {
                SetAlgorithm<JPS>(GetAlgorithmName<JPS>() + " supports diagonal movement and is faster than " + GetAlgorithmName<AStar>() + ".");
                return;
            }

            dialogResult = EditorUtility.DisplayDialogComplex(ALGORITHM_SELECTION_WINDOW_TITLE, "Does the application use a hexagonal grid?", "Yes", "No", "Cancel");
            if (dialogResult == 2)
                return;

            bool hexagonalUsed = dialogResult == 0;

            if (hexagonalUsed == true)
            {
                SetAlgorithm<JPS>(GetAlgorithmName<JPS>() + " supports hexagonal grids and is faster than " + GetAlgorithmName<AStar>() + ".");
                return;
            }

            dialogResult = EditorUtility.DisplayDialogComplex(ALGORITHM_SELECTION_WINDOW_TITLE, "Does the application support a dynamically changing grid (changing tiles at run-time)?", "Yes", "No", "Cancel");
            if (dialogResult == 2)
                return;

            bool dynamicallyChanging = dialogResult == 0;

            if (dynamicallyChanging == true)
            {
                SetAlgorithm<JPS>(GetAlgorithmName<JPS>() + " supports dynamically changing grids and is faster than " + GetAlgorithmName<AStar>() + ".");
                return;
            }

            SetAlgorithm<JPSPlus>(GetAlgorithmName<JPSPlus>() + " is the fastest available algorithm given your answers.");
        }

        /// <summary>
        /// Check to see if the grid uses uniform costs.
        /// </summary>
        private bool IsUniformCostGridUsed()
        {
            foreach (NavTileArea area in NavTileManager.Instance.AreaManager.UsedAreas)
            {
                if (area.Cost != 0)
                    return false;
            }

            return true;
        }

        private string GetAlgorithmName<T>() where T : IPathfindingAlgorithm
        {
            int selectedAlgorithmIndex = _pathingAlgorithms.FindIndex(x => typeof(T).AssemblyQualifiedName == x.AssemblyQualifiedName);
            return _pathingAlgorithmNames[selectedAlgorithmIndex];
        }

        /// <summary>
        /// Generic function to set the pathfinding algorithm in the Pipeline Tab. 
        /// </summary>
        private void SetAlgorithm<T>(string inReason = null) where T : IPathfindingAlgorithm
        {
            NavTileManager.Instance.PipelineManager.SetAlgorithm<T>();

            EditorUtility.DisplayDialog(ALGORITHM_SELECTION_WINDOW_TITLE, "The best available algorithm is: " + GetAlgorithmName<T>() +
                                        (inReason != null ? ("\n\nThe reason for this is:\n" + inReason) : ""),
                                        "Ok");
        }
    }
}
