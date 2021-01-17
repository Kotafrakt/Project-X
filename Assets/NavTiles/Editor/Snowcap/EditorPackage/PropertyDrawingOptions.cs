using UnityEngine;

namespace Snowcap.EditorPackage
{
    /// <summary>
    /// Helper class for drawing properties with EditorHelper.DrawProperty.
    /// </summary>
    public class PropertyDrawingOptions
    {
        public Rect? Rect { get; private set; }
        public string Label { get; private set; }
        public string Tooltip { get; private set; }
        public bool ReadOnly { get; private set; }
        public bool HideLabel { get; private set; }
        public bool IncludeChildren { get; private set; }
        public Texture Texture { get; private set; }
        public GUILayoutOption[] LayoutOptions { get; private set; }

        /// <summary>
        /// Draws the property in the given rect.
        /// Overrides DoLayoutOptions().
        /// </summary>
        public PropertyDrawingOptions DoRect(Rect inRect) { Rect = inRect; return this; }

        /// <summary>
        /// Draws a custom label for the property.
        /// Overridden by DoHideLabel().
        /// </summary>
        public PropertyDrawingOptions DoLabel(string inLabel) { Label = inLabel; return this; }

        /// <summary>
        /// Adds a tooltip to the property.
        /// Overridden by DoHideLabel().
        /// </summary>
        public PropertyDrawingOptions DoTooltip(string inTooltip) { Tooltip = inTooltip; return this; }

        /// <summary>
        /// Makes the property greyed out in the inspector.
        /// </summary>
        public PropertyDrawingOptions DoReadOnly(bool inReadOnly) { ReadOnly = inReadOnly; return this; }

        /// <summary>
        /// Hides the property's label.
        /// Overrides DoLabel(), DoTooltip() and DoTexture().
        /// </summary>
        public PropertyDrawingOptions DoHideLabel(bool inHideLabel) { HideLabel = inHideLabel; return this; }

        /// <summary>
        /// Draws the property's children.
        /// Overrides DoRect() height.
        /// </summary>
        public PropertyDrawingOptions DoIncludeChildren(bool inIncludeChildren) { IncludeChildren = inIncludeChildren; return this; }

        /// <summary>
        /// Draws a texture in front of the property's label.
        /// Overridden by DoHideLabel().
        /// </summary>
        public PropertyDrawingOptions DoTexture(Texture inTexture) { Texture = inTexture; return this; }

        /// <summary>
        /// Draws the label with the provided GUILayoutOptions.
        /// Overridden by DoRect().
        /// </summary>
        public PropertyDrawingOptions DoLayoutOptions(params GUILayoutOption[] inOptions) { LayoutOptions = inOptions; return this; }
    }
}
