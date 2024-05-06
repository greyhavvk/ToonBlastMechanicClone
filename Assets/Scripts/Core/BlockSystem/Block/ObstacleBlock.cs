using System;
using Core.ParticleSystems;
using ParticleSystems;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class ObstacleBlock : BlockEntity
    {
        [SerializeField] private int hitPoint;

        public override void BlastBlock()
        {
            base.BlastBlock();
            Debug.Log("blast emri geldi");
        }

        public void ReduceHitPoint()
        {
            hitPoint = Mathf.Clamp(hitPoint - 1, 0, int.MaxValue);
            Debug.Log("can düştü" + hitPoint);
        }

        public bool IsBroken => hitPoint == 0;

        protected override void PlayBlastParticle()
        {
            ParticleManager.Instance.PlayParticle(ParticleType.ObstacleBlock, transform.position);
        }
    }
}