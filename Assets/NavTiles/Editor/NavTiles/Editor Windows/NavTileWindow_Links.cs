using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    public partial class NavTileWindow
    {
        private List<NavLink> _duplicateLinks = new List<NavLink>();

        /// <summary>
        ///  Static function to open the window and show the NavLink settings from any location.
        /// </summary>
        public static void OpenLinksTab()
        {
            ShowWindow();
            if (Window == null) { return; }
            Window._activeTab = SettingsTab.Links;
        }

        /// <summary>
        /// Display all NavLinks and the ability to refresh them.
        /// </summary>
        private void DoLinksTab()
        {
            // Handle duplicate links.
            if (_duplicateLinks.Count > 0)
            {
                string warningMessage = "Duplicate links found at the following path(s):";

                foreach (NavLink link in _duplicateLinks)
                {
                    warningMessage += "\n - " + AssetDatabase.GetAssetPath(link);
                }

                EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
            }

            // Overall box.
            GUIStyle boxStyle = new GUIStyle("Box");
            boxStyle.padding = new RectOffset();

            EditorGUILayout.BeginVertical(boxStyle);

            // Headers.
            EditorGUILayout.BeginHorizontal();

            GUIStyle headerStyle = new GUIStyle("TextField");
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.normal.background = null;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 14;

            GUILayout.Box("Tiles", headerStyle, GUILayout.ExpandWidth(true));
            GUILayout.Box("Links", headerStyle, GUILayout.ExpandWidth(true));

            EditorGUILayout.EndHorizontal();

            var navLinkPairs = NavTileManagerReference.LinkManager.GetAllPairs();

            if (navLinkPairs.Count > 0)
            {
                // Define all styles.
                GUIStyle tileStyle = new GUIStyle("TextField");
                tileStyle.fixedWidth = (EditorGUIUtility.currentViewWidth - EditorStyles.inspectorDefaultMargins.padding.horizontal - 16) / 2;
                tileStyle.fixedHeight = 16;
                tileStyle.stretchWidth = false;
                tileStyle.imagePosition = ImagePosition.ImageLeft;

                GUIStyle linkStyle = new GUIStyle(tileStyle);
                linkStyle.margin.right = 0;

                GUIStyle labelStyle = new GUIStyle("Label");
                labelStyle.padding = new RectOffset();
                labelStyle.fontStyle = FontStyle.Bold;
                labelStyle.margin = new RectOffset(0, 0, 2, 2);
                labelStyle.fixedWidth = 8;

                foreach (var pair in navLinkPairs)
                {
                    // List of all links.
                    EditorGUILayout.BeginHorizontal();

                    GUIContent guiContent = EditorGUIUtility.ObjectContent(pair.Key, typeof(TileBase));

                    if (GUILayout.Button(guiContent, tileStyle))
                        EditorGUIUtility.PingObject(pair.Key);

                    GUILayout.Label("-", labelStyle);

                    guiContent = EditorGUIUtility.ObjectContent(pair.Value, typeof(NavLink));

                    if (GUILayout.Button(guiContent, linkStyle))
                        EditorGUIUtility.PingObject(pair.Value);

                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                // No tiles registered yet.
                GUIStyle centerLabelStyle = new GUIStyle("Label");
                centerLabelStyle.alignment = TextAnchor.MiddleCenter;

                GUILayout.Label("No NavLinks registered yet.\nMake sure they are located in the Resources folder.", centerLabelStyle);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Refresh Links"))
            {
                _duplicateLinks = NavTileManager.Instance.LinkManager.RefreshAllNavLinks();
            }

            GUILayout.EndHorizontal();
        }
    }
}
