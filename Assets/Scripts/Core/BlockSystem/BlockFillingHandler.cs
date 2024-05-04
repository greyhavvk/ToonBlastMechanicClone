using System;
using System.Collections.Generic;
using BlockSystem;
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

        private BlockType GetRandomBulletType()
        {
            var randomColor = Random.Range(0, sizeof(ColorType) - 1);
            var randomBlockType =
                (BlockType)Enum.Parse(typeof(BlockType), ((ColorType)randomColor).ToString() + "Block");
            return randomBlockType;
        }
        
        public (List<(int,IBlock[])>, IBlock[][]) UpdateBlockMap(IBlock[][] blockMap)
        {
            var columnIndexAndFillingBlocks = new List<(int, IBlock[])>();
            for (int j = 0; j < blockMap.GetLength(1); j++)
            {
                int neededBlock = 0;
                List<IBlock> spawnedBlocks = new List<IBlock>();
                for (int i = blockMap.GetLength(0)-1; i >= 0; i--)
                {
                    var block = blockMap[i][j];
                    if (block!=null)
                    {
                        if (block.GetBlockType()==BlockType.ObstacleBlock)
                        {
                            neededBlock = 0;
                        }
                        else
                        {
                            var newRowIndex = FindDeeperEmptyBlockMapRowIndex(j, i, blockMap);
                            if (newRowIndex<i)
                            {
                                blockMap[i][newRowIndex] = block;
                                blockMap[i][j] = null;
                                neededBlock++;
                            }
                        }
                    }
                    else
                    {
                        neededBlock++;
                    }
                }

                if (blockMap[0][j]!=null)
                {
                    neededBlock=0;
                }

                for (int i = 0; i < neededBlock; i++)
                {
                    var newBlock = blockFactories[GetRandomBulletType()].GetProduct() as IBlock;
                    spawnedBlocks.Add(newBlock);
                }
                columnIndexAndFillingBlocks.Add((j, spawnedBlocks.ToArray()));
            }

            return (columnIndexAndFillingBlocks, blockMap);
        }

        private int FindDeeperEmptyBlockMapRowIndex(int currentColumnIndex, int startRowIndex, IBlock[][] blockMap)
        {
            int foundedIndex = startRowIndex;
            int deeper = blockMap.GetLength(0);
            for (int i = deeper; i >= startRowIndex; i--)
            {
                var block = blockMap[i][currentColumnIndex];
                if (block==null)
                {
                    foundedIndex = Mathf.Max(i, foundedIndex);
                }
                else if (block.GetBlockType()==BlockType.ObstacleBlock)
                {
                    foundedIndex = startRowIndex;
                }
                
            }

            return currentColumnIndex;
        }
    }
}