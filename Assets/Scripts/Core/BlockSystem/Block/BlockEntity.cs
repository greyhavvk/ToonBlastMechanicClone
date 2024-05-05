using Cysharp.Threading.Tasks;
using Factory_and_ObjectPool;
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

        public async void DelayedBlastBlock(int delay)
        {
            await UniTask.Delay(delay);
            BlastBlock();
        }

        protected virtual void PlayBlastParticle()
        {
            
        }

        public virtual void Placed()
        {
            
        }
    }
}