using Core.BlockSystem.Block;
using UnityEngine;

namespace BlockSystem.Block
{
    public interface IBlock
    {
        BlockType GetBlockType();
        Transform GetTransform();
        void BlastBlock();

        void Placed();
    }
}