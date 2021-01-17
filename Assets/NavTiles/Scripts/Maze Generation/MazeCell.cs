using UnityEngine;

namespace Snowcap.NavTiles.MazeGeneration
{
    /// <summary>
    /// Enum to keep track of what walls are active in the four cardinal-directions.
    /// </summary>
    [System.Flags]
    public enum WallsDirections
    {
        None = 0,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
        All = 15
    }

    /// <summary>
    /// A cell within the maze. 
    /// This is not a single tile but represents an intersection within the maze.
    /// </summary>
    public class MazeCell
    {
        /// <summary>
        /// Is this cell visited during the maze generation algorithm.
        /// </summary>
        public bool IsVisited = false;

        /// <summary>
        /// Keeps track of what walls are active in the four cardinal-directions.
        /// </summary>
        public WallsDirections HasWalls = WallsDirections.All;

        /// <summary>
        /// Position within the maze array.
        /// </summary>
        public Vector2Int Position;

        /// <summary>
        /// Constructs a new maze cell for the given position.
        /// </summary>
        /// <param name="position">2D position of the maze cell.</param>
        public MazeCell(Vector2Int position)
        {
            Position = position;
        }
    }
}
