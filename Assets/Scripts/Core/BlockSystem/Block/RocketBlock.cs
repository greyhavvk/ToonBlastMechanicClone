using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class RocketBlock : PowerUpBlock
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] rocketSprites;
        [SerializeField] private RocketHead[] horizontalRocketHeads;
        [SerializeField] private RocketHead[] verticalRocketHeads;

        private float _rocketHeadSpeed;
        private (Vector2, Vector2) _rocketHeadTargets;
        
        private bool _rocketDirectionIsHorizontal;
        public bool RocketDirectionIsHorizontal => _rocketDirectionIsHorizontal;

        public override void Placed()
        {
            SetRocketDirection();
        }

        public void SetRocketHeads((Vector2, Vector2) targetPositions, float speed)
        {
            _rocketHeadTargets = targetPositions;
            _rocketHeadSpeed = speed;
        }

        private void SetRocketDirection()
        {
            int directionIndex = Random.Range(0, 1);
            _rocketDirectionIsHorizontal =(directionIndex  == 0);
            spriteRenderer.sprite = rocketSprites[directionIndex];
        }
        
        protected override void PlayBlastParticle()
        {
            var rocketHeads = _rocketDirectionIsHorizontal ? horizontalRocketHeads : verticalRocketHeads;
            rocketHeads[0].TriggerHead(_rocketHeadTargets.Item1, _rocketHeadSpeed);
            rocketHeads[1].TriggerHead(_rocketHeadTargets.Item2, _rocketHeadSpeed);
        }
    }
}