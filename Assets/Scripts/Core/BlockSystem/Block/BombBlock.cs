using System;
using Core.ParticleSystems;
using ParticleSystems;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class BombBlock : PowerUpBlock
    {
        protected override void PlayBlastParticle()
        {
            ParticleManager.Instance.PlayParticle(ParticleType.BombBlock,
                transform.position);
        }
    }
}