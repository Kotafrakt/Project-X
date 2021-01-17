using System.Collections.Generic;
using Snowcap.EditorPackage;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Custom NavTileBrush class to change Area type of NavTiles or NavLinks
    /// </summary>
    [CustomGridBrush(true, false, false, "NavTile Brush")]
    [CreateAssetMenu(fileName = "New NavTile Brush", menuName = "Brushes/NavTile Brush")]
    public class NavTileBrush : GridBrush { }

    /// <summary>
    /// Custom Editor associated with NavTileBrush.
    /// </summary>
    [CustomEditor(typeof(NavTileBrush))]
    public class NavTileBrushEditor : GridBrushEditor
    {
        /// <summary>
        /// The Area Type Brush Object.
        /// </summary>
        public NavTileBrush AreaTypeBrush { get { return target as NavTileBrush; } }

        // Serialized Objects / Properties for the selected NavTiles and NavLinks.
        private SerializedObject _serializedNavTileSelection;
        private SerializedProperty _serializedNavTileAreaIndex;
        
        private SerializedObject _serializedNavLinkSelection;
        private SerializedProperty _serializedNavLinkAreaIndex;

        /// <summary>
        /// Overridden OnEnable. Calls base class.
        /// </summary>
        protected override void OnEnable() 
        {
            base.OnEnable();

            // Subscribe to event to see when the grid has changed.
            GridSelection.gridSelectionChanged += RefreshSerializedSelection;

            RefreshSerializedSelection();
        }

        /// <summary>
        /// Overridden OnDisable. Calls base class.
        /// </summary>
        protected override void OnDisable() 
        {
            base.OnDisable();

            // unsubscribe to event to see when the grid has changed.
            GridSelection.gridSelectionChanged -= RefreshSerializedSelection;    
        }

        /// <summary>
        /// Elements drawn in the inspector view of the brush.
        /// </summary>
        public override void OnPaintInspectorGUI()
        {
            DrawAreaIndexPopUp();
        }

        /// <summary>
        /// Elements drawn in the inspector of the Grid Selection.
        /// </summary>
        public override void OnSelectionInspectorGUI()
        {
            DrawAreaIndexPopUp();
        }

        /// <summary>
        /// Displays popups for the selected NavTiles and NavLinks. 
        /// </summary>
        private void DrawAreaIndexPopUp()
        {
            _serializedNavTileSelection?.Update();
            _serializedNavLinkSelection?.Update();

            if (_serializedNavTileAreaIndex != null)
            {
                EditorGUILayout.LabelField("Selected NavTiles", EditorStyles.boldLabel);
                EditorHelper.DrawCompressedPopup(_serializedNavTileAreaIndex, NavTileManager.Instance.AreaManager.AllAreaNames);
                
                EditorGUILayout.Space();
            }

            if (_serializedNavLinkAreaIndex != null)
            {
                EditorGUILayout.LabelField("Selected NavLinks", EditorStyles.boldLabel);
                EditorHelper.DrawCompressedPopup(_serializedNavLinkAreaIndex, NavTileManager.Instance.AreaManager.AllAreaNames);

                EditorGUILayout.Space();
            }

            _serializedNavTileSelection?.ApplyModifiedProperties();
            _serializedNavLinkSelection?.ApplyModifiedProperties();
        }

        /// <summary>
        /// Retrieves the selected Tiles.
        /// </summary>
        private TileBase[] GetSelectedTiles()
        {
            if (GridPaintingState.scenePaintTarget == GridSelection.target)
            {
                return GridPaintingState.scenePaintTarget.GetComponent<Tilemap>().GetTilesBlock(GridSelection.position);
            }
            else
            {
                return GridPaintingState.palette?.GetComponentInChildren<Tilemap>().GetTilesBlock(GridSelection.position);
            }
        }

        /// <summary>
        /// Filters the selected Tiles and outputs a list of NavTiles and NavLinks in case selected.
        /// </summary>
        private void GetSelectedNavTiles(out List<NavTile> outNavTiles, out List<NavLink> outNavLinks)
        {
            outNavTiles = new List<NavTile>();
            outNavLinks = new List<NavLink>();

            TileBase[] tiles = GetSelectedTiles();

            if (tiles == null)
                return;

            foreach (TileBase tile in tiles)
            {
                if (tile is NavTile)
                {
                    outNavTiles.Add(tile as NavTile);
                }
                else
                {
                    NavLink link = null;

                    if ((link = NavTileManager.Instance.LinkManager.GetLinkedTile(tile)) != null)
                    {
                        outNavLinks.Add(link);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the serialized objects and properties.
        /// </summary>
        private void RefreshSerializedSelection()
        {
            List<NavTile> navTiles;
            List<NavLink> navLinks;

            GetSelectedNavTiles(out navTiles, out navLinks);

            if (navTiles.Count == 0)
            {
                _serializedNavTileSelection = null;
                _serializedNavTileAreaIndex = null;
            }
            else
            {
                _serializedNavTileSelection = new SerializedObject(navTiles.ToArray());
                _serializedNavTileAreaIndex = _serializedNavTileSelection?.FindProperty("AreaIndex");
            }

            if (navLinks.Count == 0)
            {
                _serializedNavLinkSelection = null;
                _serializedNavLinkAreaIndex = null;
            }
            else
            {
                _serializedNavLinkSelection = new SerializedObject(navLinks.ToArray());
                _serializedNavLinkAreaIndex = _serializedNavLinkSelection?.FindProperty("AreaIndex");
            }
        }
    }
}
