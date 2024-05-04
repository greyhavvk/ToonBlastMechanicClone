using UnityEngine;

namespace Factory_and_ObjectPool
{
    public abstract class Factory : MonoBehaviour
    {
        public abstract PoolableObject GetProduct();
    }
}