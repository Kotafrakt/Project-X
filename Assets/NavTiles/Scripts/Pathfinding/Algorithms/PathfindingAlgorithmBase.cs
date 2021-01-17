using System.Collections.Generic;
using UnityEngine;

namespace Snowcap.NavTiles
{
    public abstract class PathfindingAlgorithmBase : IPathfindingAlgorithm
    {
        public const int STRAIGHT_MOVEMENT_COST = 10;
        public const int DIAGONAL_MOVEMENT_COST = 14;
        public const int STRAIGHT_HEURISTIC_COST = 10;
        public const int DIAGONAL_HEURISTIC_COST = 14;

        public abstract string GetName();
        public abstract NavTileSurfaceData.BakeState GetRequiredBakeState();
        public abstract NavTilePath FindPath(FindPathInput inInput);

        /// <summary>
        /// Calculate the heuristic value from one tile to the other.
        /// Uses the appropriate distance function based on settings.
        /// </summary>
        protected int GetHeuristicCost(Vector2Int inCoordinateA, Vector2Int inCoordinateB, bool inIsDiagonalAllowed)
        {
            if (NavTileManager.Instance.SurfaceManager.GridInfo.CellLayout == GridLayout.CellLayout.Hexagon)
                return GetHexagonalDistance(inCoordinateA, inCoordinateB);

            return inIsDiagonalAllowed ? GetDiagonalDistance(inCoordinateA, inCoordinateB) :
                                         GetManhattanDistance(inCoordinateA, inCoordinateB);
        }

        /// <summary>
        /// Calculate the heuristic value from one tile to the other.
        /// Using the manhattan distance.
        /// </summary>
        protected int GetManhattanDistance(Vector2Int inCoordinateA, Vector2Int inCoordinateB)
        {
            return (Mathf.Abs(inCoordinateA.x - inCoordinateB.x) +
                    Mathf.Abs(inCoordinateA.y - inCoordinateB.y)) * STRAIGHT_HEURISTIC_COST;
        }

        /// <summary>
        /// Calculate the heuristic value from one tile to the other.
        /// Using the diagonal manhattan distance.
        /// </summary>
        protected int GetDiagonalDistance(Vector2Int inCoordinateA, Vector2Int inCoordinateB)
        {
            int dstX = Mathf.Abs(inCoordinateA.x - inCoordinateB.x);
            int dstY = Mathf.Abs(inCoordinateA.y - inCoordinateB.y);

            return dstX > dstY ?
                   DIAGONAL_HEURISTIC_COST * dstY + STRAIGHT_HEURISTIC_COST * (dstX - dstY) :
                   DIAGONAL_HEURISTIC_COST * dstX + STRAIGHT_HEURISTIC_COST * (dstY - dstX);
        }

        /// <summary>
        /// Calculate the heuristic value from one tile to the other.
        /// Using the hexagonal distance calculation.
        /// </summary>
        protected int GetHexagonalDistance(Vector2Int inCoordinateA, Vector2Int inCoordinateB)
        {
            return HexagonGridHelper.GetDistance(inCoordinateA, inCoordinateB) * STRAIGHT_HEURISTIC_COST;
        }

        /// <summary>
        /// Retrace the path from the end node to the start node, by tracing the parents.
        /// </summary>
        protected List<Vector2Int> RetracePath(PathfindingNode inStartNode, PathfindingNode inEndNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            PathfindingNode currentNode = inEndNode;

            // Exclude starting node.
            while (currentNode != inStartNode)
            {
                path.Add(currentNode.Coordinate);
                currentNode = currentNode.ParentNode;
            }

            return path;
        }

        /// <summary>
        /// Converts a list of coordinates to a path with world positions.
        /// </summary>
        /// <param name="inPathInCoordinates">List of 2D coordinates on the grid.</param>
        /// <param name="inAreaMask">Area mask to associate with the path. Currently used for smoothing.</param>
        /// <returns>The path with world positions.</returns>
        protected NavTilePath ConvertCoordinatesToPath(List<Vector2Int> inPathInCoordinates, int inAreaMask)
        {
            NavTilePath path = new NavTilePath();

            for (int i = inPathInCoordinates.Count - 1; i >= 0; i--)
            {
                path.Add(inPathInCoordinates[i]);
            }

            path.AreaMask = inAreaMask;

            return path;
        }

        /// <summary>
        /// Create a new node with coordinate.
        /// </summary>
        /// <param name="inCoordinate">Coordinate to create node for.</param>
        /// <returns>A pathfinding node for the specified coordinate.</returns>
        protected PathfindingNode GetNode(Vector2Int inCoordinate)
        {
            PathfindingNode navNode = new PathfindingNode(inCoordinate);
            try
            {
                TileData tileData = NavTileManager.Instance.SurfaceManager.Data.GetTileData(inCoordinate);
                navNode.TileCost = tileData.Area.Cost;
                navNode.AdditionalTileData = tileData.AdditionalData;
            }
            catch
            {
                throw new System.Exception("No PathfindingNode found at specified grid coordinate.");
            }
            return navNode;
        }

        /// <summary>
        /// Get all the walkable neighbours of a node based on grid and input.
        /// </summary>
        /// <param name="inNavNode">Node to get the neighbours for.</param>
        /// <param name="inAreaMask">Area mask to check tiles for.</param>
        /// <param name="inDiagonalAllowed">Whether or not to include diagonal tiles.</param>
        /// <param name="inCutCorners">whether or not to include diagonal tiles adjacent to non-walkables.</param>
        /// <returns>Walkable neighbours of specified node.</returns>
        protected List<Vector2Int> GetWalkableNeighbours(PathfindingNode inNavNode, int inAreaMask, bool inDiagonalAllowed, bool inCutCorners)
        {
            if (NavTileManager.Instance.SurfaceManager.GridInfo.CellLayout == GridLayout.CellLayout.Hexagon)
                return GetHexagonalNeighbours(inNavNode, inAreaMask);

            if (inDiagonalAllowed && inCutCorners)
                return GetDiagonalNeighbours(inNavNode, inAreaMask);

            // None of the above chosen so calculate nodes for non-diagonal and not cutting corners.
            List<Vector2Int> neighboursCoordinates = new List<Vector2Int>();
            Vector2Int coordinate;
            int straightMask = 0;

            // Get all the directly adjacent nodes.
            for (int i = 0; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    coordinate = i == 0 ? new Vector2Int(inNavNode.Coordinate.x + j, inNavNode.Coordinate.y) :
                                          new Vector2Int(inNavNode.Coordinate.x, inNavNode.Coordinate.y + j);

                    if (IsTileWalkable(coordinate, inAreaMask))
                    {
                        straightMask |= 1 << (i + j + 1);
                        neighboursCoordinates.Add(coordinate);
                    }
                }
            }

            if (!inDiagonalAllowed)
                return neighboursCoordinates;

            // Get diagonals where their adjacent cells are also walkable.
            for (int y = -1; y <= 1; y += 2)
            {
                for (int x = -1; x <= 1; x += 2)
                {
                    if ((straightMask & 1 << (x + 1)) == 0 ||
                        (straightMask & 1 << (y + 2)) == 0)
                        continue;

                    coordinate = new Vector2Int(inNavNode.Coordinate.x + x, inNavNode.Coordinate.y + y);
                    if (IsTileWalkable(coordinate, inAreaMask))
                        neighboursCoordinates.Add(coordinate);
                }
            }

            return neighboursCoordinates;
        }

        /// <summary>
        /// Gets all the walkable neighbours for a hexagonal grid.
        /// </summary>
        /// <param name="inNavNode">Node to get the neighbours for.</param>
        /// <param name="inAreaMask">Area mask to check tiles for.</param>
        /// <returns>Walkable neighbours of specified node.</returns>
        protected List<Vector2Int> GetHexagonalNeighbours(PathfindingNode inNavNode, int inAreaMask)
        {
            List<Vector2Int> neighboursCoordinates = new List<Vector2Int>();
            Vector2Int coordinate;

            // Getting neighbours is a bit weird due to the coordinate system of hexagon tiles.
            // Flip selection of neighbours based on an uneven y coordinate.
            int flipped = Mathf.Abs(inNavNode.Coordinate.y) % 2 == 1 ? -1 : 1;
            for (int x = -1; x <= 0; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    coordinate = new Vector2Int(inNavNode.Coordinate.x + x * flipped, inNavNode.Coordinate.y + y);
                    if (IsTileWalkable(coordinate, inAreaMask))
                        neighboursCoordinates.Add(coordinate);
                }
            }

            coordinate = new Vector2Int(inNavNode.Coordinate.x + 1 * flipped, inNavNode.Coordinate.y);
            if (IsTileWalkable(coordinate, inAreaMask))
                neighboursCoordinates.Add(coordinate);

            return neighboursCoordinates;
        }

        /// <summary>
        /// Gets all the walkable neighbours including all diagonals.
        /// </summary>
        /// <param name="inNavNode">Node to get the neighbours for.</param>
        /// <param name="inAreaMask">Area mask to check tiles for.</param>
        /// <returns>Walkable neighbours of specified node.</returns>
        protected List<Vector2Int> GetDiagonalNeighbours(PathfindingNode inNavNode, int inAreaMask)
        {
            List<Vector2Int> neighboursCoordinates = new List<Vector2Int>();
            Vector2Int coordinate;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    coordinate = new Vector2Int(inNavNode.Coordinate.x + x, inNavNode.Coordinate.y + y);
                    if (IsTileWalkable(coordinate, inAreaMask))
                        neighboursCoordinates.Add(coordinate);
                }
            }

            return neighboursCoordinates;
        }

        protected bool IsTileWalkable(Vector2Int inCoordinate, int inAreaMask)
        {
            return NavTileManager.Instance.SurfaceManager.Data.IsTileWalkable(inCoordinate, inAreaMask);
        }
    }
}
