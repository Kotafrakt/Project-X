using Snowcap.NavTiles;
using UnityEditor;
using UnityEngine;

namespace Snowcap.EditorPackage
{
    /// <summary>
    /// Draws a dynamically resizing horizontal banner.
    /// </summary>
    public class ResizingBanner
    {
        /// <summary>
        /// Contains the GUIDs for the textures used by the banner.
        /// </summary>
        public string[] GUIDs { get; private set; } = { "", "", "" };

        private Texture2D _startTexture;
        /// <summary>
        /// The texture used to draw the start of the banner.
        /// </summary>
        public Texture2D StartTexture { get { return _startTexture ?? (_startTexture = EditorHelper.LoadTexture(GUIDs[0])); } }

        private Texture2D _middleTexture;
        /// <summary>
        /// The texture used to draw the dynamically resizing center of the banner.
        /// </summary>
        public Texture2D MiddleTexture { get { return _middleTexture ?? (_middleTexture = EditorHelper.LoadTexture(GUIDs[1])); } }
        
        private Texture2D _endTexture;
        /// <summary>
        /// The texture used to draw the end of the banner.
        /// </summary>
        public Texture2D EndTexture { get { return _endTexture ?? (_endTexture = EditorHelper.LoadTexture(GUIDs[2])); } }

        private GUIStyle _middleStyle;
        private GUIStyle MiddleStyle { get { return _middleStyle ?? InitializeMiddleStyle(); } }

        private GUIStyle _startEndStyle;
        private GUIStyle StartEndStyle { get { return _startEndStyle ?? InitializeStartEndStyle(); } }

        /// <summary>
        /// Initializes the banner's textures'.
        /// </summary>
        /// <param name="inGUIDs">The GUID's for the 3 textures used by the banner.</param>
        public ResizingBanner(string[] inGUIDs)
        {
            if (inGUIDs.Length == 3)
            {
                GUIDs = inGUIDs;
            }
            else
            {
                Debug.LogError("ResizingBanner requires exactly 3 GUIDs for the start, middle and end textures used to draw the banner.");
            }
        }

        /// <summary>
        /// Draws the resizable banner in its own horizontal group.
        /// The banner dynamically resizes to fit its own group.
        /// </summary>
        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Box(StartTexture, StartEndStyle);
            GUILayout.Box(GUIContent.none, MiddleStyle, GUILayout.ExpandWidth(true));
            GUILayout.Box(EndTexture, StartEndStyle);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Initializes the style used for the middle texture in the banner.
        /// </summary>
        private GUIStyle InitializeMiddleStyle()
        {
            _middleStyle = new GUIStyle();
            _middleStyle.alignment = TextAnchor.MiddleCenter;
            _middleStyle.fixedHeight = MiddleTexture.height;
            _middleStyle.normal.background = MiddleTexture;
            return _middleStyle;
        }
        /// <summary>
        /// Initializes the style used for the first and last textures in the banner.
        /// </summary>
        private GUIStyle InitializeStartEndStyle()
        {
            _startEndStyle = new GUIStyle();
            _startEndStyle.alignment = TextAnchor.MiddleCenter;
            _startEndStyle.stretchWidth = false;
            return _startEndStyle;
        }
    }
}
