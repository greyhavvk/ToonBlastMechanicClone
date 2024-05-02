using System;
using BlockSystem.Block;
using UnityEngine;

namespace BlockSystem
{
    [Serializable]
    public class BlockData
    {
        public Vector2Int blockPositionOnGrid;
        public BlockType blockType;
    }
}