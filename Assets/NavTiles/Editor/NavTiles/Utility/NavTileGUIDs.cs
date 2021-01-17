namespace Snowcap.NavTiles
{
    /// <summary>
    /// Class used to hold string values of GUIDs to load textures from the asset database.
    /// This prevents certain texture to be stored in the resources folder which would be compiled into the game.
    /// </summary>
    public static class NavTileGUIDs
    {
        public const string NAV_TILE_WINDOW_ICON = "d679ddca8e2b24d46a196b355e76783c";

        public static readonly string[] NAV_TILE_BANNER = { "d33a5c4886be689419f480077d7bef2e",
                                                            "68f836c73cd3b164bb245cd13207f59f",
                                                            "db3c1800cbd82a641977fb53bfa2539b" };
    }
}
