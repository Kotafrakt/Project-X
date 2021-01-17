using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Snowcap.NavTiles.MazeGeneration
{
    /// <summary>
    /// This is used to generate a random procedural maze using the recursive backtracker algorithm:
    /// https://en.wikipedia.org/wiki/Maze_generation_algorithm#Recursive_backtracker
    /// 
    /// The maze is used as an example for NavTile but can be used for other applications.
    /// </summary>
    public class Maze : MonoBehaviour
    {
        /// <summary>
        /// The size of the maze in maze cells.
        /// This means the amount of intersections since these are used to generate the maze.
        /// </summary>
        public Vector2Int MazeSize = new Vector2Int(10, 10);

        /// <summary>
        /// The tilemap to place tiles on.
        /// </summary>
        public Tilemap Tilemap;

        /// <summary>
        /// The tile that will be placed on a walkable area.
        /// </summary>
        public TileBase WalkableTile;

        /// <summary>
        /// The tile that will be placed on a non-walkable area.
        /// </summary>
        public TileBase WallTile;

        /// <summary>
        /// Should the tilemap be cleared at the start of the maze generation.
        /// </summary>
        public bool ClearOnGenerate = true;

        /// <summary>
        /// Should the NavTiles be baked after the generation of the maze.
        /// </summary>
        public bool BakeOnGenerate = true;

        /// <summary>
        /// Optional agent to place at the start of the maze and assign the end of the maze as a target.
        /// </summary>
        public NavTileAgent agentToSet;

        /// <summary>
        /// All cells of the maze. These are NOT separate tiles but are intersections within the maze.
        /// These get translated into tiles after the generation algorithm.
        /// </summary>
        private MazeCell[,] _cells;

        /// <summary>
        /// Stack used for the recursive back-tracking algorithm to keep track of the tiles that still need checking.
        /// </summary>
        private Stack<MazeCell> _cellsToCheck;

        /// <summary>
        /// Generates a maze with the current settings.
        /// This will change the assigned tilemap.
        /// </summary>
        public void GenerateMaze()
        {
#if UNITY_EDITOR
            Undo.RecordObject(Tilemap, "Maze Generation");
#endif

            if (ClearOnGenerate)
                Tilemap.ClearAllTiles();

            SetupCells();

            MazeCell currentCell = _cells[0, 0];
            currentCell.IsVisited = true;

            while (true)
            {
                List<MazeCell> unvisitedNeighbours = GetUnvisitedNeighbours(currentCell);

                if (unvisitedNeighbours.Count > 0)
                {
                    // No dead end, proceed the carve.
                    MazeCell randomNeighbour = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];

                    // More directions to check later, add to stack.
                    if (unvisitedNeighbours.Count > 1)
                        _cellsToCheck.Push(currentCell);

                    // Go to next random cell
                    RemoveWalls(currentCell, randomNeighbour);

                    currentCell = randomNeighbour;
                    currentCell.IsVisited = true;

                    unvisitedNeighbours = null;
                }
                else if (_cellsToCheck.Count > 0)
                {
                    currentCell = _cellsToCheck.Pop();
                }
                else
                {
                    break;
                }
            }

            CreateMazeOnTilemap(this.Tilemap);
        }

        /// <summary>
        /// Apply the generated maze data to a tilemap.
        /// </summary>
        /// <param name="tilemap">Tilemap to change.</param>
        private void CreateMazeOnTilemap(Tilemap tilemap)
        {
            Vector3Int tileAreaSize = new Vector3Int(MazeSize.x * 2 + 1, MazeSize.y * 2 + 1, 1);
            BoundsInt mazeArea = new BoundsInt(new Vector3Int(-tileAreaSize.x / 2, -tileAreaSize.y / 2, 0), tileAreaSize);
            TileBase[] tiles = new TileBase[tileAreaSize.x * tileAreaSize.y];

            // Corners.
            for (int y = 0; y < tileAreaSize.y; y++)
            {
                for (int x = 0; x < tileAreaSize.x; x++)
                {
                    if (x % 2 == 0 && y % 2 == 0)
                        tiles[y * tileAreaSize.x + x] = WallTile;
                    else
                        tiles[y * tileAreaSize.x + x] = WalkableTile;
                }
            }

            // All walkways from maze generation.
            for (int y = 0; y < MazeSize.y; y++)
            {
                for (int x = 0; x < MazeSize.x; x++)
                {
                    MazeCell cell = _cells[x, y];
                    int tileXPos = (1 + x * 2);
                    int tileYPos = (1 + y * 2);
                    int tilePos = tileYPos * tileAreaSize.x + tileXPos;

                    if ((cell.HasWalls & WallsDirections.Up) != 0)
                        tiles[tilePos + tileAreaSize.x] = WallTile;

                    if ((cell.HasWalls & WallsDirections.Down) != 0)
                        tiles[tilePos - tileAreaSize.x] = WallTile;

                    if ((cell.HasWalls & WallsDirections.Left) != 0)
                        tiles[tilePos - 1] = WallTile;

                    if ((cell.HasWalls & WallsDirections.Right) != 0)
                        tiles[tilePos + 1] = WallTile;
                }
            }

            // Start and end.
            tiles[1 + tileAreaSize.x * (tileAreaSize.y - 1)] = WalkableTile;
            tiles[tileAreaSize.x - 2] = WalkableTile;

            tilemap.SetTilesBlock(mazeArea, tiles);

            if (agentToSet != null)
            {
#if UNITY_EDITOR
                Undo.RecordObjects(new Object[] { agentToSet, agentToSet.transform }, "Maze Generation");
#endif

                agentToSet.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(mazeArea.x + 1, mazeArea.yMax - 1, 0));
                agentToSet.TargetPos = new Vector2Int(mazeArea.xMax - 2, mazeArea.yMin);
            }

            if (BakeOnGenerate)
            {
                NavTileManager.Instance.SurfaceManager.Bake(NavTileManager.Instance.PipelineManager.Algorithm);
            }
        }

        /// <summary>
        /// Setup the array of cells with their corresponding coordinates.
        /// </summary>
        private void SetupCells()
        {
            _cells = new MazeCell[MazeSize.x, MazeSize.y];

            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                for (int x = 0; x < _cells.GetLength(0); x++)
                {
                    _cells[x, y] = new MazeCell(new Vector2Int(x, y));
                }
            }

            _cellsToCheck = new Stack<MazeCell>();
        }

        /// <summary>
        /// Get a list of all adjacent neighbours that are unvisited.
        /// </summary>
        /// <param name="centerCell">Cell to get the neighbours for.</param>
        /// <returns>A list of all unvisited neighbours.</returns>
        private List<MazeCell> GetUnvisitedNeighbours(MazeCell centerCell)
        {
            List<MazeCell> neighbours = new List<MazeCell>();
            Vector2Int centerPos = new Vector2Int(centerCell.Position.x, centerCell.Position.y);
            MazeCell currentCheckingCell = null;

            if (centerPos.y + 1 < MazeSize.y && !(currentCheckingCell = _cells[centerPos.x, centerPos.y + 1]).IsVisited)
                neighbours.Add(currentCheckingCell);

            if (centerPos.x + 1 < MazeSize.x && !(currentCheckingCell = _cells[centerPos.x + 1, centerPos.y]).IsVisited)
                neighbours.Add(currentCheckingCell);

            if (centerPos.y - 1 >= 0 && !(currentCheckingCell = _cells[centerPos.x, centerPos.y - 1]).IsVisited)
                neighbours.Add(currentCheckingCell);

            if (centerPos.x - 1 >= 0 && !(currentCheckingCell = _cells[centerPos.x - 1, centerPos.y]).IsVisited)
                neighbours.Add(currentCheckingCell);

            return neighbours;
        }

        // Cells have to be adjacent.
        /// <summary>
        /// Removes the walls between two cells.
        /// The two cells have to be adjacent to make it work properly.
        /// </summary>
        private void RemoveWalls(MazeCell firstCell, MazeCell secondCell)
        {
            Vector2Int direction = secondCell.Position - firstCell.Position;

            if (direction.x != 0)
            {
                if (direction.x == 1)
                {
                    firstCell.HasWalls ^= WallsDirections.Right;
                    secondCell.HasWalls ^= WallsDirections.Left;
                }
                else
                {
                    secondCell.HasWalls ^= WallsDirections.Right;
                    firstCell.HasWalls ^= WallsDirections.Left;
                }
            }
            else
            {
                if (direction.y == 1)
                {
                    firstCell.HasWalls ^= WallsDirections.Up;
                    secondCell.HasWalls ^= WallsDirections.Down;
                }
                else
                {
                    secondCell.HasWalls ^= WallsDirections.Up;
                    firstCell.HasWalls ^= WallsDirections.Down;
                }
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor that adds a simple button to the inspector.
    /// </summary>
    [CustomEditor(typeof(Maze))]
    public class MazeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate Maze"))
            {
                ((Maze)target).GenerateMaze();
            }
        }
    }
#endif
}
