using System;
using Core.Factory_and_ObjectPool;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class BlockEntity : PoolableObject, IBlock
    {
        [SerializeField] private BlockType blockType;
        protected Action<ParticleType, Vector2> OnPlayParticle;

        public override void Initialize(PoolableObjectInitializeData poolableObjectInitializeData)
        {
            if (poolableObjectInitializeData is BlockInitializeData data)
            {
                OnPlayParticle = data.OnPlayParticle;
            }
        }


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
            OnPlayParticle?.Invoke(GetParticleType(), transform.position);
        }

        protected virtual ParticleType GetParticleType()
        {
            return (ParticleType) Enum.Parse(typeof(ParticleType), blockType.ToString());
        }

        public virtual void Placed()
        {

        }
    }
}