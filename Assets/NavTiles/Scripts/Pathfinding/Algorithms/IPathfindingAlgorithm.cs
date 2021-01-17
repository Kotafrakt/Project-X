namespace Snowcap.NavTiles
{
    /// <summary>
    /// Interface for a pathfinding algorithm class.
    /// </summary>
    public interface IPathfindingAlgorithm
    {
        /// <summary>
        /// Gets the display name of the algorithm.
        /// </summary>
        /// <returns>The display name of the algorithm.</returns>
        string GetName();

        /// <summary>
        /// Gets the bake state that is required for this algorithm to work.
        /// </summary>
        /// <returns>The required bake state for this algorithm.</returns>
        NavTileSurfaceData.BakeState GetRequiredBakeState();

        /// <summary>
        /// Find a path with given input.
        /// </summary>
        /// <param name="inInput">Input variables used in calculating the path.</param>
        /// <returns>A path from A to B.</returns>
        NavTilePath FindPath(FindPathInput inInput);
    }
}
