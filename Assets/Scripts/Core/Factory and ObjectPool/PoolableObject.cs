using UnityEngine;

namespace Factory_and_ObjectPool
{
    public class PoolableObject : MonoBehaviour
    {
        private ObjectPool _objectPool;

        public virtual void Initialize(PoolableObjectInitializeData poolableObjectInitializeData)
        {
            
        }
        
        public void SetObjectPool(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }
        
        protected virtual void ReturnToPool()
        {
            _objectPool.ReturnEntity(this);
        }
    }
}