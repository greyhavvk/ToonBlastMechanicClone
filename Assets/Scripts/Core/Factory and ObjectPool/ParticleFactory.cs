using UnityEngine;

namespace Core.Factory_and_ObjectPool
{
    public class ParticleFactory : Factory
    {
        [SerializeField] private PoolableObject refPoolableObject;
        
        public override PoolableObject GetProduct()
        {
            return Pool.GetEntity();
        }

        protected override void SetReferenceProduct()
        {
            RefPoolableObject = refPoolableObject;
        }
    }
}