using System;
using System.Collections.Generic;
using BlockSystem;
using Core.BlockSystem;
using Core.BlockSystem.Block;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public class BlockManager : MonoBehaviour
    {
        public Action OnBlocksSettled;
        public Action OnBlocksMoving;

        public void Initialize(BlockType[][] getGridMapBlockSettlement)
        {
            BlockMovementAndPositionHandler.Instance.OnFallEnded += FallEnded;
            BlockMovementAndPositionHandler.Instance.OnBlastInCenterAnimationEnded += BlastInCenterAnimationComplete;
           
            var generatedBlocks = BlockFillingHandler.Instance.SpawnRequestedBlocksByLevel(getGridMapBlockSettlement);
            BlockMovementAndPositionHandler.Instance.PlaceInitialBlocks(generatedBlocks);
            BlockSearchHandler.Instance.SetBlockMap(generatedBlocks);
        }

        private void OnDestroy()
        {
            BlockMovementAndPositionHandler.Instance.OnFallEnded -= FallEnded;
            BlockMovementAndPositionHandler.Instance.OnBlastInCenterAnimationEnded -= BlastInCenterAnimationComplete;
        }

        public void TouchDetected(Vector2 worldPosition)
        {
            if (GridMapManager.Instance.IsThisPositionAtCellBorders(worldPosition))
            {
                var cellIndex = GridMapManager.Instance.GetCellIndex(worldPosition);
                var interactedBlock = BlockSearchHandler.Instance.FindInteractableBlock(cellIndex);
                if (interactedBlock!=null)
                {
                    OnBlocksMoving?.Invoke();
                    ActivateInteractedBlock(cellIndex,interactedBlock);
                }
            }
        }

        private void BlastInCenterAnimationComplete(List<IBlock> blocksThatBlasted)
        {
            foreach (var blockBlasted in blocksThatBlasted)
            {
               BlockSearchHandler.Instance.RemoveBlockFromMap(blockBlasted);
            }

            var blockMap = BlockSearchHandler.Instance.BlockMap;
            var (columnIndexAndFillingBlocks, newMap)=BlockFillingHandler.Instance.UpdateBlockMap(blockMap);
            BlockSearchHandler.Instance.SetBlockMap(blockMap);
            foreach (var columnIndexAndFillingBlock in columnIndexAndFillingBlocks)
            {
                BlockMovementAndPositionHandler.Instance.PlaceFillingBlock(columnIndexAndFillingBlock.Item2,
                    columnIndexAndFillingBlock.Item1);
            }

            BlockMovementAndPositionHandler.Instance.RepositionBlocks(newMap);
        }

        private void ActivateInteractedBlock(Vector2Int cellIndex, IBlock interactedBlock)
        {
            if (interactedBlock is ColorBlock)
            {
                List<IBlock> blocksThatWillBlast = BlockSearchHandler.Instance.GetNeighbor();
                var centerPosition = interactedBlock.GetTransform().position;
                BlockMovementAndPositionHandler.Instance.BlastInCenter(centerPosition, blocksThatWillBlast);
                ReduceObstaclesHitPoint();
            }
            else if (interactedBlock is PowerUpBlock)
            {
                //TODO şimdi bunlara el atılacak.
            }
        }

        private void ReduceObstaclesHitPoint()
        {
            var obstacleAndInteractedCountList =
                BlockSearchHandler.Instance.GetObstacleThatInteractedAndInteractedCount();
            foreach (var obstacleAndInteractedCount in obstacleAndInteractedCountList)
            {
                var obstacle = obstacleAndInteractedCount.Item1;
                var count = obstacleAndInteractedCount.Item2;
                obstacle.ReduceHitPoint(count);
                if (!obstacle.IsBroken) continue;
                BlockSearchHandler.Instance.RemoveBlockFromMap(obstacle);
                BlockMovementAndPositionHandler.Instance.RemoveBlockOnGridMap(obstacle.GetTransform().position);
                obstacle.BlastBlock();
            }
        }

        private void FallEnded()
        {
            OnBlocksSettled?.Invoke(); 
        }
    }
}