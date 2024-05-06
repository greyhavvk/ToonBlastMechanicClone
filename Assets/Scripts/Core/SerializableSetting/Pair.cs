using System;
using UnityEngine;

namespace Core.SerializableSetting
{
    [Serializable]
    public class Pair<TKey, TValue>
    {
        public TKey Key
        {
            get => _key;
            private set => _key = value;
        }

        public TValue Value
        {
            get => _value;
            private set => _value = value;
        }

        [SerializeField]
        private TKey _key;

        [SerializeField]
        private TValue _value;

        public Pair()
        {
        }

        public Pair(TKey key, TValue value)
        {
            Set(key, value);
        }

        public void Set(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}