using System;
using System.Collections.Generic;
using Core.BlockSystem.Block;
using Core.Factory_and_ObjectPool;
using Core.SerializableSetting;
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
            if (blockType==BlockType.RandomColor)
            {
                var randomBlock = blockFactories.GetValue(GetRandomBulletType()).GetProduct() as IBlock;
                randomBlock?.Placed();
                return randomBlock;
            }
            if (!blockFactories.ContainsKey(blockType)) return null;
            var block = blockFactories.GetValue(blockType).GetProduct() as IBlock;
            block?.Placed();
            return block;
        }

        public IBlock[][] SpawnRequestedBlocksByLevel(BlockType[][] requestedBlocks)
        {
            var x=requestedBlocks.Length;
            var y = requestedBlocks[0].Length;
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
            var randomColor = Random.Range(0, 6);
            var randomBlockType =
                (BlockType)Enum.Parse(typeof(BlockType), ((ColorType)randomColor).ToString() + "Block");
            return randomBlockType;
        }
        
        public (List<(int,IBlock[])>, IBlock[][]) UpdateBlockMap(IBlock[][] blockMap)
        {
            var newMap= CalculateNewMap(blockMap);

            var columnIndexAndFillingBlocks = GenerateBlocksForNeededColumns(newMap);

            
            
            return (columnIndexAndFillingBlocks, newMap);
        }

        private IBlock[][] CalculateNewMap(IBlock[][] blockMap)
        {
            var newMap = blockMap;

            for (int j = 0; j < blockMap[0].Length; j++)
            {
                List<Vector2Int> emptyPositions = new List<Vector2Int>();
                for (int i = newMap.Length-1; i >= 0; i--)
                {
                    var block = newMap[i][j];
                    if (block!=null)
                    {
                        if (block.GetBlockType()==BlockType.ObstacleBlock)
                        {
                            emptyPositions.Clear();
                        }
                        else if (emptyPositions.Count>0)
                        {
                            var newIndex = emptyPositions[0];
                            emptyPositions.Remove(newIndex);
                            
                            newMap[newIndex.x][newIndex.y] = block;
                            newMap[i][j] = null;
                            emptyPositions.Add(new Vector2Int(i,j));
                        }
                    }
                    else
                    {
                        emptyPositions.Add(new Vector2Int(i,j));
                    }
                }
                emptyPositions.Clear();
            }
            
            return newMap;
        }

        private List<(int, IBlock[])> GenerateBlocksForNeededColumns(IBlock[][] blockMap)
        {
            List<(int, IBlock[])> columnIndexAndFillingBlocks = new List<(int, IBlock[])>();

            for (int j = 0; j < blockMap[0].Length; j++)
            {
                List<IBlock> spawnedBlocks = new List<IBlock>();
                for (int i = 0; i < blockMap.Length; i++)
                {
                    var block = blockMap[i][j];
                    if (block!=null)
                    {
                        break;
                    }
                    else
                    {
                        var newBlock = blockFactories.GetValue(GetRandomBulletType()).GetProduct() as IBlock;
                        newBlock?.Placed();
                        spawnedBlocks.Add(newBlock);
                        blockMap[i][j] = newBlock;
                    }
                }
                columnIndexAndFillingBlocks.Add((j,spawnedBlocks.ToArray()));
                spawnedBlocks.Clear();
            }
           
            return columnIndexAndFillingBlocks;
        }
    }
}