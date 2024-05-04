using System.Collections.Generic;
using System.Linq;
using BlockSystem.Block;
using Core.BlockSystem.Block;
using UnityEngine;

namespace Core.BlockSystem
{
    public class BlockSearchHandler : MonoBehaviour
    {
        private IBlock[][] _blockMap;
        
        public static BlockSearchHandler Instance;
        private List<Vector2Int> _neighborCoordinates;
        private List<Vector2Int> _foundedObstacleCoordinates;
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
        
        private void RemoveBlockFromMap(Vector2Int index)
        {
            _blockMap[index.x][index.y] = null;
        }

        public void UpdateBlockMapColumnForNewBlock(IBlock[] fillingBlocks, int columnIndex)
        {
            for (int i = 0; i < fillingBlocks.Length; i++)
            {
                _blockMap[i][columnIndex] = fillingBlocks[i];
            }
        }

        public List<ObstacleBlock> GetObstacleThatInteracted()
        {
            return _foundedObstacleCoordinates.Select(foundedObstacleCoordinate =>
                _blockMap[foundedObstacleCoordinate.x][foundedObstacleCoordinate.y] as ObstacleBlock).ToList();
        }
        
        public List<IBlock> GetNeighbor()
        {
            return _neighborCoordinates.Select(neighborCoordinate =>
                _blockMap[neighborCoordinate.x][neighborCoordinate.y]).ToList();
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
                    break;
            }
            return null;
        }

        private void SearchNeighborCoordinates(BlockType blockType, Vector2Int searchIndex)
        {
            _neighborCoordinates = new List<Vector2Int>();
            _foundedObstacleCoordinates = new List<Vector2Int>();
            
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

                    if (curBlock.GetBlockType() == blockType && !_neighborCoordinates.Contains(index))
                    {
                        _neighborCoordinates.Add(index);
                        RecursiveSearchNeighbours(blockType, index);
                    }
                    else if (curBlock is ObstacleBlock && !_foundedObstacleCoordinates.Contains(index))
                    {
                        _foundedObstacleCoordinates.Add(index);
                    }
                }
            }
        }

    }
}