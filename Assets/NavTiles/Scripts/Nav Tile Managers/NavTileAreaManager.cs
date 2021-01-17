using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Snowcap.NavTiles
{
    [Serializable]
    public class NavTileAreaManager
    {
        [SerializeField]
        private NavTileArea[] _areas;
        public ReadOnlyCollection<NavTileArea> AllAreas { get { return Array.AsReadOnly(_areas); } }
        public ReadOnlyCollection<NavTileArea> UsedAreas { get { return _areas.Where(x => !String.IsNullOrEmpty(x.Name)).ToList().AsReadOnly(); } }
        public List<string> AllAreaNames { get { return AllAreas.Select(x => x.Name).ToList(); } }
        public List<string> UsedAreaNames { get { return UsedAreas.Select(x => x.Name).ToList(); } }

        /// <summary>
        /// Creates the list of areas and sets the two default values.
        /// </summary>
        public void InitializeDefaultNavTileAreas()
        {
            float hueStep = 93.32f / 255f; //93.32f gives a varied color selection with few repeats.
            float hue = 0;
            float saturation = .65f;
            float value = 1;

            _areas = new NavTileArea[32];
            _areas[0] = new NavTileArea("Walkable", 0, Color.HSVToRGB(.6f, saturation, value), 0);
            _areas[1] = new NavTileArea("Non Walkable", 0, Color.HSVToRGB(0, saturation, value), 1);

            for (int i = 2; i < _areas.Length; i++)
            {
                hue += hueStep;
                hue %= 1;
                _areas[i] = new NavTileArea("", 0, Color.HSVToRGB(hue, saturation, value), 0);
            }
        }

        /// <summary>
        /// Gets the area by its ID. Returns null if outside the array. Can return unused areas.
        /// </summary>
        /// <param name="inID">ID to retrieve the area for.</param>
        /// <returns>Area corresponding with the ID. Null if outside array. Unused areas can be returned.</returns>
        public NavTileArea GetAreaByID(int inID)
        {
            return inID >= 0 && inID < 32 ? _areas[inID] : null;
        }

        /// <summary>
        /// Finds the ID matched to an area. If not found, returns -1.
        /// </summary>
        /// <param name="inName">Name of the area index to find.</param>
        /// <returns>The index of the named area. -1 if non found.</returns>
        public int GetAreaIDByName(string inName)
        {
            return Array.FindIndex(_areas, item => string.Equals(item.Name.ToLower(), inName.ToLower()));
        }

        /// <summary>
        /// Finds the area with the given name. Null if no match is found.
        /// </summary>
        /// <param name="inName">Name to find the area for.</param>
        /// <returns>The area corresponding with the name. Null if no match is found.</returns>
        public NavTileArea GetAreaByName(string inName)
        {
            return Array.Find(_areas, item => string.Equals(item.Name.ToLower(), inName.ToLower()));
        }

        /// <summary>
        /// Checks if an area name is contained in the areas array multiple times.
        /// </summary>
        /// <param name="inName">The name to check for duplicate entries.</param>
        /// <returns>Whether the given area is contained in the array multiple times.</returns>
        public bool IsDuplicateEntry(string inName)
        {
            if (inName == string.Empty || inName == null)
            {
                return false;
            }
            return Array.FindAll(_areas, item => string.Equals(item.Name.ToLower(), inName.ToLower())).Count() > 1;
        }

        /// <summary>
        /// Checks if an index falls within the range of the area array.
        /// </summary>
        /// <param name="inIndex">Index to check.</param>
        /// <returns>True if index is valid within the area array. False if not.</returns>
        public static bool IsAreaIndexValid(int inIndex)
        {
            return inIndex >= 0 && inIndex < 32;
        }
    }
}
