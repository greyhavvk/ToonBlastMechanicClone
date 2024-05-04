using ParticleSystems;
using UnityEngine;
using UnityEngine.Serialization;

namespace BlockSystem.Block
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
        
        public override void BlastBlock()
        {
            ParticleManager.Instance.PlayParticle(
                _rocketDirectionIsHorizontal ? ParticleType.RocketBlockHorizontal : ParticleType.RocketBlockVertical,
                transform.position);
        }
    }
}