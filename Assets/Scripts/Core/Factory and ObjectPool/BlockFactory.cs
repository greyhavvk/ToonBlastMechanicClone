using System;
using Factory_and_ObjectPool;
using UnityEngine;

namespace BlockSystem
{
    public class BlockFactory : Factory
    {
        [SerializeField] private PoolableObject refPoolableObject;

        private ObjectPool _pool;
        private const int SpawnCount = 10;

        public void Initialize()
        {
            SetObjectPool();
        }
        
        private void SetObjectPool()
        {
            _pool = new ObjectPool(refPoolableObject, SpawnCount, new BlockInitializeData());
        }

        public override PoolableObject GetProduct()
        {
            return _pool.GetEntity();
        }
    }
}