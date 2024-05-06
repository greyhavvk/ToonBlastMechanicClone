using System;
using Enums;
using UnityEngine;

namespace Core.Factory_and_ObjectPool
{
    public class BlockInitializeData:PoolableObjectInitializeData
    {
        public readonly Action<ParticleType, Vector2> OnPlayParticle;
        public BlockInitializeData(Action<ParticleType, Vector2> onPlayParticle)
        {
            OnPlayParticle = onPlayParticle;
        }
    }
}