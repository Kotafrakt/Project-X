namespace Snowcap.NavTiles
{
    /// <summary>
    /// Interface used to integrate any tile into NavTile.
    /// Implement this interface if you have a custom tile and 
    /// want full control over its area index.
    /// </summary>
    public interface IHasNavTileArea
    {
        /// <summary>
        /// Specify which area index is used for this tile.
        /// </summary>
        /// <returns>Area index.</returns>
        int GetNavTileAreaIndex();
    }
}
