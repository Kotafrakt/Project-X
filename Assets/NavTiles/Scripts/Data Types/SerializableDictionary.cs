using System;
using System.Collections.Generic;
using UnityEngine;

namespace Snowcap.Utilities
{
    /// <summary>
    /// Dictionary which serializes all elements to 2 lists so Unity can store them. Inherit to specify types.
    /// </summary>
    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected List<TKey> _keys = new List<TKey>();

        [SerializeField]
        protected List<TValue> _values = new List<TValue>();

        public SerializableDictionary() { }

        public SerializableDictionary(int inSize) : base(inSize) { }

        // Save the dictionary to lists.
        public virtual void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        // Load dictionary from lists.
        public virtual void OnAfterDeserialize()
        {
            this.Clear();

            if (_keys.Count != _values.Count)
                throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.", 
                                                         _keys.Count, _values.Count));

            for (int i = 0; i < _keys.Count; i++)
                this.Add(_keys[i], _values[i]);
        }

        public int GetIndexOfKey(TKey inKey)
        {
            return _keys.IndexOf(inKey);
        }
    }
}
