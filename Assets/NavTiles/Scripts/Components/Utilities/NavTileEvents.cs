using UnityEngine;
using UnityEngine.Events;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Unity Event called when a NavTileAgent changes area type.
    /// </summary>
    [System.Serializable]
    public class NavTileAreaUnityEvent : UnityEvent<int> { }

    /// <summary>
    /// Unity Event called when a path is found. Gives the path found.
    /// </summary>
    [System.Serializable]
    public class NavTilePathFoundEvent : UnityEvent<NavTilePath> { }

    /// <summary>
    /// Unity Event called when the path is aborted. Gives a reason and a position on which the Agent could not get to.
    /// </summary>
    [System.Serializable]
    public class NavTilePathAbortedEvent : UnityEvent<NavTileAgent.EAbortReason, Vector2Int> { }
}
