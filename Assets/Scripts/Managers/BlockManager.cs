using System;
using BlockSystem;
using BlockSystem.Block;
using Core.BlockSystem;
using Core.BlockSystem.Block;
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
           
            var generatedBlocks = BlockFillingHandler.Instance.SpawnRequestedBlocksByLevel(getGridMapBlockSettlement);
            BlockMovementAndPositionHandler.Instance.PlaceInitialBlocks(generatedBlocks);
            BlockSearchHandler.Instance.SetBlockMap(generatedBlocks);
        }

        private void OnDestroy()
        {
            BlockMovementAndPositionHandler.Instance.OnFallEnded -= FallEnded;
        }

        public void TouchDetected(Vector2 worldPosition)
        {
            throw new NotImplementedException();
        }

        private void FallEnded()
        {
            OnBlocksSettled?.Invoke(); 
        }
    }
}