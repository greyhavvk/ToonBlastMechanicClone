using UnityEngine;

namespace Core.Factory_and_ObjectPool
{
    public abstract class Factory : MonoBehaviour
    {
        protected PoolableObject RefPoolableObject;
        protected ObjectPool Pool;
        private const int SpawnCount = 10;
        
        public abstract PoolableObject GetProduct();
        protected abstract void SetReferenceProduct();
       
        public virtual void Initialize(PoolableObjectInitializeData blockInitializeData)
        {
            SetReferenceProduct();
            SetObjectPool(blockInitializeData);
        }
        
        private void SetObjectPool(PoolableObjectInitializeData poolableObjectInitialize)
        {
            Pool = new ObjectPool(RefPoolableObject, SpawnCount, poolableObjectInitialize);
        }

      
    }
}