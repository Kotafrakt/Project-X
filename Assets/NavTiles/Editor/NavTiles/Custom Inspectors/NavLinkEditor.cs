using Snowcap.EditorPackage;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Custom editor for NavLinks in order to display areas correctly.
    /// </summary>
    [CustomEditor(typeof(NavLink))]
    [CanEditMultipleObjects]
    public class NavLinkEditor : Editor
    {
        /// <summary>
        /// The NavLink being inspected.
        /// </summary>
        private NavLink Tile
        {
            get
            {
                return (NavLink)target;
            }
        }

        /// <summary>
        /// The NavTile banner displayed at the top of the editor.
        /// </summary>
        private static ResizingBanner _navTileBanner = new ResizingBanner(NavTileGUIDs.NAV_TILE_BANNER);

        /// <summary>
        /// The property holding the reference to the TileBase asset used to link.
        /// </summary>
        private SerializedProperty _linkedTileProperty;

        /// <summary>
        /// The area index property holding the index of the area linked to this tile.
        /// </summary>
        private SerializedProperty _areaIndexProperty;

        private void OnEnable()
        {
            _linkedTileProperty = serializedObject.FindProperty(nameof(NavLink.LinkedTile));
            _areaIndexProperty = serializedObject.FindProperty(nameof(NavLink.AreaIndex));
        }

        public override void OnInspectorGUI()
        {
            _navTileBanner.Draw();

            serializedObject.Update();

            // Multi editing not supported as it would not make sense, since every NavLink should be unique.
            using (new EditorGUI.DisabledGroupScope(targets.Length > 1))
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.PropertyField(_linkedTileProperty);

                if (EditorGUI.EndChangeCheck())
                {
                    // Record the NavLink dictionary for undo.
                    Undo.RecordObject(NavTileManager.Instance, Undo.GetCurrentGroupName());
                    TileBase newTile = (TileBase)_linkedTileProperty.objectReferenceValue;

                    // Callback for the tile change.
                    if (_linkedTileProperty.objectReferenceValue != Tile.LinkedTile)
                        Tile.OnTileChanged(Tile.LinkedTile, newTile);
                }
            }

            // Area index.
            EditorHelper.DrawCompressedPopup(_areaIndexProperty, NavTileManager.Instance.AreaManager.AllAreaNames);

            // Buttons.
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("View Links"))
            {
                NavTileWindow.OpenLinksTab();
            }

            if (GUILayout.Button("Edit Areas"))
            {
                NavTileWindow.OpenAreasTab();
            }
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
