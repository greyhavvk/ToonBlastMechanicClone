using Core.ParticleSystems;
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
            ParticleManager.Instance.PlayParticle(ParticleType.ObstacleBlock, transform.position);
        }
    }
}