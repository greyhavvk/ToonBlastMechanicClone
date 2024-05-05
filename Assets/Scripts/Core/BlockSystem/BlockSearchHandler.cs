using System.Collections.Generic;
using System.Linq;
using Core.BlockSystem.Block;
using UnityEngine;

namespace Core.BlockSystem
{
    public class BlockSearchHandler : MonoBehaviour
    {
        private IBlock[][] _blockMap;
        
        public static BlockSearchHandler Instance;
        
        private List<(Vector2Int,int)> _foundedBlockIndexesAndOrders;

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
            if (Instance==null)
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
            return _blockMap[index.x][index.y];
        }

        public void RemoveBlockFromMap(Vector2Int index)
        {
            _blockMap[index.x][index.y] = null;
        }
        
        public void RemoveBlockFromMap(IBlock block)
        {
            for (int i = 0; i < _blockMap.GetLength(0); i++)
            {
                for (int j = 0; j < _blockMap.GetLength(1); j++)
                {
                    if (block==_blockMap[i][j])
                    {
                        _blockMap[i][j] = null;
                    } 
                }
            }
        }

        public IBlock FindInteractableBlock(Vector2Int index)
        {
            var block = GetBlock(index);

            switch (block)
            {
                case ColorBlock:
                {
                    if (InteractColorBlock(index, block, out var interactableColorBlock)) return interactableColorBlock;

                    break;
                }
                case PowerUpBlock:
                    InteractPowerUpBlock(index, block);
                   return block;
                    break;
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
            if (block is BombBlock)
            {
                SearchBombTargets(index, startOrder);
            }
            else if (block is RocketBlock rocketBlock)
            {
                var rocketDirectionIsHorizontal = rocketBlock.RocketDirectionIsHorizontal;
                SearchRocketTargets(index, rocketDirectionIsHorizontal, startOrder);
            }
        }

        private void SearchRocketTargets(Vector2Int index, bool rocketDirectionIsHorizontal, int startOrder)
        {
            int x, y;
            int currentOrder = startOrder;
            if (rocketDirectionIsHorizontal)
            {
                for (int i = 1; i < _blockMap.GetLength(1); i++)
                {
                    bool lookRight = (index.y + i >= 0 && index.y + i <= _blockMap.GetLength(1));
                    bool lookLeft = (index.y - i >= 0 && index.y - i <= _blockMap.GetLength(1));
                    if (lookRight || lookLeft)
                    {
                        if (lookLeft)
                        {
                            
                            x = index.x;
                            y = index.y - 1;
                            FoundPowerUpTarget(x, y, currentOrder);
                        }

                        if (lookRight)
                        {
                            x = index.x;
                            y = index.y + 1;
                            FoundPowerUpTarget(x, y, currentOrder);
                        }

                        currentOrder++;
                    }

                    break;
                }
            }
            else
            {
                for (int i = 1; i < _blockMap.GetLength(0); i++)
                {
                    bool lookUp = (index.x - i >= 0 && index.x - i <= _blockMap.GetLength(0));
                    bool lookDown = (index.x + i >= 0 && index.x + i <= _blockMap.GetLength(0));
                    if (lookUp || lookDown)
                    {
                        if (lookDown)
                        {
                            x = index.x - 1;
                            y = index.y;
                            FoundPowerUpTarget(x, y, currentOrder);
                        }

                        if (lookUp)
                        {
                            x = index.x + 1;
                            y = index.y;
                            FoundPowerUpTarget(x, y, currentOrder);
                        }

                        currentOrder++;
                    }

                    break;
                }
            }

        }

        private void FoundPowerUpTarget(int x, int y, int currentOrder)
        {
            var currentIndex = new Vector2Int(x, y);
            _foundedBlockIndexesAndOrders.Add((currentIndex, currentOrder));
            RecursiveSearchBlockInSameTypeNeighbor(BlockType.ObstacleBlock,currentIndex);
            var block = _blockMap[currentIndex.x][currentIndex.y];
            if (block is PowerUpBlock)
            {
                RecursiveSearchByPowerUp(currentIndex, block, currentOrder+1);
            }
        }

        private void SearchBombTargets(Vector2Int index, int startOrder)
        {
            bool inRowBorders;
            bool inColumnBorders;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i==0 && j==0)
                    {
                        continue;
                    }
                    inRowBorders = (index.x + i >= 0 && index.x + i <= _blockMap.GetLength(0));
                    inColumnBorders = (index.y + j >= 0 && index.y + j <= _blockMap.GetLength(1));
                    if (inRowBorders && inColumnBorders)
                    {
                        FoundPowerUpTarget(index.x+i,index.y+j, startOrder);
                    }
                }
            }
        }

        private bool InteractColorBlock(Vector2Int index, IBlock block, out IBlock interactableBlock)
        {
            interactableBlock = null;
            var blockType = block.GetBlockType();
            SearchBlockInSameTypeNeighbor(blockType, index);
            if (_foundedBlockIndexesAndOrders.Any((tuple => GetBlock(tuple.Item1).GetBlockType() == blockType)))
            {
                {
                    _foundedBlockIndexesAndOrders.Add((index,0));
                    interactableBlock = block;
                    return true;
                }
            }

            return false;
        }

        private void SearchBlockInSameTypeNeighbor(BlockType blockType, Vector2Int searchIndex)
        {
            RecursiveSearchBlockInSameTypeNeighbor(blockType, searchIndex);
        }

        private void RecursiveSearchBlockInSameTypeNeighbor(BlockType blockType, Vector2Int searchIndex)
        {
            Vector2Int index;
            IBlock curBlock;
            for (int xOffset = -1; xOffset <= 1; xOffset++)
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    if (xOffset == 0 && yOffset == 0) continue;

                    int x = searchIndex.x + xOffset;
                    int y = searchIndex.y + yOffset;

                    if (x < 0 || x >= _blockMap.GetLength(0) || y < 0 || y >= _blockMap.GetLength(1))
                    {
                        continue;
                    }

                    index = new Vector2Int(x, y);
                    curBlock = _blockMap[index.x][index.y];
                    if (curBlock == null)
                    {
                        continue;
                    }

                    var hasIndex = _foundedBlockIndexesAndOrders.Any(tuple => tuple.Item1 == index);
                    if (curBlock.GetBlockType() == blockType && !hasIndex)
                    {
                        _foundedBlockIndexesAndOrders.Add((index, 0));
                        RecursiveSearchBlockInSameTypeNeighbor(blockType, index);
                    }
                    else if (curBlock is ObstacleBlock)
                    {
                        _foundedBlockIndexesAndOrders.Add((index, 0));
                    }
                }
            }
        }
    }
}