using Core.BlockSystem.Block;
using Factory_and_ObjectPool;
using UnityEngine;

namespace BlockSystem.Block
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
            ReturnToPool();
        }

        public virtual void Placed()
        {
            
        }
    }
}