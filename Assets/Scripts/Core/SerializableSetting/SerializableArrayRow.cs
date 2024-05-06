using System;
using System.Collections.Generic;

namespace Core.SerializableSetting
{
    [Serializable]
    public class SerializableArrayRow<TValue>
    {
        public List<TValue> row;

        public SerializableArrayRow(List<TValue> row)
        { 
            this.row=row;
        }
    }
}