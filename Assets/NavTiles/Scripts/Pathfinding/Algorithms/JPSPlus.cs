using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JumpPointDirection = Snowcap.NavTiles.AdditionalJPSPlusData.JumpPointDirection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Jump Point Search+ implementation to work with the NavTile Pipeline.
    /// </summary>
    public class JPSPlus : PathfindingAlgorithmBase
    {
        #region Find Path

        // The openset of nodes which still need to be checked for a valid path.
        NavNodeHeap openSet;
        // The closed set with nodes that have been checked already.
        HashSet<Vector2Int> closedSet;

        /// <summary>
        /// Interface method to find a path using the JPS+ algorithm.
        /// </summary>
        /// <param name="inInput">Path input parameters to be used when finding a path.</param>
        /// <returns>A NavTilePath instance consisting of a list of nodes that represent a path.</returns>
        public override NavTilePath FindPath(FindPathInput inInput)
        {
            openSet = new NavNodeHeap();
            closedSet = new HashSet<Vector2Int>();

            // Create the first node to start the pathfinding.
            PathfindingNode startNode = GetNode(inInput.StartCoordinate);
            Vector2Int targetPosition = inInput.TargetCoordinate;

            // The startnode is a special case, so check all directions.
            for (int i = 0; i <= 3; i++)
            {
                JumpPointDirection direction = (JumpPointDirection)(1 << i);

                SearchInDirection(direction, startNode, inInput);
            }

            // Main loop. While there are still nodes which can be checked, continue checking them.
            while (openSet.Count > 0)
            {
                // Grab the node with the lowest cost of the heap and add it to the closedset.
                PathfindingNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode.Coordinate);

                // Check whether the node is the targetnode, which means the pathfinding can be stopped.
                if (currentNode.Coordinate == targetPosition)
                {
                    // Path found
                    List<Vector2Int> pathInCoordinates = RetracePath(startNode, currentNode);

#if UNITY_EDITOR
                    NavTilePathVisualizer.SetDebugVisualization(inInput.Visualizer, closedSet.ToList(), openSet.GetCoordinateList(), pathInCoordinates);
#endif

                    return ConvertCoordinatesToPath(pathInCoordinates, inInput.AreaMask);    
                }

                // Get the direction of how this node was reached.
                JumpPointDirection parentDirection = (currentNode.AdditionalTileData as AdditionalJPSPlusData).ParentDirection;

                // Find the perpendicular directions based on the parent direction.
                JumpPointDirection firstPerpendicularDirection;
                JumpPointDirection secondPerpendicularDirection;

                GetPerpendicularDirections(parentDirection, out firstPerpendicularDirection, out secondPerpendicularDirection);

                // Search in all three directions for relevant nodes. 
                SearchInDirection(parentDirection, currentNode, inInput);
                SearchInDirection(firstPerpendicularDirection, currentNode, inInput);
                SearchInDirection(secondPerpendicularDirection, currentNode, inInput);
            }

            Debug.Log("No path found from " + inInput.StartCoordinate + " to " + inInput.TargetCoordinate);
            return null;
        }

        /// <summary>
        /// Search nodes in a particular direction.
        /// </summary>
        private void SearchInDirection(JumpPointDirection inDirection, PathfindingNode inNode, FindPathInput inInput)
        {
            Vector2Int coordinate = inNode.Coordinate;

            Vector2Int goalPos = inInput.TargetCoordinate;

            // Get the jump distance and the absolute value of the jump distance.
            int jumpDistance = (inNode.AdditionalTileData as AdditionalJPSPlusData).GetJumpDistance(inDirection);
            int absJumpDistance = Mathf.Abs(jumpDistance);

            // Check whether the target node is in line with the current node.
            int diff;
            if (InLineWithTargetPosition(coordinate, goalPos, absJumpDistance, inDirection, out diff))
            {
                // Calculate cost of next node and add to open list.
                int cost = inNode.GCost + GetAdditionalCost(diff);

                Vector2Int nextNodeCoordinate = Vector2Int.zero;
                switch (inDirection)
                {
                    case JumpPointDirection.North:
                    case JumpPointDirection.South:
                        nextNodeCoordinate = new Vector2Int(coordinate.x, goalPos.y);
                        break;
                    case JumpPointDirection.East:
                    case JumpPointDirection.West:
                        nextNodeCoordinate = new Vector2Int(goalPos.x, coordinate.y);
                        break;
                }

                AddToOpenList(nextNodeCoordinate, inNode, JumpPointDirection.North, cost, inInput);
                return;
            }

            // Otherwise, check if the jump distance is positive to jump to a jump point.
            if (jumpDistance > 0)
            {
                int cost = inNode.GCost + GetAdditionalCost(diff);
                AddToOpenList(GetNextPosition(coordinate, absJumpDistance, inDirection), inNode, inDirection, cost, inInput);
            }
        }

        /// <summary>
        /// Helper function to check whether the targetposition is on one line with the given coordinate.
        /// </summary>
        /// <param name="inCoordinate">The coordinate being checked.</param>
        /// <param name="inTargetPosition">The target coordinate.</param>
        /// <param name="inAbsJumpDistance">The absolute value of the jump distance.</param>
        /// <param name="inDirection">The direction being looked at.</param>
        /// <param name="outDiff">The difference between the two coordinates.</param>
        /// <returns>True if in line of sight, otherwise false.</returns>
        private bool InLineWithTargetPosition(Vector2Int inCoordinate, Vector2Int inTargetPosition, int inAbsJumpDistance, JumpPointDirection inDirection, out int outDiff)
        {
            switch (inDirection)
            {
                case JumpPointDirection.North:
                    outDiff = Mathf.Abs(inCoordinate.y - inTargetPosition.y);
                    return inCoordinate.y < inTargetPosition.y && inCoordinate.y + inAbsJumpDistance >= inTargetPosition.y;
                case JumpPointDirection.East:
                    outDiff = Mathf.Abs(inTargetPosition.x - inCoordinate.x);
                    return inCoordinate.x < inTargetPosition.x && inCoordinate.x + inAbsJumpDistance >= inTargetPosition.x;
                case JumpPointDirection.South:
                    outDiff = Mathf.Abs(inTargetPosition.y - inCoordinate.y);
                    return inCoordinate.y > inTargetPosition.y && inCoordinate.y - inAbsJumpDistance <= inTargetPosition.y;
                case JumpPointDirection.West:
                    outDiff = Mathf.Abs(inCoordinate.x - inTargetPosition.x);
                    return inCoordinate.x > inTargetPosition.x && inCoordinate.x - inAbsJumpDistance <= inTargetPosition.x;
                default:
                    outDiff = 0;
                    return false;
            }
        }

        /// <summary>
        /// Helper function to get the next position based on the direction and the original coordinate.
        /// </summary>
        /// <param name="inCoordinate">Coordinate being looked at.</param>
        /// <param name="inAbsJumpDistance">Absolute value of the jump distance.</param>
        /// <param name="inDirection">The direction being looked at.</param>
        /// <returns>The position based on the current coordinate and the jumpdistance in the given direction.</returns>
        private Vector2Int GetNextPosition(Vector2Int inCoordinate, int inAbsJumpDistance, JumpPointDirection inDirection)
        {
            switch (inDirection)
            {
                case JumpPointDirection.North:
                    return new Vector2Int(inCoordinate.x, inCoordinate.y + inAbsJumpDistance);
                case JumpPointDirection.East:
                    return new Vector2Int(inCoordinate.x + inAbsJumpDistance, inCoordinate.y);
                case JumpPointDirection.South:
                    return new Vector2Int(inCoordinate.x, inCoordinate.y - inAbsJumpDistance);
                case JumpPointDirection.West:
                    return new Vector2Int(inCoordinate.x - inAbsJumpDistance, inCoordinate.y);
                default:
                    return Vector2Int.zero;
            }
        }

        /// <summary>
        /// Adds the given coordinate to the open list, or updates a node already on there with new information if necessary.
        /// </summary>
        /// <param name="inNewCoordinate">The coordinate being put on the open list.</param>
        /// <param name="inCurrentNode">The node the new coordinate led from.</param>
        /// <param name="inDirection">Direction being looked at.</param>
        /// <param name="inCost">Cost of the new node.</param>
        /// <param name="inInput">Input parameters for finding a path.</param>
        private void AddToOpenList(Vector2Int inNewCoordinate, PathfindingNode inCurrentNode, JumpPointDirection inDirection, int inCost, FindPathInput inInput)
        {
            PathfindingNode node;

            if ((node = openSet.GetExistingNode(inNewCoordinate)) != null)
            {
                node.ParentNode = inCurrentNode;
                (node.AdditionalTileData as AdditionalJPSPlusData).ParentDirection = inDirection;
                node.GCost = inCost;

                openSet.UpdateItem(node);
            }
            else if (!closedSet.Contains(inNewCoordinate))
            {
                int heuristicCost = GetManhattanDistance(inNewCoordinate, inInput.TargetCoordinate);

                node = GetNode(inNewCoordinate);

                node.ParentNode = inCurrentNode;
                (node.AdditionalTileData as AdditionalJPSPlusData).ParentDirection = inDirection;
                node.GCost = inCost;
                node.HCost = heuristicCost;

                openSet.Add(node);
            }
        }

        /// <summary>
        /// Returns the additional cost based on the given distance.
        /// </summary>
        private int GetAdditionalCost(int inDistance)
        {
            return inDistance * STRAIGHT_MOVEMENT_COST;
        }

#endregion
        #region Baking

        /// <summary>
        /// Interface method to generate additional tile data for the baked grid.
        /// </summary>
        public void GenerateAdditionalTileData(NavTileSurfaceData inData)
        {
            DisplayProgressBar(0f);

            foreach (var tile in inData.Tiles)
            {
                tile.Value.AdditionalData = new AdditionalJPSPlusData();

                IdentifyJumpPoint(tile, inData);
            }

            // Do vertical first.
            DisplayProgressBar(1f / 5f);
            ProcessGridWestwards(inData);
            DisplayProgressBar(2f / 5f);
            ProcessGridEastwards(inData);

            // Horizontal next so the vertical data is available.
            DisplayProgressBar(3f / 5f);
            ProcessGridNorthwards(inData);
            DisplayProgressBar(4f / 5f);
            ProcessGridSouthwards(inData);

#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
        }

        /// <summary>
        /// Displays a progress bar in the editor.
        /// </summary>
        private void DisplayProgressBar(float inProgress)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayProgressBar("Baking Additional JPS+ Data", "Baking Additional JPS+ Data", inProgress);
#endif
        }

        /// <summary>
        /// Checks the grid and marks any jump point.
        /// </summary>
        private void IdentifyJumpPoint(KeyValuePair<Vector2Int, TileData> inTile, NavTileSurfaceData inData)
        {
            JumpPointDirection jumpPointFlag = JumpPointDirection.None;

            if (inTile.Value == null || !inTile.Value.IsWalkable(1))
                return;

            // Hard coded 0 to 3 as there are only 4 cardinal directions.
            for (int i = 0; i <= 3; i++)
            {
                JumpPointDirection parentDirection = (JumpPointDirection)(1 << i);
                Vector2Int parentPosition = GetNextPositionInDirection(inTile.Key, parentDirection);

                TileData parentTile = inData.GetTileData(parentPosition);

                if (parentTile == null || !parentTile.IsWalkable(1))
                    continue;

                JumpPointDirection firstPerpendicularDirection;
                JumpPointDirection secondPerpendicularDirection;

                GetPerpendicularDirections(parentDirection, out firstPerpendicularDirection, out secondPerpendicularDirection);

                if ((HasAdjacentTraverseNode(inTile.Key, firstPerpendicularDirection, inData) && !HasAdjacentTraverseNode(parentPosition, firstPerpendicularDirection, inData))
                    || HasAdjacentTraverseNode(inTile.Key, secondPerpendicularDirection, inData) && !HasAdjacentTraverseNode(parentPosition, secondPerpendicularDirection, inData))
                    {
                        jumpPointFlag |= GetOppositeDirection(parentDirection);
                    }
            }

            (inTile.Value.AdditionalData as AdditionalJPSPlusData).JumpPointDirections = jumpPointFlag;
        }

        /// <summary>
        /// returns a position in the given direction.
        /// </summary>
        private Vector2Int GetNextPositionInDirection(Vector2Int inOrigin, JumpPointDirection inDirection)
        {
            switch (inDirection)
            {
                case JumpPointDirection.North:
                    return new Vector2Int(inOrigin.x, inOrigin.y + 1);
                case JumpPointDirection.East:
                    return new Vector2Int(inOrigin.x + 1, inOrigin.y);
                case JumpPointDirection.South:
                    return new Vector2Int(inOrigin.x, inOrigin.y - 1);
                case JumpPointDirection.West:
                    return new Vector2Int(inOrigin.x - 1, inOrigin.y);
                default:
                    return inOrigin;
            }
        }

        /// <summary>
        /// Assigns the perpendicular directions of the given direction as out variables.
        /// </summary>
        private void GetPerpendicularDirections(JumpPointDirection inDirection, out JumpPointDirection outFirstPerpendicularDirection, out JumpPointDirection outSecondPerpendicularDirection)
        {
            switch (inDirection)
            {
                case JumpPointDirection.North:
                case JumpPointDirection.South:
                    outFirstPerpendicularDirection = JumpPointDirection.East;
                    outSecondPerpendicularDirection = JumpPointDirection.West;
                    break;
                case JumpPointDirection.East:
                case JumpPointDirection.West:
                    outFirstPerpendicularDirection = JumpPointDirection.North;
                    outSecondPerpendicularDirection = JumpPointDirection.South;
                    break;
                default:
                    outFirstPerpendicularDirection = JumpPointDirection.None;
                    outSecondPerpendicularDirection = JumpPointDirection.None;
                    break;
            }
        }

        /// <summary>
        /// Checks whether the given position has a traversable node in the given direction next to it.
        /// </summary>
        private bool HasAdjacentTraverseNode(Vector2Int inPosition, JumpPointDirection inDirection, NavTileSurfaceData inData)
        {
            Vector2Int adjacentCoordinate = GetNextPositionInDirection(inPosition, inDirection);

            return inData.IsTileWalkable(adjacentCoordinate, 1);
        }

        /// <summary>
        /// Returns the opposite direction based on the given one.
        /// </summary>
        private JumpPointDirection GetOppositeDirection(JumpPointDirection inDirection)
        {
            switch (inDirection)
            {
                case JumpPointDirection.North:
                    return JumpPointDirection.South;
                case JumpPointDirection.East:
                    return JumpPointDirection.West;
                case JumpPointDirection.South:
                    return JumpPointDirection.North;
                case JumpPointDirection.West:
                    return JumpPointDirection.East;
                default:
                    return JumpPointDirection.None;
            }
        }

        /// <summary>
        /// Processes the grid northwards and assigns all distances per tile in the northward direction.
        /// </summary>
        private void ProcessGridNorthwards(NavTileSurfaceData inData)
        {
            int currentDistance = 0;

            RectInt gridBounds = inData.GridBounds.AsRect();

            Vector2Int coordinate = new Vector2Int();

            for (coordinate.x = gridBounds.xMin; coordinate.x <= gridBounds.xMax; coordinate.x++)
            {
                for (coordinate.y = gridBounds.yMax; coordinate.y >= gridBounds.yMin; coordinate.y--)
                {
                    TileData tile = inData.GetTileData(coordinate);

                    if (tile == null)
                        continue;

                    AdditionalJPSPlusData addedData = tile.AdditionalData;

                    if (coordinate.y == gridBounds.yMax
                        || !inData.IsTileWalkable(coordinate + new Vector2Int(0, 1), 1))
                    {
                        addedData.SetJumpDistance(JumpPointDirection.North, 0);
                    }
                    else if (inData.IsTileWalkable(coordinate + new Vector2Int(0, 1), 1)
                            && (inData.GetTileData(coordinate + new Vector2Int(0, 1))?.AdditionalData.GetJumpDistance(JumpPointDirection.West) > 0 
                            || inData.GetTileData(coordinate + new Vector2Int(0, 1))?.AdditionalData.GetJumpDistance(JumpPointDirection.East) > 0))
                    {
                        addedData.SetJumpDistance(JumpPointDirection.North, 1);
                    }
                    else
                    {
                        currentDistance = inData.GetTileData(coordinate + new Vector2Int(0, 1)).AdditionalData.GetJumpDistance(JumpPointDirection.North);

                        if (currentDistance > 0)
                        {
                            addedData.SetJumpDistance(JumpPointDirection.North, currentDistance + 1);
                        }
                        else
                        {
                            addedData.SetJumpDistance(JumpPointDirection.North, currentDistance - 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Processes the grid eastwards and assigns all distances per tile in the eastward direction.
        /// </summary>
        private void ProcessGridEastwards(NavTileSurfaceData inData)
        {
            int currentDistance = 0;
            TileData relevantTile = null;

            RectInt gridBounds = inData.GridBounds.AsRect();

            Vector2Int coordinate = new Vector2Int();

            for (coordinate.x = gridBounds.xMax, coordinate.y = gridBounds.yMin; coordinate.x >= gridBounds.xMin && coordinate.y <= gridBounds.yMax; coordinate.y++)
            {
                for (; coordinate.x >= gridBounds.xMin; coordinate.x--)
                {
                    TileData currentTile = inData.GetTileData(coordinate);

                    SetDistance(JumpPointDirection.East, currentTile, ref relevantTile, ref currentDistance);
                }

                coordinate.x = gridBounds.xMax;
                relevantTile = null;
                currentDistance = 0;
            }
        }

        /// <summary>
        /// Processes the grid southwards and assigns all distances per tile in the southward direction.
        /// </summary>
        private void ProcessGridSouthwards(NavTileSurfaceData inData)
        {
            int currentDistance = 0;

            RectInt gridBounds = inData.GridBounds.AsRect();

            Vector2Int coordinate = new Vector2Int();

            for (coordinate.x = gridBounds.xMin; coordinate.x <= gridBounds.xMax; coordinate.x++)
            {
                for (coordinate.y = gridBounds.yMin; coordinate.y <= gridBounds.yMax; coordinate.y++)
                {
                    TileData tile = inData.GetTileData(coordinate);

                    if (tile == null)
                        continue;

                    AdditionalJPSPlusData addedData = tile.AdditionalData;

                    if (coordinate.y == gridBounds.yMin
                        || !inData.IsTileWalkable(coordinate + new Vector2Int(0, -1), 1))
                    {
                        addedData.SetJumpDistance(JumpPointDirection.South, 0);
                    }
                    else if (inData.IsTileWalkable(coordinate + new Vector2Int(0, -1), 1)
                            && (inData.GetTileData(coordinate + new Vector2Int(0, -1))?.AdditionalData.GetJumpDistance(JumpPointDirection.West) > 0 
                            || inData.GetTileData(coordinate + new Vector2Int(0, -1))?.AdditionalData.GetJumpDistance(JumpPointDirection.East) > 0))
                    {
                        addedData.SetJumpDistance(JumpPointDirection.South, 1);
                    }
                    else
                    {
                        currentDistance = inData.GetTileData(coordinate + new Vector2Int(0, -1)).AdditionalData.GetJumpDistance(JumpPointDirection.South);

                        if (currentDistance > 0)
                        {
                            addedData.SetJumpDistance(JumpPointDirection.South, currentDistance + 1);
                        }
                        else
                        {
                            addedData.SetJumpDistance(JumpPointDirection.South, currentDistance - 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Processes the grid westwards and assigns all distances per tile in the westward direction.
        /// </summary>
        private void ProcessGridWestwards(NavTileSurfaceData inData)
        {
            int currentDistance = 0;
            TileData relevantTile = null;

            RectInt gridBounds = inData.GridBounds.AsRect();

            Vector2Int coordinate = new Vector2Int();

            for (coordinate.x = gridBounds.xMin, coordinate.y = gridBounds.yMin; coordinate.x < gridBounds.xMax && coordinate.y < gridBounds.yMax; coordinate.y++)
            {
                for (; coordinate.x < gridBounds.xMax; coordinate.x++)
                {
                    TileData currentTile = inData.GetTileData(coordinate);

                    SetDistance(JumpPointDirection.West, currentTile, ref relevantTile, ref currentDistance);
                }

                coordinate.x = gridBounds.xMin;
                relevantTile = null;
                currentDistance = 0;
            }
        }

        /// <summary>
        /// Sets the distance on a tile based on the situation that tile is in.
        /// </summary>
        private void SetDistance(JumpPointDirection inDirection, TileData inCurrentTile, ref TileData refRelevantTile, ref int refCurrentDistance)
        {
            if (inCurrentTile == null)
            {
                return;
            }
            else if (!inCurrentTile.IsWalkable(1)) // Is the tile NOT walkable?
            {
                // Set the tile as the relevant one and reset the jump distance.
                refRelevantTile = inCurrentTile;
                refCurrentDistance = 0;
            }
            else if ((((inCurrentTile.AdditionalData as AdditionalJPSPlusData).JumpPointDirections) & inDirection) != 0) // Is the current tile a jump point in the given direction?
            {
                // If the relevant tile is not walkable or non-existent, set the distance to be negative.
                if (refRelevantTile == null || !refRelevantTile.IsWalkable(1))
                {
                    refCurrentDistance *= -1;
                }
                else // Otherwise do a +1.
                {
                    refCurrentDistance++;
                }

                // assign the jump distance.
                (inCurrentTile.AdditionalData as AdditionalJPSPlusData).SetJumpDistance(inDirection, refCurrentDistance);

                // Reset it to be counted from the current tile.
                refRelevantTile = inCurrentTile;
                refCurrentDistance = 0;

            }
            else // No special tile, keep counting and assign intermediate value to the jump distance in the relevant direction.
            {
                if (refRelevantTile == null || !refRelevantTile.IsWalkable(1))
                {
                    (inCurrentTile.AdditionalData as AdditionalJPSPlusData).SetJumpDistance(inDirection, refCurrentDistance * -1);
                    refCurrentDistance++;
                    return;
                }
                else
                {
                    (inCurrentTile.AdditionalData as AdditionalJPSPlusData).SetJumpDistance(inDirection, ++refCurrentDistance);
                }
            }
        }

        /// <summary>
        /// Interface method to get the name of this algorithm. To be displayed in the Pipeline tab of the NavTiles Settings Window.
        /// </summary>
        /// <returns></returns>
        public override string GetName()
        {
            return "Jump Point Search+";
        }

        /// <summary>
        /// Interface method to get the bakestate that belongs to this algorithm.
        /// </summary>
        /// <returns></returns>
        public override NavTileSurfaceData.BakeState GetRequiredBakeState()
        {
            return NavTileSurfaceData.BakeState.AdditionalData;
        }
        #endregion
    }
}
