using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public interface IBlock
    {
        BlockType GetBlockType();
        Transform GetTransform();
        void BlastBlock();

        void DelayedBlastBlock(int delay);

        void Placed();
    }
}