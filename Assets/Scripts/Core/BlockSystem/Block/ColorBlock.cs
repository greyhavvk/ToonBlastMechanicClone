using System;
using Core.ParticleSystems;
using Core.TrackerSystem;
using ParticleSystems;

namespace Core.BlockSystem.Block
{
    public class ColorBlock : BlockEntity
    {
        public override void BlastBlock()
        {
            base.BlastBlock();
            GoalTracker.Instance.BlockDestroyed(GetBlockType());
        }

        protected override void PlayBlastParticle()
        {
            ParticleManager.Instance.PlayParticle(
                (ParticleType)Enum.Parse(typeof(ParticleType), GetBlockType().ToString()),
                transform.position);
        }
    }
}