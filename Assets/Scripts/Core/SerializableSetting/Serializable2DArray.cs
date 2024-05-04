using System;
using System.Collections.Generic;

namespace SerializableSetting
{
    [Serializable]
    public class Serializable2DArray<TValue>
    {
        public List<List<TValue>> Array = new List<List<TValue>>();

        public Serializable2DArray(TValue[][] arrayToSerialize)
        {
            foreach (TValue[] row in arrayToSerialize)
            {
                List<TValue> serializedRow = new List<TValue>(row);
                Array.Add(serializedRow);
            }
        }

        public TValue[][] ToArray()
        {
            TValue[][] deserializedArray = new TValue[Array.Count][];
            for (int i = 0; i < Array.Count; i++)
            {
                deserializedArray[i] = Array[i].ToArray();
            }
            return deserializedArray;
        }
    }
}