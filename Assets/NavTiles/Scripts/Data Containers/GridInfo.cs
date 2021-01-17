using Snowcap.Extensions;
using UnityEngine;
using static UnityEngine.GridLayout;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// This class is used to store the info of the grid separate from Unity so it can be thread-safe.
    /// </summary>
    public class GridInfo
    {
        public Matrix4x4 GridLocalToWorldMatrix { get; private set; }
        public Vector3 CellSize { get; private set; }
        public Vector3 CellGap { get; private set; }
        public CellLayout CellLayout { get; private set; }

        private object _lockRef = new object();

        public GridInfo(GridLayout inGridLayout)
        {
            InitializeGridInfo(inGridLayout);
        }

        /// <summary>
        /// Initializes the gridinfo using the data of the provided grid object.
        /// </summary>
        /// <param name="inGridLayout">Grid object to copy data of.</param>
        public void InitializeGridInfo(GridLayout inGridLayout)
        {
            if (inGridLayout == null)
                return;

            lock (_lockRef)
            {
                Transform gridLayoutTransform = inGridLayout.transform;
                GridLocalToWorldMatrix = gridLayoutTransform.localToWorldMatrix;
                CellSize = inGridLayout.cellSize;
                CellGap = inGridLayout.cellGap;
                CellLayout = inGridLayout.cellLayout;
            }
        }

        /// <summary>
        /// Manually calculate cell center. This allows for multithreading.
        /// This should do the same as Grid.GetCellCenter.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to get center for.</param>
        /// <returns>3D position of the center of the cell.</returns>
        public Vector3 GetCenterOfCell(Vector3Int inCoordinate)
        {
            Vector3 center = new Vector3();

            switch (CellLayout)
            {
                case CellLayout.Rectangle:
                    center.x = inCoordinate.x * CellSize.x + CellSize.x / 2;
                    center.y = inCoordinate.y * CellSize.y + CellSize.y / 2;
                    break;
                case CellLayout.Hexagon:
                    center.x = inCoordinate.x * CellSize.x;
                    center.x += Mathf.Abs(inCoordinate.y) % 2 == 1 ? CellSize.x / 2 : 0;
                    center.y = inCoordinate.y * CellSize.y * 0.75f;
                    break;
                case CellLayout.Isometric:
                case CellLayout.IsometricZAsY:
                    center.x = (inCoordinate.x - inCoordinate.y) * CellSize.x / 2;
                    center.y = (1 + inCoordinate.y + inCoordinate.x) * CellSize.y / 2;
                    break;
                default:
                    Debug.LogError($"The grid layout {CellLayout.ToString()} is not supported.");
                    break;
            }

            return GridLocalToWorldMatrix.MultiplyPoint3x4(center);
        }
        
        /// <summary>
        /// Manually calculate cell center. This allows for multithreading.
        /// This should do the same as Grid.GetCellCenter but the local version.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to get center for.</param>
        /// <returns>2D position of the center of the cell.</returns>
        public Vector2 GetLocalCenterOfCell(Vector2Int inCoordinate)
        {
            Vector2 center = new Vector2();

            switch (CellLayout)
            {
                case CellLayout.Rectangle:
                    center.x = inCoordinate.x * CellSize.x + CellSize.x / 2;
                    center.y = inCoordinate.y * CellSize.y + CellSize.y / 2;
                    break;
                case CellLayout.Hexagon:
                    center.x = inCoordinate.x * CellSize.x;
                    center.x += Mathf.Abs(inCoordinate.y) % 2 == 1 ? CellSize.x / 2 : 0;
                    center.y = inCoordinate.y * CellSize.y * 0.75f;
                    break;
                case CellLayout.Isometric:
                case CellLayout.IsometricZAsY:
                    center.x = (inCoordinate.x - inCoordinate.y) * CellSize.x / 2;
                    center.y = (1 + inCoordinate.y + inCoordinate.x) * CellSize.y / 2;
                    break;
                default:
                    Debug.LogError($"The grid layout {CellLayout.ToString()} is not supported.");
                    break;
            }

            return center;
        }

        /// <summary>
        /// Converts the local grid position to a world position using the grid's matrix.
        /// </summary>
        /// <param name="localPosition">Local position in the grid.</param>
        /// <returns>World position.</returns>
        public Vector3 ConvertToWorldPosition(Vector2 localPosition)
        {
            return GridLocalToWorldMatrix.MultiplyPoint3x4(localPosition);
        }

        /// <summary>
        /// Manually calculate cell center. This allows for multithreading.
        /// This should do the same as Grid.GetCellCenter.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to get center for.</param>
        /// <returns>3D position of the center of the cell.</returns>
        public Vector3 GetCenterOfCell(Vector2Int inCoordinate) { return GetCenterOfCell(inCoordinate.GetVector3Int()); }
    }
}
