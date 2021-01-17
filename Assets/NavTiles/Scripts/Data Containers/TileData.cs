using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Holds data for a single tile in the NavTile grid.
    /// This is what is being stored for each tile.
    /// </summary>
    [Serializable]
    public class TileData
    {
        /// <summary>
        /// The index of the area used for this tile.
        /// </summary>
        public int AreaIndex;

        /// <summary>
        /// Area information based on the area index of the tile (read-only).
        /// </summary>
        public NavTileArea Area { get { return NavTileManager.Instance.AreaManager.GetAreaByID(AreaIndex); } }

        /// <summary>
        /// Additional Data for the tile if Jump Point Search+ is used.
        /// </summary>
        [SerializeField]
        public AdditionalJPSPlusData AdditionalData;

        /// <summary>
        /// List of agents occupying the current tile.
        /// </summary>
        public List<NavTileAgent> OccupyingAgents = new List<NavTileAgent>();

        public TileData(int inAreaIndex)
        {
            this.AreaIndex = inAreaIndex;
        }

        /// <summary>
        /// Check if tile is walkable comparing to the given mask.
        /// </summary>
        /// <param name="inAreaMask">Area mask to compare to.</param>
        /// <returns>Whether or not the tile is walkable.</returns>
        public bool IsWalkable(int inAreaMask)
        {
            return ((1 << AreaIndex) & inAreaMask) != 0;
        }

        /// <summary>
        /// Checks if this tile contains an agent that obstructs the given agent.
        /// </summary>
        /// <param name="inAgent">Agent to check obstruction for.</param>
        /// <param name="outObstructingAgent">Found agent that is obstructing.</param>
        /// <returns>Whether or not an obstruction was found.</returns>
        public bool IsObstructed(NavTileAgent inAgent, out NavTileAgent outObstructingAgent)
        {
            NavTileAgentManager manager = NavTileManager.Instance.AgentManager;
            outObstructingAgent = null;

            foreach (NavTileAgent agent in OccupyingAgents)
            {
                if (manager.GetValue(inAgent.AgentType, agent.AgentType))
                {
                    outObstructingAgent = agent;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all occupying and obstructing agents ordered per status in a dictionary.
        /// </summary>
        /// <param name="inAgent">Agent to check obstruction for.</param>
        /// <returns>Dictionary containing all obstructing agents order per status.</returns>
        public Dictionary<NavTileAgent.EMovementStatus, List<NavTileAgent>> GetSortedOccupyingAgents(NavTileAgent inAgent)
        {
            Dictionary<NavTileAgent.EMovementStatus, List<NavTileAgent>> dic = new Dictionary<NavTileAgent.EMovementStatus, List<NavTileAgent>>();
            NavTileAgentManager manager = NavTileManager.Instance.AgentManager;

            foreach (NavTileAgent agent in OccupyingAgents)
            {
                if (manager.GetValue(inAgent.AgentType, agent.AgentType))
                {
                    List<NavTileAgent> agents;
                    if (dic.TryGetValue(agent.MovementStatus, out agents))
                    {
                        agents.Add(agent);
                    }
                    else
                    {
                        agents = new List<NavTileAgent>() { agent };

                        dic.Add(agent.MovementStatus, agents);
                    }
                }
            }

            return dic;
        }
    }
}
