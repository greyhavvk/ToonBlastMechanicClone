using System;
using System.Collections.Generic;
using System.Linq;
using Core.BlockSystem;
using Core.BlockSystem.Block;
using Core.TrackerSystem;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

namespace Managers
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private int minBlastForMakingBomb;
        [SerializeField] private int minBlastForMakingRocket;

        public Action OnBlocksSettled;
        public Action OnBlocksMoving;
        public Action<ParticleType, Vector2> OnPlayParticle;
        private const int TimePerOrderInMs = 100;
        private const float TimePerOrderInSecond= .1f;

        public void Initialize(BlockType[][] getGridMapBlockSettlement)
        {
            BlockPlacementHandler.Instance.OnFallEnded += FallEnded;
            BlockPlacementHandler.Instance.OnMoveToCenterAnimationEnded += MoveToCenterAnimationComplete;

            BlockFillingHandler.Instance.Initialize(OnPlayParticle);
            var generatedBlocks = BlockFillingHandler.Instance.SpawnRequestedBlocksByLevel(getGridMapBlockSettlement);
            BlockPlacementHandler.Instance.PlaceInitialBlocks(generatedBlocks);
            BlockSearchHandler.Instance.SetBlockMap(generatedBlocks);
            CheckShuffleNecessary();


        }

        private void OnDestroy()
        {
            BlockPlacementHandler.Instance.OnFallEnded -= FallEnded;
            BlockPlacementHandler.Instance.OnMoveToCenterAnimationEnded -= MoveToCenterAnimationComplete;
        }

        public void TouchDetected(Vector2 worldPosition)
        {
            if (GridMapManager.Instance.IsThisPositionAtCellBorders(worldPosition))
            {
                
                var cellIndex = GridMapManager.Instance.GetCellIndex(worldPosition);
                var interactedBlock = BlockSearchHandler.Instance.FindInteractableBlock(cellIndex);
                if (interactedBlock != null)
                {
                    OnBlocksMoving?.Invoke();
                    MoveCountTracker.Instance.DetectMove();
                    ActivateInteractedBlock(cellIndex, interactedBlock);
                }
            }
        }

        private void MoveToCenterAnimationComplete(List<IBlock> blocksThatBlasted, Vector2Int centerIndex)
        {
            foreach (var blockBlasted in blocksThatBlasted)
            {
                BlockSearchHandler.Instance.RemoveBlockFromMap(blockBlasted);
                blockBlasted.BlastBlock();
            }

            var blockMap = BlockSearchHandler.Instance.BlockMap;
            MakePowerUp(blocksThatBlasted, centerIndex);
            FallBlocks(blockMap);
        }

        private static void FallBlocks(IBlock[][] blockMap)
        {
            var (columnIndexAndFillingBlocks, newMap) = BlockFillingHandler.Instance.UpdateBlockMap(blockMap);
            BlockSearchHandler.Instance.SetBlockMap(newMap);
            foreach (var columnIndexAndFillingBlock in columnIndexAndFillingBlocks)
            {
                BlockPlacementHandler.Instance.PlaceFillingBlock(columnIndexAndFillingBlock.Item2,
                    columnIndexAndFillingBlock.Item1);
            }
            
            BlockPlacementHandler.Instance.RepositionBlocks(newMap);
            BlockSearchHandler.Instance.ResetSearchLists();
        }

        private void MakePowerUp(List<IBlock> blocksThatBlasted, Vector2Int centerIndex)
        {
            IBlock powerUp = null;
            if (blocksThatBlasted.Count > minBlastForMakingBomb)
            {
                powerUp = BlockFillingHandler.Instance.SpawnBlock(BlockType.BombBlock);
            }
            else if (blocksThatBlasted.Count > minBlastForMakingRocket)
            {
                powerUp = BlockFillingHandler.Instance.SpawnBlock(BlockType.RocketBlock);
            }

            if (powerUp != null)
            {
                BlockPlacementHandler.Instance.PlaceBlock(centerIndex, powerUp);
                BlockSearchHandler.Instance.AddNewBlock(centerIndex, powerUp);
            }
        }

        private void ActivateInteractedBlock(Vector2Int cellIndex, IBlock interactedBlock)
        {
            if (interactedBlock is ColorBlock)
            {
                BlastColorBlocks(interactedBlock, cellIndex);
            }
            else if (interactedBlock is PowerUpBlock)
            {
                BlastPowerUp();
            }
        }

        private async void BlastPowerUp()
        {
            int lastOrderNumber = 0;
            var blastInOrderList = BlockSearchHandler.Instance.FoundedBlockIndexesAndOrders;
            foreach (var blastOrder in blastInOrderList)
            {
                var block = BlockSearchHandler.Instance.GetBlock(blastOrder.Item1);
                var delay = (blastOrder.Item2) * TimePerOrderInMs;
                switch (block)
                {
                    case null:
                        continue;
                    case RocketBlock rocketBlock:
                    {
                        var targets = rocketBlock.RocketDirectionIsHorizontal
                            ? BlockPlacementHandler.Instance.GetPositionsOfSideOfRow(blastOrder.Item1.x)
                            : BlockPlacementHandler.Instance.GetPositionsOfSideOfColumn(blastOrder.Item1.y);
                        var speed = BlockPlacementHandler.Instance.GetDistanceBetweenBlocks()*((blastOrder.Item2)+1) / TimePerOrderInSecond;
                        rocketBlock.SetRocketHeads(targets, speed);
                        block.DelayedBlastBlock((blastOrder.Item2) * TimePerOrderInMs);
                        lastOrderNumber = Mathf.Max(lastOrderNumber, blastOrder.Item2);
                        continue;
                    }
                    default:
                        block.DelayedBlastBlock((blastOrder.Item2) * TimePerOrderInMs);
                        ReduceObstacleHitPoint(blastOrder.Item1,delay);
                        lastOrderNumber = Mathf.Max(lastOrderNumber, blastOrder.Item2);
                        continue;
                        
                }
            }

            await UniTask.Delay(lastOrderNumber * TimePerOrderInMs);
            await UniTask.DelayFrame(1);

            foreach (var blockBlasted in blastInOrderList)
            {
                BlockSearchHandler.Instance.RemoveBlockFromMap(blockBlasted.Item1);
                BlockPlacementHandler.Instance.RemoveBlockOnGridMap(blockBlasted.Item1);
            }

            var blockMap = BlockSearchHandler.Instance.BlockMap;
            FallBlocks(blockMap);
        }

        private void BlastColorBlocks(IBlock interactedBlock, Vector2Int cellIndex)
        {
            var foundedColorBlocksIndexesAndOrders = BlockSearchHandler.Instance.FoundedBlockIndexesAndOrders.FindAll(
                blockIndexAndOrder =>
                    BlockSearchHandler.Instance.GetBlock(blockIndexAndOrder.Item1) is ColorBlock);
            var blocksThatWillBlast = foundedColorBlocksIndexesAndOrders.Select(blockIndexAndOrder =>
                BlockSearchHandler.Instance.GetBlock(blockIndexAndOrder.Item1)).ToList();
            var blockThatWillBlastIndexes = foundedColorBlocksIndexesAndOrders.Select(blockIndexAndOrder =>
                blockIndexAndOrder.Item1).ToList();
            var centerPosition = interactedBlock.GetTransform().position;
            foreach (var foundedColorBlocksIndexAndOrder in foundedColorBlocksIndexesAndOrders)
            {
                BlockSearchHandler.Instance.RemoveBlockFromMap(foundedColorBlocksIndexAndOrder.Item1);
            }

            BlockPlacementHandler.Instance.MoveBlocksToCenter(centerPosition, blocksThatWillBlast, cellIndex);
            ReduceObstaclesHitPoint(blockThatWillBlastIndexes);
        }

        private void ReduceObstaclesHitPoint(List<Vector2Int> blockThatWillBlastIndexes)
        {
            foreach (var blockThatWillBlastIndex in blockThatWillBlastIndexes)
            {
                ReduceObstacleHitPoint(blockThatWillBlastIndex);
            }
        }

        private async void ReduceObstacleHitPoint(Vector2Int blockThatWillBlastIndex,int delay=0)
        {
            var obstacles = BlockSearchHandler.Instance.GetObstacleBlockNeighbor(blockThatWillBlastIndex);
            foreach (var obstacle in obstacles)
            {
                if (obstacle)
                {
                    obstacle.ReduceHitPoint();
                    if (!obstacle.IsBroken) continue;
                    BlockSearchHandler.Instance.RemoveBlockFromMap(obstacle);
                    BlockPlacementHandler.Instance.RemoveBlockOnGridMap(obstacle.GetTransform().position);
                    obstacle.BlastBlock();
                }
            }
            await UniTask.Delay(delay);
        }

        private void CheckShuffleNecessary()
        {
            if (BlockSearchHandler.Instance.IsShuffleNecessary())
            {
                OnBlocksMoving?.Invoke();
                var shuffledMap = BlockSearchHandler.Instance.Shuffle();
                BlockPlacementHandler.Instance.RepositionBlocks(shuffledMap);
            }
            else
            {
                OnBlocksSettled?.Invoke();
                BlockSearchHandler.Instance.ResetSearchLists();
            }
                
        }
        
        private void FallEnded()
        {
            CheckShuffleNecessary();
        }
    }
}