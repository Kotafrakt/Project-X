using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Snowcap.Utilities;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Takes care of all functionality related to the agents tab in the NavTile Settings window.
    /// </summary>
    [Serializable]
    public class NavTileAgentManager
    {
        // List of strings with agent type names.
        [SerializeField]
        private List<string> _agents = new List<string>();

        // Read only list of all agents.
        public ReadOnlyCollection<string> Agents { get { return _agents.AsReadOnly(); } }

        // Read only list of all named agents.
        public ReadOnlyCollection<string> NamedAgents { get { return _agents.Where(x => !String.IsNullOrEmpty(x)).ToList().AsReadOnly(); } }

        // Datatype to store booleans for the conflict matrix.
        // Key value is a hash value based on the x- and y-coordinate.
        [Serializable]
        private class AgentMatrixDictionary : SerializableDictionary<int, bool> {}

        [SerializeField]
        private AgentMatrixDictionary _conflictMatrix = new AgentMatrixDictionary();

        /// <summary>
        /// Initializes the default agents.
        /// </summary>
        public void InitializeDefaultAgents()
        {
            _agents.Add("Default");
            SetValue(0, 0, true);
        }

        /// <summary>
        /// Sets the value based on two indexes of the conflict matrix.
        /// </summary>
        public void SetValue(int inFirstIndex, int inSecondIndex, bool inValue)
        {
            int hash = GetMatrixHash(inFirstIndex, inSecondIndex);
            _conflictMatrix[hash] = inValue;
        }

        /// <summary>
        /// Gets the value based on two indexes of the conflict matrix.
        /// </summary>
        public bool GetValue(int inFirstIndex, int inSecondIndex)
        {
            int hash = GetMatrixHash(inFirstIndex, inSecondIndex);

            bool result;
            if (_conflictMatrix.TryGetValue(hash, out result))
            {
                return result;
            }

            return false;
        }

        /// <summary>
        /// Checks if an agent name is contained in the agents array multiple times.
        /// </summary>
        public bool IsDuplicateEntry(string inAgentName)
        {
            if (inAgentName == string.Empty || inAgentName == null)
            {
                return false;
            }
            return _agents.FindAll(agent => string.Equals(agent.ToLower(), inAgentName.ToLower())).Count() > 1;
        }

        /// <summary>
        /// Hash function to convert two integers to a unique single integer. Order does not matter for the outcome.
        /// </summary>
        public int GetMatrixHash(int inFirstIndex, int inSecondIndex)
        {
            return inSecondIndex >= inFirstIndex ? inSecondIndex * inSecondIndex + inSecondIndex + inFirstIndex : inSecondIndex + inFirstIndex * inFirstIndex;
        }

        public bool GetIndexOfHashInKeysList(int inHash, out int outIndex)
        {
            outIndex = 0;

            if (_conflictMatrix.ContainsKey(inHash))
            {
                outIndex = _conflictMatrix.GetIndexOfKey(inHash);
                return true;
            }

            return false;
        }
    }
}
