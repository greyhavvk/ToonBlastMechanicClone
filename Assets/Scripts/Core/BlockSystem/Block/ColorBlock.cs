using Core.TrackerSystem;

namespace Core.BlockSystem.Block
{
    public class ColorBlock : BlockEntity
    {
        public override void BlastBlock()
        {
            base.BlastBlock();
            GoalTracker.Instance.BlockDestroyed(GetBlockType());
        }
    }
}