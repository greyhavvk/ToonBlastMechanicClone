using System;
using BlockSystem;
using BlockSystem.Block;
using Core.BlockSystem.Block;
using SerializableSetting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.BlockSystem
{
    public class BlockFillingHandler : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<BlockType, BlockFactory> blockFactories;

        public static BlockFillingHandler Instance { get; private set; }

        private void Awake()
        {
            SetInstance();
            InitializeFactories();
        }

        private void InitializeFactories()
        {
            foreach (var blockFactory in blockFactories)
            {
                blockFactory.Value.Initialize();
            }
        }

        private void SetInstance()
        {
            Instance = this;
        }

        private IBlock GetBlock(BlockType blockType)
        {
            if (!blockFactories.ContainsKey(blockType)) return null;
            var block = blockFactories[blockType].GetProduct() as IBlock;
            return block;
        }

        public IBlock[][] SpawnRequestedBlocksByLevel(BlockType[][] requestedBlocks)
        {
            var x=requestedBlocks.GetLength(0);
            var y = requestedBlocks.GetLength(1);
            IBlock[][] blocks = new IBlock[x][];

            for (int i = 0; i < x; i++)
            {
                blocks[i] = new IBlock[y];

                for (int j = 0; j < y; j++)
                {
                    blocks[i][j] = GetBlock(requestedBlocks[i][j]);
                }
            }

            return blocks;
        }

        public IBlock SpawnBlock(BlockType blockType)
        {
            return GetBlock(blockType);
        }

        public IBlock[] SpawnBlocksForEmptyCellsInColumn(int requestedCount)
        {
            IBlock[] blocks = new IBlock[requestedCount];
            for (int i = 0; i < requestedCount; i++)
            {
                blocks[i] = GetBlock(GetRandomBulletType());
            }

            return blocks;
        }

        private BlockType GetRandomBulletType()
        {
            var randomColor = Random.Range(0, sizeof(ColorType) - 1);
            var randomBlockType =
                (BlockType)Enum.Parse(typeof(BlockType), ((ColorType)randomColor).ToString() + "Block");
            return randomBlockType;
        }
    }
}