using BlockSystem;
using Factory_and_ObjectPool;
using UnityEngine;

namespace Core.Factory_and_ObjectPool
{
    public class ParticleFactory : Factory
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