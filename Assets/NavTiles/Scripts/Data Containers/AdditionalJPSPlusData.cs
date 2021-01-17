using System;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Class to store extra grid baking data specifically used for the Jump Point Search+ algorithm 
    /// </summary>
    [System.Serializable]
    public class AdditionalJPSPlusData
    {
        // Enum with flags for the directions.
        [Flags]
        public enum JumpPointDirection
        {
            None    = 0,
            North   = 1,
            East    = 1 << 1,
            South   = 1 << 2,
            West    = 1 << 3
        }

        // Flag variable storing which directions a tile counts as a jump point.
        public JumpPointDirection JumpPointDirections;

        // Distance to relevant tiles in cardinal directions.
        public int NorthwardDistance;
        public int EastwardDistance;
        public int SouthwardDistance;
        public int WestwardDistance;

        // The direction of which the search came from.
        private JumpPointDirection _parentDirection;
        public JumpPointDirection ParentDirection
        {
            get
            {
                return _parentDirection;
            }
            set
            {
                _parentDirection = value;
            }
        }

        /// <summary>
        /// Sets the jump distance for a particular direction.
        /// </summary>
        public void SetJumpDistance(JumpPointDirection inDirection, int inDistance)
        {
            switch (inDirection)
            {
                case JumpPointDirection.North:
                    NorthwardDistance = inDistance;
                    break;
                case JumpPointDirection.East:
                    EastwardDistance = inDistance;
                    break;
                case JumpPointDirection.South:
                    SouthwardDistance = inDistance;
                    break;
                case JumpPointDirection.West:
                    WestwardDistance = inDistance;
                    break;
            }
        }

        /// <summary>
        /// Gets the jump distance for a particular direction.
        /// </summary>
        public int GetJumpDistance(JumpPointDirection inDirection)
        {
            switch (inDirection)
            {
                case JumpPointDirection.North:
                    return NorthwardDistance;
                case JumpPointDirection.East:
                    return EastwardDistance;
                case JumpPointDirection.South:
                    return SouthwardDistance;
                case JumpPointDirection.West:
                    return WestwardDistance;
            }

            return 0;
        }
    }
}
