using System;
using System.Collections.Generic;
using Core.BlockSystem.Block;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Core.BlockSystem
{
    public class BlockMovementAndPositionHandler : MonoBehaviour
    {
        public static BlockMovementAndPositionHandler Instance;
        public Action OnFallEnded;
        public Action<List<IBlock>> OnBlastInCenterAnimationEnded;
        private const float FallSpeed = 5f;
        private const float MoveCenterTime = .5f;
        private const float SpawnFallingAbovePositionY=3;

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

        public void PlaceInitialBlocks(IBlock[][] blocks)
        {
            for (int i = 0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    var blockPositionOnGrid = new Vector2Int(i,j);
                    if (GridMapManager.Instance.CheckCellExist(blockPositionOnGrid))
                    {
                        blocks[i][j].GetTransform().position = GridMapManager.Instance.GetCellPosition(blockPositionOnGrid);
                        GridMapManager.Instance.FillCell(blockPositionOnGrid);
                    } 
                }
            }
        }

        public async void RepositionBlocks(IBlock[][] blocks)
        {
            List<UniTask> taskList = new List<UniTask>();
            
            for (int i = 0; i < blocks.GetLength(0); i++)
            {
                for (int j = 0; j < blocks.GetLength(1); j++)
                {
                    var blockPositionOnGrid = new Vector2Int(i,j);
                    if (GridMapManager.Instance.CheckCellExist(blockPositionOnGrid))
                    {
                        var blockPosition = blocks[i][j].GetTransform().position;
                        RemoveBlockOnGridMap(blockPosition);
                        var destinationY = GridMapManager.Instance.GetCellPosition(blockPositionOnGrid).y;
                        var time = Mathf.Abs(destinationY - blockPosition.y) / FallSpeed;
                        var tween = blocks[i][j].GetTransform().DOMoveY(destinationY, time).SetEase(Ease.OutBounce);
                        taskList.Add(tween.ToUniTask());
                        GridMapManager.Instance.FillCell(blockPositionOnGrid);
                    }
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

        public void PlaceFillingBlock(IBlock[] fillingBlocks, int columnIndex)
        {
            var headCellIndexOfColumn = new Vector2Int(0, columnIndex);
            if (GridMapManager.Instance.CheckCellExist(headCellIndexOfColumn))
            {
                var refCellPosition = GridMapManager.Instance.GetCellPosition(headCellIndexOfColumn);
                refCellPosition.y += SpawnFallingAbovePositionY;
                for (int i = fillingBlocks.Length - 1; i >= 0; i--)
                {
                    fillingBlocks[i].GetTransform().position = refCellPosition;
                    refCellPosition.y += GridMapManager.Instance.GetCellScaleY();
                }
            }
        }
        
        public async void BlastInCenter(Vector3 centerPosition, List<IBlock> blocksThatWillBlast)
        {
            List<UniTask> taskList = new List<UniTask>();

            foreach (var blockThatWillBlast in blocksThatWillBlast)
            {
                RemoveBlockOnGridMap(blockThatWillBlast.GetTransform().position);
                var tween = blockThatWillBlast.GetTransform().DOMove(centerPosition, MoveCenterTime)
                    .SetEase(Ease.Linear).OnComplete((() =>
                    {
                        blockThatWillBlast.BlastBlock();
                    }));
                taskList.Add(tween.ToUniTask());
            }
            
            await UniTask.WhenAll(taskList);
            OnBlastInCenterAnimationEnded?.Invoke(blocksThatWillBlast);
        }
    }
}