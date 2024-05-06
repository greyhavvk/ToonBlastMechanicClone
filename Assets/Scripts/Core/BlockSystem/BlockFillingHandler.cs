using System;
using System.Collections.Generic;
using BlockSystem;
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
        
        
        //TODO burada bir sıkıntı var ama ne oldugunu çözemedim henüz
        public (List<(int,IBlock[])>, IBlock[][]) UpdateBlockMap(IBlock[][] blockMap)
        {
            var newMap= CalculateNewMap(blockMap);

            var columnIndexAndFillingBlocks = GenerateBlocksForNeededColumns(newMap);

            
            
            return (columnIndexAndFillingBlocks, newMap);
        }

        private void HaveJustOne(IBlock[][] blockMap, List<(int, IBlock[])> columnIndexAndFillingBlocks)
        {
            Dictionary<IBlock,Vector2Int> uniqueList=new Dictionary<IBlock,Vector2Int>();

            for (int i = 0; i < blockMap.Length; i++)
            {
                for (int j = 0; j <  blockMap[0].Length; j++)
                {
                    var block = blockMap[i][j];
                    if (block!=null)
                    {
                        if (uniqueList.ContainsKey(block))
                        {
                            var founded = uniqueList[block];
                            Debug.Log(block + " " + new Vector2Int(i,j) + " "+ founded);
                        }
                        else
                        {
                            uniqueList.Add(blockMap[i][j], new Vector2Int(i,j));
                        }
                    }
                }
            }
            
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

                        for (int k = 0; k < blockMap.Length; k++)
                        {
                            for (int l = 0; l < blockMap[0].Length; l++)
                            {
                                if (blockMap[k][l]==newBlock)
                                {
                                    Debug.Log("aynısını spawn etti" + newBlock);
                                }
                            }
                        }
                        newBlock?.Placed();
                        spawnedBlocks.Add(newBlock);
                        blockMap[i][j] = newBlock;
                    }
                }
                columnIndexAndFillingBlocks.Add((j,spawnedBlocks.ToArray()));
                spawnedBlocks.Clear();
            }
           
            HaveJustOne(blockMap,columnIndexAndFillingBlocks);
            return columnIndexAndFillingBlocks;
        }

        private int FindDeeperEmptyBlockMapRowIndex(int currentColumnIndex, int startRowIndex, IBlock[][] blockMap)
        {
            int foundedIndex = startRowIndex;
            int deeper = blockMap.Length-1;
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

            return foundedIndex;
        }
    }
}