using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snowcap.NavTiles
{
    public class AStar : PathfindingAlgorithmBase
    {
        /// <summary>
        /// Calculate a path using the A* algorithm and the specified input.
        /// </summary>
        /// <param name="inInput">Input used to calculate path with.</param>
        public override NavTilePath FindPath(FindPathInput inInput)
        {
            NavNodeHeap openSet = new NavNodeHeap();
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            PathfindingNode startNode = GetNode(inInput.StartCoordinate);
            openSet.Add(startNode);
            Vector2Int targetPosition = inInput.TargetCoordinate;

            while (openSet.Count > 0)
            {
                PathfindingNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode.Coordinate);

                if (currentNode.Coordinate == targetPosition)
                {
                    // Path found.
                    List<Vector2Int> pathInCoordinates = RetracePath(startNode, currentNode);

#if UNITY_EDITOR
                    NavTilePathVisualizer.SetDebugVisualization(inInput.Visualizer, closedSet.ToList(), openSet.GetCoordinateList(), pathInCoordinates);
#endif

                    return ConvertCoordinatesToPath(pathInCoordinates, inInput.AreaMask);
                }

                foreach (Vector2Int neighbourCoordinate in GetWalkableNeighbours(currentNode, inInput.AreaMask, inInput.DiagonalAllowed, inInput.CutCorners))
                {
                    if (inInput.PositionsToAvoid.Contains(neighbourCoordinate))
                    {                        
                        continue;
                    }

                    if (closedSet.Contains(neighbourCoordinate))
                        continue;

                    // Get node from the open set if it exists, otherwise, create one.
                    PathfindingNode neighbourNode = openSet.GetExistingNode(neighbourCoordinate) ?? GetNode(neighbourCoordinate);

                    int movementPenalty = inInput.IgnoreTileCost ? 0 : neighbourNode.TileCost;
                    int newMovementCostToNeighbour = currentNode.GCost + GetAdjacentCost(neighbourCoordinate - currentNode.Coordinate) + movementPenalty;
                    if (neighbourNode.GCost == 0 || newMovementCostToNeighbour < neighbourNode.GCost)
                    {
                        neighbourNode.GCost = newMovementCostToNeighbour;
                        neighbourNode.HCost = GetHeuristicCost(neighbourCoordinate, inInput.TargetCoordinate, inInput.DiagonalAllowed);
                        neighbourNode.ParentNode = currentNode;

                        if (openSet.Contains(neighbourNode))
                            openSet.UpdateItem(neighbourNode);
                        else
                            openSet.Add(neighbourNode);
                    }
                }
            }

            // Path not found.
            return null;
        }

        /// <summary>
        /// Gets the movement cost of an adjacent node.
        /// </summary>
        /// <param name="inDifferenceVector">Vector has to be difference between adjacent nodes.</param>
        /// <returns>Cost to the adjacent node.</returns>
        private int GetAdjacentCost(Vector2Int inDifferenceVector)
        {
            if (NavTileManager.Instance.SurfaceManager.GridInfo.CellLayout == GridLayout.CellLayout.Hexagon)
                return STRAIGHT_MOVEMENT_COST;

            return inDifferenceVector.x == 0 || inDifferenceVector.y == 0 ? STRAIGHT_MOVEMENT_COST : DIAGONAL_MOVEMENT_COST;
        }

        public override string GetName()
        {
            return "A*";
        }

        public override NavTileSurfaceData.BakeState GetRequiredBakeState()
        {
            return NavTileSurfaceData.BakeState.Standard;
        }
    }
}
