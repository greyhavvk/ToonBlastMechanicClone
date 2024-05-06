using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.SerializableSetting
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> where TKey: IComparable
    {
        [SerializeField] private List<Pair<TKey, TValue>> elements = new List<Pair<TKey, TValue>>();

        public int Count => elements.Count;

        public IEnumerator<Pair<TKey, TValue>> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public TValue GetValue(TKey key)
        {
            foreach (var pair in elements)
            {
                if (pair.Key.Equals(key))
                {
                    return pair.Value;
                }
            }

            return default;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var pair in elements)
            {
                if (pair.Key.Equals(key))
                {
                    value = pair.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            foreach (var pair in elements)
            {
                if (pair.Key.Equals(key)) return true;
            }

            return false;
        }

        public bool ContainsValue(TValue value)
        {
            foreach (var pair in elements)
            {
                if (pair.Value.Equals(value)) return true;
            }

            return false;
        }

        public TValue GetValueRandomly()
        {
            return elements[Random.Range(0, elements.Count)].Value;
        }

#if UNITY_EDITOR
        private bool ValidateDuplicatedKeys(List<Pair<TKey, TValue>> elements)
        {
            for (var i = 1; i < elements.Count; i++)
            {
                if (elements[i].Key.Equals(elements[i - 1].Key)) return false;
            }

            return true;
        }

        private void OnElementsChanged()
        {
            SortAscending();
        }

        private void SortAscending()
        {
            elements.Sort((x, y) => x.Key.CompareTo(y.Key));
        }
#endif
    }
}