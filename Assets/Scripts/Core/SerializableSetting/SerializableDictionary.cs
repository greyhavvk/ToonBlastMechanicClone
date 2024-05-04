using System.Collections.Generic;
using UnityEngine;

namespace SerializableSetting
{
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys;

        [SerializeField]
        private List<TValue> values;
        
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (keys.Count != values.Count)
                throw new System.Exception($"Error deserializing SerializableDictionary: keys count ({keys.Count}) does not match values count ({values.Count}).");

            for (int i = 0; i < keys.Count; i++)
                Add(keys[i], values[i]);
        }
    }
}