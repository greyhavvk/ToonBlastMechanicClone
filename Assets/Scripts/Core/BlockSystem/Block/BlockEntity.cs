using Factory_and_ObjectPool;
using ParticleSystems;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class BlockEntity : PoolableObject, IBlock
    {
        [SerializeField] private BlockType blockType;
        
        public BlockType GetBlockType()
        {
            return blockType;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public virtual void BlastBlock()
        {
            PlayBlastParticle();
            ReturnToPool();
        }

        protected virtual void PlayBlastParticle()
        {
            
        }

        public virtual void Placed()
        {
            
        }
    }
}