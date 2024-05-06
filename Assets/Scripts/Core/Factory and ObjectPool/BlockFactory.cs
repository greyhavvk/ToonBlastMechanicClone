using UnityEngine;

namespace Core.Factory_and_ObjectPool
{
    public class BlockFactory : Factory
    {
        [SerializeField] private PoolableObject refPoolableObject;
        
        public override PoolableObject GetProduct()
        {
            var entity = Pool.GetEntity();
            return entity;
        }

        protected override void SetReferenceProduct()
        {
            RefPoolableObject = refPoolableObject;
        }
    }
}