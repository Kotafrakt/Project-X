using System;
using UnityEngine;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Holds info about an area which is assigned to tiles.
    /// </summary>
    [Serializable]
    public class NavTileArea
    {
        public string Name;
        public int Cost;
        public Color Color;
        public int Priority;

        public NavTileArea()
        {
            Name = "";
            Cost = 1;
            Color = Color.white;
            Priority = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inName">Name of the area.</param>
        /// <param name="inCost">Cost to walk on this area.</param>
        /// <param name="inColor">Colour corresponding with this tile in debug mode.</param>
        /// <param name="inPriority">Priority over other tiles in the same location, across multiple Tilemaps.</param>
        public NavTileArea(string inName, int inCost, Color inColor, int inPriority)
        {
            this.Name = inName;
            this.Cost = inCost;
            this.Color = inColor;
            this.Priority = inPriority;
        }
    }
}
