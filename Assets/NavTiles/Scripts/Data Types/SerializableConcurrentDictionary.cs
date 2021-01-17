using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snowcap.Utilities
{
    /// <summary>
    /// Dictionary which serializes all elements to 2 lists so Unity can store them. Inherit to specify types.
    /// </summary>
    [Serializable]
    public abstract class SerializableConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected List<TKey> _keys = new List<TKey>();

        [SerializeField]
        protected List<TValue> _values = new List<TValue>();

        public SerializableConcurrentDictionary() { }

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
            if (_keys.Count != _values.Count)
                throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable.",
                                                         _keys.Count, _values.Count));

            for (int i = 0; i < _keys.Count; i++)
                this[_keys[i]] = _values[i];
        }
    }
}
