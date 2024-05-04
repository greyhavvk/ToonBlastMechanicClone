using System;
using Core.ParticleSystems;
using ParticleSystems;

namespace Core.BlockSystem.Block
{
    public class ColorBlock : BlockEntity
    {
        protected override void PlayBlastParticle()
        {
            ParticleManager.Instance.PlayParticle(
                (ParticleType)Enum.Parse(typeof(ParticleType), GetBlockType().ToString()),
                transform.position);
        }
    }
}