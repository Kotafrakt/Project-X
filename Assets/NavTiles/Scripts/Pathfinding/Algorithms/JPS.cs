using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Direction = HexagonGridHelper.Direction;

namespace Snowcap.NavTiles
{
    public class JPS : PathfindingAlgorithmBase
    {
        public override NavTilePath FindPath(FindPathInput inInput)
        {
            NavNodeHeap openSet = new NavNodeHeap();
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            PathfindingNode startNode = GetNode(inInput.StartCoordinate);
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathfindingNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode.Coordinate);

                if (currentNode.Coordinate == inInput.TargetCoordinate)
                {
                    // Path found.
                    List<Vector2Int> pathInCoordinates = RetracePath(startNode, currentNode);

#if UNITY_EDITOR
                    NavTilePathVisualizer.SetDebugVisualization(inInput.Visualizer, closedSet.ToList(), openSet.GetCoordinateList(), pathInCoordinates);
#endif

                    return ConvertCoordinatesToPath(pathInCoordinates, inInput.AreaMask);
                }

                IdentifySuccessors(currentNode, openSet, closedSet, inInput);
            }

            // No path found
            return null;
        }

        private void IdentifySuccessors(PathfindingNode inNode, NavNodeHeap inOpenSet, HashSet<Vector2Int> inClosedSet, FindPathInput inInput)
        {
            List<Vector2Int> neighbours = FindNeighbours(inNode, inInput);

            foreach (Vector2Int neighbourCoord in neighbours)
            {
                Vector2Int? jumpCoordinateNullable = Jump(neighbourCoord, inNode.Coordinate, inInput);

                if (jumpCoordinateNullable == null || inClosedSet.Contains(jumpCoordinateNullable.GetValueOrDefault()))
                    continue;

                PathfindingNode examinedNode = inOpenSet.GetExistingNode(jumpCoordinateNullable.Value) ?? new PathfindingNode(jumpCoordinateNullable.Value);
                int newMovementCost = inNode.GCost + (inInput.DiagonalAllowed ? GetDiagonalDistance(examinedNode.Coordinate, inNode.Coordinate) :
                                                                                GetManhattanDistance(examinedNode.Coordinate, inNode.Coordinate));

                if (examinedNode.GCost == 0 || newMovementCost < examinedNode.GCost)
                {
                    examinedNode.GCost = newMovementCost;
                    examinedNode.HCost = GetHeuristicCost(examinedNode.Coordinate, inInput.TargetCoordinate, inInput.DiagonalAllowed);
                    examinedNode.ParentNode = inNode;

                    if (inOpenSet.Contains(examinedNode))
                        inOpenSet.UpdateItem(examinedNode);
                    else
                        inOpenSet.Add(examinedNode);
                }
            }
        }

        private Vector2Int? Jump(Vector2Int inNeighbourCoordinate, Vector2Int inParentCoordinate, FindPathInput inInput)
        {
            if (!IsTileWalkable(inNeighbourCoordinate, inInput.AreaMask))
                return null;

            if (inNeighbourCoordinate == inInput.TargetCoordinate)
                return inNeighbourCoordinate;

            if (NavTileManager.Instance.SurfaceManager.GridInfo.CellLayout == GridLayout.CellLayout.Hexagon)
            {
                // Identify forced neighbours.
                Direction movingDirection = HexagonGridHelper.GetDirection(inParentCoordinate, inNeighbourCoordinate);
                Vector2Int aboveFarCoord = HexagonGridHelper.GetNeighbourInDirection(inNeighbourCoordinate, HexagonGridHelper.GetRelativeDirection(movingDirection, 2));
                Vector2Int aboveCloseCoord = HexagonGridHelper.GetNeighbourInDirection(inNeighbourCoordinate, HexagonGridHelper.GetRelativeDirection(movingDirection, 1));

                if (!IsTileWalkable(aboveFarCoord, inInput.AreaMask) && IsTileWalkable(aboveCloseCoord, inInput.AreaMask))
                    return inNeighbourCoordinate;

                Vector2Int belowFarCoord = HexagonGridHelper.GetNeighbourInDirection(inNeighbourCoordinate, HexagonGridHelper.GetRelativeDirection(movingDirection, -2));
                Vector2Int belowCloseCoord = HexagonGridHelper.GetNeighbourInDirection(inNeighbourCoordinate, HexagonGridHelper.GetRelativeDirection(movingDirection, -1));

                if (!IsTileWalkable(belowFarCoord, inInput.AreaMask) && IsTileWalkable(belowCloseCoord, inInput.AreaMask))
                    return inNeighbourCoordinate;

                // No forced neighbours, jump further.
                if ((int)movingDirection % 2 == 0)
                {
                    // Even directions have to check adjacent directions.
                    if (Jump(aboveCloseCoord, inNeighbourCoordinate, inInput) != null ||
                        Jump(belowCloseCoord, inNeighbourCoordinate, inInput) != null)
                        return inNeighbourCoordinate;
                }

                return Jump(HexagonGridHelper.GetNeighbourInDirection(inNeighbourCoordinate, movingDirection), inNeighbourCoordinate, inInput);
            }

            Vector2Int horizontalDelta = new Vector2Int(inNeighbourCoordinate.x - inParentCoordinate.x, 0);
            Vector2Int verticalDelta = new Vector2Int(0, inNeighbourCoordinate.y - inParentCoordinate.y);

            if (inInput.DiagonalAllowed && horizontalDelta.x != 0 && verticalDelta.y != 0)
            {
                // Diagonal movement.
                if (inInput.CutCorners)
                {
                    if (!IsTileWalkable(inNeighbourCoordinate - horizontalDelta, inInput.AreaMask) && IsTileWalkable(inNeighbourCoordinate - horizontalDelta + verticalDelta, inInput.AreaMask) ||
                        !IsTileWalkable(inNeighbourCoordinate - verticalDelta, inInput.AreaMask) && IsTileWalkable(inNeighbourCoordinate + horizontalDelta - verticalDelta, inInput.AreaMask))
                        return inNeighbourCoordinate;
                }

                // Straight jumps.
                if (Jump(inNeighbourCoordinate + horizontalDelta, inNeighbourCoordinate, inInput) != null ||
                    Jump(inNeighbourCoordinate + verticalDelta, inNeighbourCoordinate, inInput) != null)
                    return inNeighbourCoordinate;
            }
            else
            {
                // Straight movement.
                if (horizontalDelta.x != 0 && CheckStraightJumpMovement(inNeighbourCoordinate, horizontalDelta, inInput))
                    return inNeighbourCoordinate;
                else if (verticalDelta.y != 0)
                {
                    if (CheckStraightJumpMovement(inNeighbourCoordinate, verticalDelta, inInput))
                        return inNeighbourCoordinate;

                    if (!inInput.DiagonalAllowed)
                    {
                        // Jump horizontally when moving vertically during non-diagonal search.
                        if (Jump(inNeighbourCoordinate + Vector2Int.right, inNeighbourCoordinate, inInput) != null ||
                            Jump(inNeighbourCoordinate + Vector2Int.left, inNeighbourCoordinate, inInput) != null)
                            return inNeighbourCoordinate;
                    }
                }
            }

            if (inInput.DiagonalAllowed && inInput.CutCorners)
            {
                if (IsTileWalkable(inNeighbourCoordinate + horizontalDelta, inInput.AreaMask) && IsTileWalkable(inNeighbourCoordinate + verticalDelta, inInput.AreaMask))
                    return Jump(inNeighbourCoordinate + horizontalDelta + verticalDelta, inNeighbourCoordinate, inInput);
                else
                    return null;
            }
            else
            {
                return Jump(inNeighbourCoordinate + horizontalDelta + verticalDelta, inNeighbourCoordinate, inInput);
            }
        }

        private bool CheckStraightJumpMovement(Vector2Int inNeighbourCoordinate, Vector2Int inDelta, FindPathInput inInput)
        {
            Vector2Int swizzledDelta = new Vector2Int(inDelta.y, inDelta.x);

            if (inInput.DiagonalAllowed && inInput.CutCorners)
                return !IsTileWalkable(inNeighbourCoordinate + swizzledDelta, inInput.AreaMask) && IsTileWalkable(inNeighbourCoordinate + inDelta + swizzledDelta, inInput.AreaMask) ||
                       !IsTileWalkable(inNeighbourCoordinate - swizzledDelta, inInput.AreaMask) && IsTileWalkable(inNeighbourCoordinate + inDelta - swizzledDelta, inInput.AreaMask);

            return !IsTileWalkable(inNeighbourCoordinate + swizzledDelta - inDelta, inInput.AreaMask) && IsTileWalkable(inNeighbourCoordinate + swizzledDelta, inInput.AreaMask) ||
                   !IsTileWalkable(inNeighbourCoordinate - swizzledDelta - inDelta, inInput.AreaMask) && IsTileWalkable(inNeighbourCoordinate - swizzledDelta, inInput.AreaMask);
        }

        private List<Vector2Int> FindNeighbours(PathfindingNode inNode, FindPathInput inInput)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();

            PathfindingNode parentNode = inNode.ParentNode;

            if (parentNode != null)
            {
                if (NavTileManager.Instance.SurfaceManager.GridInfo.CellLayout == GridLayout.CellLayout.Hexagon)
                {
                    AddHexagonalNeighbours(neighbours, inNode.Coordinate, parentNode.Coordinate, inInput);
                    return neighbours;
                }

                Vector2Int difference = inNode.Coordinate - parentNode.Coordinate;

                Vector2Int horizontalDelta = new Vector2Int(difference.x / Mathf.Max(Mathf.Abs(difference.x), 1), 0);
                Vector2Int verticalDelta = new Vector2Int(0, difference.y / Mathf.Max(Mathf.Abs(difference.y), 1));

                if (inInput.DiagonalAllowed && horizontalDelta.x != 0 && verticalDelta.y != 0)
                {
                    // Diagonal movement.
                    if (IsTileWalkable(inNode.Coordinate + verticalDelta, inInput.AreaMask))
                        neighbours.Add(inNode.Coordinate + verticalDelta);
                    if (IsTileWalkable(inNode.Coordinate + horizontalDelta, inInput.AreaMask))
                        neighbours.Add(inNode.Coordinate + horizontalDelta);

                    if (inInput.CutCorners)
                    {
                        if (IsTileWalkable(inNode.Coordinate + verticalDelta + horizontalDelta, inInput.AreaMask))
                            neighbours.Add(inNode.Coordinate + verticalDelta + horizontalDelta);
                        if (!IsTileWalkable(inNode.Coordinate - horizontalDelta, inInput.AreaMask))
                            neighbours.Add(inNode.Coordinate - horizontalDelta + verticalDelta);
                        if (!IsTileWalkable(inNode.Coordinate - verticalDelta, inInput.AreaMask))
                            neighbours.Add(inNode.Coordinate + horizontalDelta - verticalDelta);
                    }
                    else
                    {
                        if (IsTileWalkable(inNode.Coordinate + verticalDelta, inInput.AreaMask) &&
                            IsTileWalkable(inNode.Coordinate + horizontalDelta, inInput.AreaMask))
                            neighbours.Add(inNode.Coordinate + verticalDelta + horizontalDelta);
                    }
                }
                else
                {
                    // Straight movement.
                    if (horizontalDelta.x != 0)
                        AddStraightNeighbours(neighbours, inNode, horizontalDelta, inInput);
                    else
                        AddStraightNeighbours(neighbours, inNode, verticalDelta, inInput);
                }
            }
            else
            {
                // This is the starting node, get all neighbours.
                neighbours = GetWalkableNeighbours(inNode, inInput.AreaMask, inInput.DiagonalAllowed, inInput.CutCorners);
            }

            return neighbours;
        }
        
        private void AddHexagonalNeighbours(List<Vector2Int> inNeighbours, Vector2Int inCurrentCoord, Vector2Int inParentCoord, FindPathInput inInput)
        {
            Direction movingDirection = HexagonGridHelper.GetDirection(inParentCoord, inCurrentCoord);

            Vector2Int nextCoord = HexagonGridHelper.GetNeighbourInDirection(inCurrentCoord, movingDirection);
            if (IsTileWalkable(nextCoord, inInput.AreaMask))
                inNeighbours.Add(nextCoord);

            Vector2Int aboveCoord = HexagonGridHelper.GetNeighbourInDirection(inCurrentCoord, HexagonGridHelper.GetRelativeDirection(movingDirection, 1));
            if (IsTileWalkable(aboveCoord, inInput.AreaMask))
                inNeighbours.Add(aboveCoord);

            Vector2Int belowCoord = HexagonGridHelper.GetNeighbourInDirection(inCurrentCoord, HexagonGridHelper.GetRelativeDirection(movingDirection, -1));
            if (IsTileWalkable(belowCoord, inInput.AreaMask))
                inNeighbours.Add(belowCoord);
        }

        private void AddStraightNeighbours(List<Vector2Int> inNeighbours, PathfindingNode inNode, Vector2Int inDelta, FindPathInput inInput)
        {
            Vector2Int swizzledDelta = new Vector2Int(inDelta.y, inDelta.x);

            bool nextWalkable = IsTileWalkable(inNode.Coordinate + inDelta, inInput.AreaMask);
            bool topWalkable = IsTileWalkable(inNode.Coordinate + swizzledDelta, inInput.AreaMask);
            bool bottomWalkable = IsTileWalkable(inNode.Coordinate - swizzledDelta, inInput.AreaMask);

            if (inInput.DiagonalAllowed && inInput.CutCorners)
            {
                if (nextWalkable)
                    inNeighbours.Add(inNode.Coordinate + inDelta);
                if (!topWalkable)
                    inNeighbours.Add(inNode.Coordinate + inDelta + swizzledDelta);
                if (!bottomWalkable)
                    inNeighbours.Add(inNode.Coordinate + inDelta - swizzledDelta);
            }
            else
            {
                if (nextWalkable)
                {
                    inNeighbours.Add(inNode.Coordinate + inDelta);

                    if (inInput.DiagonalAllowed)
                    {
                        if (topWalkable)
                            inNeighbours.Add(inNode.Coordinate + inDelta + swizzledDelta);
                        if (bottomWalkable)
                            inNeighbours.Add(inNode.Coordinate + inDelta - swizzledDelta);
                    }
                }

                if (topWalkable)
                    inNeighbours.Add(inNode.Coordinate + swizzledDelta);
                if (bottomWalkable)
                    inNeighbours.Add(inNode.Coordinate - swizzledDelta);
            }
        }

        public override string GetName()
        {
            return "Jump Point Search";
        }

        public override NavTileSurfaceData.BakeState GetRequiredBakeState()
        {
            return NavTileSurfaceData.BakeState.Standard;
        }
    }
}
