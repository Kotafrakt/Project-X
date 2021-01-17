using Snowcap.Extensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Draws handles and gizmos for the NavTiles based on selected debugging option.
    /// </summary>
    public static class NavTileDebugVisualizer
    {
        /// <summary>
        /// Possible options for debugging.
        /// </summary>
        public enum DisplayOptions
        {
            None,
            Areas
        }

        // Editor prefs keys.
        private const string KEY_PREFIX = nameof(NavTileDebugVisualizer) + "_";
        private const string DISPLAYING_OPTIONS_KEY = KEY_PREFIX + nameof(DisplayingOptions);
        private const string AREA_GIZMO_SIZE_KEY = KEY_PREFIX + nameof(AreaGizmoSize);
        private const string AREA_GIZMO_ALPHA_KEY = KEY_PREFIX + nameof(AreaGizmoAlpha);

        /// <summary>
        /// Option for debug drawing.
        /// </summary>
        public static DisplayOptions DisplayingOptions = DisplayOptions.Areas;

        /// <summary>
        /// Size of handles drawn in percentage.
        /// </summary>
        public static float AreaGizmoSize = 0.4f;

        /// <summary>
        /// Alpha values of the drawn handles.
        /// </summary>
        public static float AreaGizmoAlpha = 1f;

        /// <summary>
        /// Maximum amount of gizmos/handles drawn, used for performance reasons.
        /// </summary>
        private const int MAX_TILE_GIZMOS = 40000;

        /// <summary>
        /// Surface to draw gizmos/handles for.
        /// </summary>
        private static NavTileSurfaceManager _surface;

        /// <summary>
        /// Keeps track of whether editor prefs variables are initialized or not.
        /// </summary>
        private static bool _isInitialized = false;

        /// <summary>
        /// Loads properties from editor prefs if not already initialized.
        /// </summary>
        public static void LoadDebugVisualizerEditorPrefs()
        {
            if (_isInitialized)
                return;

            if (EditorPrefs.HasKey(DISPLAYING_OPTIONS_KEY))
                DisplayingOptions = (DisplayOptions)EditorPrefs.GetInt(DISPLAYING_OPTIONS_KEY);
            if (EditorPrefs.HasKey(AREA_GIZMO_SIZE_KEY))
                AreaGizmoSize = EditorPrefs.GetFloat(AREA_GIZMO_SIZE_KEY);
            if (EditorPrefs.HasKey(AREA_GIZMO_ALPHA_KEY))
                AreaGizmoAlpha = EditorPrefs.GetFloat(AREA_GIZMO_ALPHA_KEY);

            _isInitialized = true;
        }

        /// <summary>
        /// Saves current properties to editor prefs.
        /// </summary>
        public static void SaveDebugVisualizerEditorPrefs()
        {
            EditorPrefs.SetInt(DISPLAYING_OPTIONS_KEY, (int)DisplayingOptions);
            EditorPrefs.SetFloat(AREA_GIZMO_SIZE_KEY, AreaGizmoSize);
            EditorPrefs.SetFloat(AREA_GIZMO_ALPHA_KEY, AreaGizmoAlpha);
        }

        /// <summary>
        /// Draws handles in the scene view based on the selected display options.
        /// </summary>
        public static void DrawSceneViewHandles(NavTileSurfaceManager inSurfaceToDraw)
        {
            LoadDebugVisualizerEditorPrefs();

            if (!inSurfaceToDraw.IsDataInitialized || inSurfaceToDraw.Grid == null)
                return;

            if (DisplayingOptions == DisplayOptions.None)
                return;

            _surface = inSurfaceToDraw;

            // if (SceneView.currentDrawingSceneView.in2DMode && _surface.Grid.cellLayout == GridLayout.CellLayout.Rectangle || _surface.Grid.cellLayout == GridLayout.CellLayout.Hexagon)
            // {
            //     Ray cornerRay = Camera.current.ViewportPointToRay(Vector3.zero);
            //     Plane navPlane = new Plane(-_surface.Grid.transform.forward, 0);
            //
            //     Vector2Int bottomLeft = new Vector2Int();
            //     Vector2Int upperRight = new Vector2Int();
            //
            //     float dist;
            //     if (navPlane.Raycast(cornerRay, out dist))
            //         bottomLeft = _surface.Grid.WorldToCell(cornerRay.origin + cornerRay.direction * dist - _surface.Grid.cellSize).GetVector2Int();
            //
            //     bottomLeft.Clamp(_surface.Data.GridBounds.BottomLeft, _surface.Data.GridBounds.TopRight);
            //
            //     cornerRay = Camera.current.ViewportPointToRay(new Vector3(1, 1, 0));
            //
            //     if (navPlane.Raycast(cornerRay, out dist))
            //         upperRight = _surface.Grid.WorldToCell(cornerRay.origin + cornerRay.direction * dist + _surface.Grid.cellSize).GetVector2Int();
            //
            //     upperRight.Clamp(_surface.Data.GridBounds.BottomLeft, _surface.Data.GridBounds.TopRight);
            //
            //     if (Mathf.Abs(upperRight.x - bottomLeft.x) * Mathf.Abs(upperRight.y - bottomLeft.y) > MAX_TILE_GIZMOS)
            //         return;
            //
            //     TileData tileData = null;
            //
            //     for (int y = bottomLeft.y; y <= upperRight.y; y++)
            //     {
            //         for (int x = bottomLeft.x; x <= upperRight.x; x++)
            //         {
            //             Vector2Int coordinate = new Vector2Int(x, y);
            //             if (_surface.Data.Tiles.TryGetValue(coordinate, out tileData))
            //             {
            //                 Vector2 localPosition = _surface.GridInfo.GetLocalCenterOfCell(coordinate);
            //                 Vector3 worldPosition = _surface.GridInfo.ConvertToWorldPosition(localPosition);
            //                 DrawTileGizmoBasedOnOption(coordinate, localPosition, worldPosition, tileData);
            //             }
            //         }
            //     }
            // }
            // else
            // {
            foreach (KeyValuePair<Vector2Int, TileData> pair in _surface.Data.Tiles)
            {
                Vector2 localPosition = _surface.GridInfo.GetLocalCenterOfCell(pair.Key);
                Vector3 worldPosition = _surface.GridInfo.ConvertToWorldPosition(localPosition);

                if (Camera.current != null)
                {
                    Vector3 viewPortPos = Camera.current.WorldToViewportPoint(worldPosition);
                    if (viewPortPos.x < 0 || viewPortPos.x > 1 || viewPortPos.y < 0 || viewPortPos.y > 1 || viewPortPos.z < 0)
                        continue;

                    DrawTileGizmoBasedOnOption(pair.Key, localPosition, worldPosition, pair.Value);
                }
            }
            // }
        }

        /// <summary>
        /// Passes the drawing of a tile to the correct function based on the selected debug option.
        /// </summary>
        private static void DrawTileGizmoBasedOnOption(Vector2Int inCoordinate, Vector2 inLocalPosition, Vector3 inWorldPosition, TileData inTileData)
        {
            switch (DisplayingOptions)
            {
                case DisplayOptions.None:
                    break;
                case DisplayOptions.Areas:
                    DrawNavTileAreas(inCoordinate, inLocalPosition, inWorldPosition, inTileData);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Draws a rectangle for each tile with their corresponding color.
        /// </summary>
        /// <param name="inData">Data to draw areas from.</param>
        private static void DrawNavTileAreas(Vector2Int inCoordinate, Vector2 inLocalPosition, Vector3 inWorldPosition, TileData inTileData)
        {
            Vector2 size = _surface.Grid.cellSize * AreaGizmoSize;

            Color c = inTileData.Area.Color;
            c.a = AreaGizmoAlpha;
            Handles.color = c;

            Handles.DrawAAConvexPolygon(TileGizmoShapeCalculator.GetGridShapeVertices(inLocalPosition, size, _surface.GridInfo));
        }
    }
}
