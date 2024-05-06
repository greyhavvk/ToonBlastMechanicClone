using System.Collections.Generic;
using System.Linq;
using Core.BlockSystem.Block;
using Enums;
using UnityEngine;

namespace Core.BlockSystem
{
    public class BlockSearchHandler : MonoBehaviour
    {
        private IBlock[][] _blockMap;

        public static BlockSearchHandler Instance;

        private List<(Vector2Int, int)> _foundedBlockIndexesAndOrders;

        public List<(Vector2Int, int)> FoundedBlockIndexesAndOrders => _foundedBlockIndexesAndOrders;

        public IBlock[][] BlockMap => _blockMap;

        private void Awake()
        {
            SetInstance();
            ResetSearchLists();
        }

        public void ResetSearchLists()
        {
            _foundedBlockIndexesAndOrders = new List<(Vector2Int, int)>();
        }

        private void SetInstance()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void SetBlockMap(IBlock[][] blockMap)
        {
            _blockMap = blockMap;
        }

        public IBlock GetBlock(Vector2Int index)
        {
            if (index.x>=0 && index.x<_blockMap.Length && index.y>=0 && index.y<_blockMap[0].Length)
            {
                return _blockMap[index.x][index.y];
            }

            return null;
        }

        public void RemoveBlockFromMap(Vector2Int index)
        {
            _blockMap[index.x][index.y] = null;
        }

        public void RemoveBlockFromMap(IBlock block)
        {
            foreach (var row in _blockMap)
            {
                for (int j = 0; j < row.Length; j++)
                {
                    if (block == row[j])
                    {
                        row[j] = null;
                    }
                }
            }
        }

        public IBlock FindInteractableBlock(Vector2Int index)
        {
            var block = GetBlock(index);

            switch (block)
            {
                case null:
                    break;
                case ColorBlock:
                {
                    if (IsCanInteractColorBlock(index, block, out var interactableColorBlock)) return interactableColorBlock;

                    break;
                }
                case PowerUpBlock:
                    InteractPowerUpBlock(index, block);
                    return block;
            }

            return null;
        }

        public void AddNewBlock(Vector2Int centerIndex, IBlock powerUp)
        {
            _blockMap[centerIndex.x][centerIndex.y] = powerUp;
        }

        private void InteractPowerUpBlock(Vector2Int index, IBlock block)
        {
            int startOrder = 0;
            _foundedBlockIndexesAndOrders.Add((index, startOrder));
            SearchByPowerUp(index, block, startOrder);
        }

        private void SearchByPowerUp(Vector2Int index, IBlock block, int startOrder)
        {
            RecursiveSearchByPowerUp(index, block, startOrder);
        }

        private void RecursiveSearchByPowerUp(Vector2Int index, IBlock block, int startOrder)
        {
            if (block is BombBlock bombBlock)
            {
                SearchBombTargets(index, startOrder, bombBlock);
            }
            else if (block is RocketBlock rocketBlock)
            {
                var rocketDirectionIsHorizontal = rocketBlock.RocketDirectionIsHorizontal;
                SearchRocketTargets(index, rocketDirectionIsHorizontal, startOrder,rocketBlock );
            }
        }

        
        private void SearchRocketTargets(Vector2Int index, bool rocketDirectionIsHorizontal, int startOrder, IBlock searchBlocks)
        {
            int x, y;
            Vector2Int newIndex;
            int currentOrder = startOrder;
            if (rocketDirectionIsHorizontal)
            {
                for (int i = 1; i < _blockMap[0].Length; i++)
                {
                    bool lookRight = (index.y + i >= 0 && index.y + i < _blockMap[0].Length);
                    bool lookLeft = (index.y - i >= 0 && index.y - i < _blockMap[0].Length);
                    if (lookRight || lookLeft)
                    {
                        if (lookLeft)
                        {
                            x = index.x;
                            y = index.y - i;
                            newIndex = new Vector2Int(x, y);
                            FoundPowerUpTarget(newIndex, currentOrder,searchBlocks);
                        }

                        if (lookRight)
                        {
                            x = index.x;
                            y = index.y + i;
                            newIndex = new Vector2Int(x, y);
                            FoundPowerUpTarget(newIndex, currentOrder,searchBlocks);
                        }

                        currentOrder++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = 1; i < _blockMap.Length; i++)
                {
                    bool lookUp = (index.x - i >= 0 && index.x - i < _blockMap.Length);
                    bool lookDown = (index.x + i >= 0 && index.x + i < _blockMap.Length);

                    if (lookUp || lookDown)
                    {
                        if (lookDown)
                        {
                            x = index.x + i;
                            y = index.y;
                            newIndex = new Vector2Int(x, y);
                            FoundPowerUpTarget(newIndex, currentOrder,searchBlocks);
                        }

                        if (lookUp)
                        {
                            x = index.x - i;
                            y = index.y;
                            newIndex = new Vector2Int(x, y);
                            FoundPowerUpTarget(newIndex, currentOrder,searchBlocks);
                        }

                        currentOrder++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }

        private void FoundPowerUpTarget(Vector2Int index, int currentOrder, IBlock searchPowerUpBlock)
        {
            _foundedBlockIndexesAndOrders.Add((index, currentOrder));
            var block = _blockMap[index.x][index.y];
            if (block?.GetBlockType() == BlockType.BombBlock)
            {
                RecursiveSearchByPowerUp(index, block, currentOrder + 1);
            }
            else if (block is RocketBlock searchRocket)
                if (searchPowerUpBlock is RocketBlock rocketBlock)
                {
                    if (searchRocket.RocketDirectionIsHorizontal != rocketBlock.RocketDirectionIsHorizontal)
                    {
                        RecursiveSearchByPowerUp(index, block, currentOrder + 1);
                    }
                }
                else
                {
                    RecursiveSearchByPowerUp(index, block, currentOrder + 1);
                }
        }

        private void SearchBombTargets(Vector2Int index, int startOrder, IBlock searchBlock)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    var inRowBorders = (index.x + i >= 0 && index.x + i <= _blockMap.Length - 1);
                    var inColumnBorders = (index.y + j >= 0 && index.y + j <= _blockMap[0].Length - 1);
                    if (inRowBorders && inColumnBorders)
                    {
                        FoundPowerUpTarget(new Vector2Int(index.x + i, index.y + j), startOrder,searchBlock);
                    }
                }
            }
        }

        private bool IsCanInteractColorBlock(Vector2Int index, IBlock block, out IBlock interactableBlock)
        {
            interactableBlock = null;
            var blockType = block.GetBlockType();

            SearchBlockInSameTypeNeighbor(blockType, index);
            if (!_foundedBlockIndexesAndOrders.Any((tuple => GetBlock(tuple.Item1)?.GetBlockType() == blockType)))
                return false;
            if (!_foundedBlockIndexesAndOrders.Contains((index, 0)))
            {
                _foundedBlockIndexesAndOrders.Add((index, 0));

            }

            interactableBlock = block;
            return true;

        }

        private void SearchBlockInSameTypeNeighbor(BlockType blockType, Vector2Int searchIndex)
        {
            RecursiveSearchBlockInSameTypeNeighbor(blockType, searchIndex);
        }

        private void RecursiveSearchBlockInSameTypeNeighbor(BlockType blockType, Vector2Int searchIndex)
        {
            int x = searchIndex.x;
            int y = searchIndex.y + 1;
            LookNeighbor(blockType, x, y);
            
            y = searchIndex.y - 1;
            LookNeighbor(blockType, x, y);
            
            x = searchIndex.x + 1;
            y = searchIndex.y;
            LookNeighbor(blockType, x, y);
            
            x = searchIndex.x - 1;
            LookNeighbor(blockType, x, y);
        }

        private void LookNeighbor(BlockType blockType, int x, int y)
        {
            if (!(x < 0 || x >= _blockMap.Length || y < 0 || y >= _blockMap[0].Length))
            {
                Vector2Int index = new Vector2Int(x, y);
                var curBlock = _blockMap[index.x][index.y];
                if (curBlock != null)
                {
                    var hasIndex = _foundedBlockIndexesAndOrders.Any(tuple => tuple.Item1 == index);
                    if (curBlock.GetBlockType() == blockType && !hasIndex)
                    {
                        
                        _foundedBlockIndexesAndOrders.Add((index, 0));
                        RecursiveSearchBlockInSameTypeNeighbor(blockType, index);
                    }
                }
            }
        }

        public ObstacleBlock[] GetObstacleBlockNeighbor(Vector2Int searchIndex)
        {
            List<ObstacleBlock> obstacleBlocks=new List<ObstacleBlock>();
            int x = searchIndex.x;
            int y = searchIndex.y + 1;
            var block=GetBlock(new Vector2Int(x, y));
            if (block is ObstacleBlock obstacleBlock)
            {
                obstacleBlocks.Add(obstacleBlock);
            }
            
            y = searchIndex.y - 1;
            block=GetBlock(new Vector2Int(x, y));
            if (block is ObstacleBlock obstacleBlock1)
            {
                obstacleBlocks.Add(obstacleBlock1);
            }
            
            x = searchIndex.x + 1;
            y = searchIndex.y;
            block=GetBlock(new Vector2Int(x, y));
            if (block is ObstacleBlock obstacleBlock2)
            {
                obstacleBlocks.Add(obstacleBlock2);
            }
            
            x = searchIndex.x - 1;
            block=GetBlock(new Vector2Int(x, y));
            if (block is ObstacleBlock obstacleBlock3)
            {
                obstacleBlocks.Add(obstacleBlock3);
            }

            return obstacleBlocks.ToArray();
        }

        public bool IsShuffleNecessary()
        {
            var blastPossible=IsBlastPossible();
            ResetSearchLists();
            return !blastPossible;
        }

        private bool IsBlastPossible()
        {
            for (int i = 0; i < _blockMap.Length; i++)
            {
                for (int j = 0; j < _blockMap[0].Length; j++)
                {
                    if (_blockMap[i][j] is PowerUpBlock)
                    {
                        return true;
                    }
                    else if (_blockMap[i][j] is ColorBlock)
                    {
                        if (IsCanInteractColorBlock(new Vector2Int(i, j), _blockMap[i][j], out var _))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public IBlock[][] Shuffle()
        {
            var list = _blockMap.SelectMany(blockMapRow => blockMapRow).ToList();

            foreach (var row in _blockMap)
            {
                for (int j = 0; j < row.Length; j++)
                {
                    row[j] = list[Random.Range(0, list.Count - 1)];
                    list.Remove(row[j]);
                }
            }

            return _blockMap;
        }
    }
}