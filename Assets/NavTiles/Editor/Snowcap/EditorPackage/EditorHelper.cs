using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Snowcap.EditorPackage
{
    /// <summary>
    /// Custom helper functions for the Unity Editor, custom inspectors and editor windows.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// Contains inner rects that specify the bounds for fields contained in the provided outer rect.
        /// Can be used to show a field with a toggle in front of it, for instance.
        /// </summary>
        public class VariableRect
        {
            private Rect _full;
            public Rect Full
            {
                get { return _full; }
                set
                {
                    _full = value;
                    MiniPrefix = new Rect(_full.x, _full.y, EditorGUIUtility.singleLineHeight, _full.height);
                    FlexibleSuffix = new Rect(_full.x + MiniPrefix.width, _full.y, _full.width - MiniPrefix.width, _full.height);
                }
            }
            public Rect MiniPrefix { get; private set; }
            public Rect FlexibleSuffix { get; private set; }

            public VariableRect(Rect inRect) { Full = inRect; }
        }

        public const int MEDIUM_BUTTON_WIDTH = 95;

        //////////////////////////////////
        ////////// FILE LOADING //////////
        
        public static Texture2D LoadTexture(string inGUID)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(inGUID);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
        }

        ////////// FILE LOADING //////////
        //////////////////////////////////

        ///////////////////////////////////
        ////////// GUI LAYOUTING //////////

        /// <summary>
        /// Begins a horizontal layout group and adds a flexible space to the start of it.
        /// </summary>
        public static void BeginFlexibleHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        }
        /// <summary>
        /// Places a flexible space at the end of the current horizontal layout group and then ends it.
        /// </summary>
        public static void EndFlexibleHorizontal()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a colored rectangle with a colored outline of custom thickness.
        /// </summary>
        public static void DrawRectWithOutline(Rect inRect, Color inFillColor, Color inOutlineColor, int inOutlineThickness)
        {
            EditorGUI.DrawRect(inRect, inFillColor);

            EditorGUI.DrawRect(new Rect(inRect.x, inRect.y, inOutlineThickness, inRect.height), inOutlineColor);
            EditorGUI.DrawRect(new Rect(inRect.x + inRect.width - inOutlineThickness, inRect.y, inOutlineThickness, inRect.height), inOutlineColor);
            EditorGUI.DrawRect(new Rect(inRect.x + inOutlineThickness, inRect.y, inRect.width - inOutlineThickness - 1, inOutlineThickness), inOutlineColor);
            EditorGUI.DrawRect(new Rect(inRect.x + inOutlineThickness, inRect.y + inRect.height - inOutlineThickness, inRect.width - inOutlineThickness - 1, inOutlineThickness), inOutlineColor);
        }

        ////////// GUI LAYOUTING //////////
        ///////////////////////////////////
        
        /////////////////////////////////////////////////
        ////////// SERIALIZED PROPERTY DRAWING //////////
        
        private static GUIContent ContentFromDrawingOptions(PropertyDrawingOptions inOptions, string inDefaultLabel)
        {
            if (inOptions == null) { return new GUIContent(inDefaultLabel); }

            GUIContent content = new GUIContent(GUIContent.none);
            string label = inOptions.HideLabel ? null : ObjectNames.NicifyVariableName(inOptions.Label ?? inDefaultLabel);

            if (label != null)
            {
                content.text = label;
                content.image = inOptions.Texture;
                content.tooltip = inOptions.Tooltip;
            }

            return content;
        }

        private static void BeginReadOnly(out bool outPreviousGUIState, bool inReadOnly)
        {
            outPreviousGUIState = GUI.enabled;
            if (inReadOnly) { GUI.enabled = false; }
        }

        private static void EndReadOnly(bool inPreviousGUIState)
        {
            GUI.enabled = inPreviousGUIState;
        }

        /// <summary>
        /// Draws the given property.
        /// </summary>
        private static void DrawPropertyInternal(SerializedProperty inProperty, PropertyDrawingOptions inOptions = null)
        {
            if (inOptions == null)
            {
                EditorGUILayout.PropertyField(inProperty, true);
                return;
            }

            GUIContent content = ContentFromDrawingOptions(inOptions, inProperty.name);

            // Begin read only block.
            bool previousGUIState;
            BeginReadOnly(out previousGUIState, inOptions.ReadOnly);

            if (inOptions.Rect == null)
            {
                EditorGUILayout.PropertyField(inProperty, content, inOptions.IncludeChildren, inOptions.LayoutOptions);
            }
            else
            {
                EditorGUI.PropertyField(inOptions.Rect.Value, inProperty, content, inOptions.IncludeChildren);
            }

            // End read only block.
            EndReadOnly(previousGUIState);
        }

        /// <summary>
        /// Draws the given property as it would be drawn in a normal inspector window.
        /// </summary>
        public static void DrawProperty(SerializedProperty inProperty)
        {
            DrawPropertyInternal(inProperty);
        }
        /// <summary>
        /// Draws the given property with custom layouting options.
        /// </summary>
        public static void DrawProperty(SerializedProperty inProperty, PropertyDrawingOptions inOptions)
        {
            DrawPropertyInternal(inProperty, inOptions);
        }

        /// <summary>
        /// Draws a compressed mask field for the passed in mask values, filtering out null strings.
        /// Returns whether the mask was modified.
        /// </summary>
        public static bool DrawCompressedMask(SerializedProperty inMaskProperty, List<string> inMaskOptions, string inlabel = null)
        {
            List<string> compressedMaskOptions = new List<string>();
            int currentMask = inMaskProperty.intValue;
            int compressedMask = 0;

            for (int index = 0, compressedIndex = 0; index < inMaskOptions.Count; index++)
            {
                if (string.IsNullOrEmpty((inMaskOptions[index])))
                    continue;

                if (((1 << index) & currentMask) != 0)
                    compressedMask |= (1 << compressedIndex);

                compressedMaskOptions.Add((inMaskOptions[index]));
                compressedIndex++;
            }

            bool changed;
            EditorGUI.BeginChangeCheck();
            string label = inlabel ?? ObjectNames.NicifyVariableName(inMaskProperty.name);

            EditorGUI.showMixedValue = inMaskProperty.hasMultipleDifferentValues;
            int areaMask = EditorGUILayout.MaskField(label, compressedMask, compressedMaskOptions.ToArray(), EditorStyles.layerMaskField);
            EditorGUI.showMixedValue = false;

            if (changed = EditorGUI.EndChangeCheck())
            {
                int newMask = 0;

                for (int index = 0, compressedIndex = 0; index < inMaskOptions.Count; index++)
                {
                    if (string.IsNullOrEmpty((inMaskOptions[index])))
                        continue;

                    if (((1 << compressedIndex) & areaMask) != 0)
                    {
                        newMask |= (1 << index);
                    }

                    compressedIndex++;
                }

                inMaskProperty.intValue = newMask;
            }

            return changed;
        }

        /// <summary>
        /// Draws a compressed popup field for the passed in popup values, filtering out null strings.
        /// Returns whether the popup was modified.
        /// </summary>
        public static bool DrawCompressedPopup(SerializedProperty inPopupProperty, List<string> inPopupOptions)
        {
            List<string> compressedPopupOptions = new List<string>();
            int currentValue = inPopupProperty.intValue;
            int compressedValue = 0;

            for (int index = 0, compressedIndex = 0; index < inPopupOptions.Count; index++)
            {
                if (string.IsNullOrEmpty((inPopupOptions[index])))
                    continue;

                if (index == currentValue)
                    compressedValue = compressedIndex;

                compressedPopupOptions.Add((inPopupOptions[index]));
                compressedIndex++;
            }

            bool changed;
            EditorGUI.BeginChangeCheck();
            string label = ObjectNames.NicifyVariableName(inPopupProperty.name);

            EditorGUI.showMixedValue = inPopupProperty.hasMultipleDifferentValues;
            int newAreaIndex = EditorGUILayout.Popup(label, compressedValue, compressedPopupOptions.ToArray());
            EditorGUI.showMixedValue = false;

            if (changed = EditorGUI.EndChangeCheck())
            {
                for (int index = 0, compressedIndex = 0; index < inPopupOptions.Count; index++)
                {
                    if (string.IsNullOrEmpty((inPopupOptions[index])))
                        continue;

                    if (newAreaIndex == compressedIndex)
                    {
                        inPopupProperty.intValue = index;
                        break;
                    }

                    compressedIndex++;
                }
            }

            return changed;
        }

        ////////// SERIALIZED PROPERTY DRAWING //////////
        /////////////////////////////////////////////////
    }
}
