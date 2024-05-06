using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class RocketBlock : PowerUpBlock
    {
        [SerializeField] private GameObject horizontalHeadParent;
        [SerializeField] private GameObject verticalHeadParent;
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
            int directionIndex = Random.Range(0, 2);
            _rocketDirectionIsHorizontal =(directionIndex  == 0);
            horizontalHeadParent.SetActive(_rocketDirectionIsHorizontal);
            verticalHeadParent.SetActive(!_rocketDirectionIsHorizontal);
        }
        
        protected override void PlayBlastParticle()
        {
            var rocketHeads = _rocketDirectionIsHorizontal ? horizontalRocketHeads : verticalRocketHeads;
            rocketHeads[0].TriggerHead(_rocketHeadTargets.Item1, _rocketHeadSpeed);
            rocketHeads[1].TriggerHead(_rocketHeadTargets.Item2, _rocketHeadSpeed);
        }
    }
}