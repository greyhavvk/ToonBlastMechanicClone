using Core.ParticleSystems;
using ParticleSystems;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class RocketBlock : PowerUpBlock
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] rocketSprites;

        private bool _rocketDirectionIsHorizontal;

        public override void Placed()
        {
            SetRocketDirection();
        }

        private void SetRocketDirection()
        {
            int directionIndex = Random.Range(0, 1);
            _rocketDirectionIsHorizontal =(directionIndex  == 0);
            spriteRenderer.sprite = rocketSprites[directionIndex];
        }
        
        protected override void PlayBlastParticle()
        {
            ParticleManager.Instance.PlayParticle(
                _rocketDirectionIsHorizontal ? ParticleType.RocketBlockHorizontal : ParticleType.RocketBlockVertical,
                transform.position);
        }
    }
}