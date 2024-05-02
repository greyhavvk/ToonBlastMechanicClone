using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GridSystem;
using UnityEngine;

namespace BlockSystem
{
    public class GridCellBlockHandler : MonoBehaviour
    {
        public static GridCellBlockHandler Instance;
        public Action OnFallEnded;
        private const float FallSpeed = 5f;


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
        
        public void PlaceInitialBlocks(Dictionary<Transform, BlockData> blocks)
        {
            foreach (var block in blocks)
            {
                var blockPositionOnGrid = block.Value.blockPositionOnGrid;
                if (GridMapManager.Instance.CheckCellExist(blockPositionOnGrid))
                {
                    block.Key.position = GridMapManager.Instance.GetCellPosition(blockPositionOnGrid);
                    GridMapManager.Instance.FillCell(blockPositionOnGrid, block.Key.gameObject);
                }
            }
        }

        public async void ReplaceBlocks(Dictionary<Transform, BlockData> blocks)
        {

            List<UniTask> taskList = new List<UniTask>();
            foreach (var block in blocks)
            {
                var blockPositionOnGrid = block.Value.blockPositionOnGrid;
                if (GridMapManager.Instance.CheckCellExist(blockPositionOnGrid))
                {
                    var blockPosition = block.Key.position;
                    RemoveBlockOnGridMap(blockPosition);
                    var destinationY = GridMapManager.Instance.GetCellPosition(blockPositionOnGrid).y;
                    var time = Mathf.Abs(destinationY - blockPosition.y) / FallSpeed;
                    var tween = block.Key.DOMoveY(destinationY, time).SetEase(Ease.OutBounce);
                    taskList.Add(tween.ToUniTask());
                    GridMapManager.Instance.FillCell(blockPositionOnGrid, block.Key.gameObject);
                }
            }

            await UniTask.WhenAll(taskList);
            OnFallEnded?.Invoke();
        }

        public void RemoveBlockOnGridMap(Vector2 blockPosition)
        {
            if (GridMapManager.Instance.IsThisPositionAtCellBorders(blockPosition))
            {
                GridMapManager.Instance.EmptyCell(blockPosition);
            }
        }
    }
}