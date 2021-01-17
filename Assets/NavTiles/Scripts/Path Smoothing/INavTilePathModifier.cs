namespace Snowcap.NavTiles
{
    /// <summary>
    /// Interface for path smoothing algorithms.
    /// </summary>
    public interface INavTilePathModifier
    {
        NavTilePath ModifyPath(NavTilePath inPath);

        string GetName();
    }   
}
