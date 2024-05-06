using Core.ParticleSystems;
using Enums;
using Managers;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class ObstacleBlock : BlockEntity
    {
        [SerializeField] private int hitPoint;

        public void ReduceHitPoint()
        {
            hitPoint = Mathf.Clamp(hitPoint - 1, 0, int.MaxValue);
        }

        public bool IsBroken => hitPoint == 0;

        protected override void PlayBlastParticle()
        {
            OnPlayParticle?.Invoke(ParticleType.ObstacleBlock, transform.position);
        }
    }
}