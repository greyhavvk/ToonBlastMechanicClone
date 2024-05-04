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
        private List<Vector2Int> _neighborCoordinates;
        private List<(Vector2Int,int)> _foundedObstacleCoordinatesAndInteractCounts;
        public IBlock[][] BlockMap => _blockMap;
        private void Awake()
        {
            SetInstance();
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

        private IBlock GetBlock(Vector2Int index)
        {
            return _blockMap[index.x][index.y];
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

        public void UpdateBlockMapColumnForNewBlock(IBlock[] fillingBlocks, int columnIndex)
        {
            for (int i = 0; i < fillingBlocks.Length; i++)
            {
                _blockMap[i][columnIndex] = fillingBlocks[i];
            }
        }

        public List<(ObstacleBlock, int)> GetObstacleThatInteractedAndInteractedCount()
        {
            return _foundedObstacleCoordinatesAndInteractCounts.Select(foundedObstacleCoordinate =>
                (_blockMap[foundedObstacleCoordinate.Item1.x][foundedObstacleCoordinate.Item1.y] as ObstacleBlock,
                    foundedObstacleCoordinate.Item2)).ToList();
        }
        
        public List<IBlock> GetNeighbor()
        {
            var neighbor=_neighborCoordinates.Select(neighborCoordinate =>
                _blockMap[neighborCoordinate.x][neighborCoordinate.y]).ToList();
            foreach (var neighborCoordinate in _neighborCoordinates)
            {
                _blockMap[neighborCoordinate.x][neighborCoordinate.y] = null;
            }
            return neighbor;
        }
        

        public IBlock FindInteractableBlock(Vector2Int index)
        {
            var block = GetBlock(index);

            switch (block)
            {
                case ColorBlock:
                {
                    var blockType = block.GetBlockType();
                    SearchNeighborCoordinates(blockType, index);
                    if (_neighborCoordinates.Count>0)
                    {
                        return block;
                    }

                    break;
                }
                case PowerUpBlock:
                    return block;
            }
            return null;
        }

        private void SearchNeighborCoordinates(BlockType blockType, Vector2Int searchIndex)
        {
            _neighborCoordinates = new List<Vector2Int>{searchIndex};
            _foundedObstacleCoordinatesAndInteractCounts = new List<(Vector2Int, int)>();
            
            RecursiveSearchNeighbours(blockType, searchIndex);
        }

        private void RecursiveSearchNeighbours(BlockType blockType, Vector2Int searchIndex)
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
                    if (curBlock==null)
                    {
                        continue;
                    }
                    if (curBlock.GetBlockType() == blockType && !_neighborCoordinates.Contains(index))
                    {
                        _neighborCoordinates.Add(index);
                        RecursiveSearchNeighbours(blockType, index);
                    }
                    else if (curBlock is ObstacleBlock)
                        if (_foundedObstacleCoordinatesAndInteractCounts.Any(foundedObstacle=>foundedObstacle.Item1==index))
                        {
                            var foundedObstacleCoordinateAndInteractCountIndex =
                                _foundedObstacleCoordinatesAndInteractCounts.FindIndex(foundedObstacle =>
                                    foundedObstacle.Item1 == index);
                            _foundedObstacleCoordinatesAndInteractCounts
                                [foundedObstacleCoordinateAndInteractCountIndex] = (index,
                                _foundedObstacleCoordinatesAndInteractCounts[
                                    foundedObstacleCoordinateAndInteractCountIndex].Item2 + 1);
                        }
                        else
                        {
                            _foundedObstacleCoordinatesAndInteractCounts.Add((index,1));
                        }
                }
            }
        }

    }
}