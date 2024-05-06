using System;
using System.Collections.Generic;
using Core.BlockSystem.Block;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Core.BlockSystem
{
    public class BlockPlacementHandler : MonoBehaviour
    {
        public static BlockPlacementHandler Instance;
        public Action OnFallEnded;
        public Action<List<IBlock>, Vector2Int> OnMoveToCenterAnimationEnded;
        private const float FallSpeed = 5f;
        private const float MoveCenterTime = .2f;
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
            for (int i = 0; i < blocks.Length; i++)
            {
                for (int j = 0; j < blocks[0].Length; j++)
                {
                    PlaceBlock(new Vector2Int(i,j), blocks[i][j]);
                }
            }
        }

        public async void RepositionBlocks(IBlock[][] blocks)
        {
            List<UniTask> taskList = new List<UniTask>();
            
            for (int i = 0; i < blocks.Length; i++)
            {
                for (int j = 0; j < blocks[0].Length; j++)
                {
                    var blockPositionOnGrid = new Vector2Int(i,j);
                    if (GridMapManager.Instance.CheckCellExist(blockPositionOnGrid) && blocks[i][j]!=null)
                    {
                        var blockPosition = blocks[i][j].GetTransform().position;
                        RemoveBlockOnGridMap(blockPosition);
                        var destination = GridMapManager.Instance.GetCellPosition(blockPositionOnGrid);
                        var time = Mathf.Abs(destination.y - blockPosition.y) / FallSpeed;
                        var tween = blocks[i][j].GetTransform().DOMove(destination, time).SetEase(Ease.OutBounce);
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
        
        public async void MoveBlocksToCenter(Vector2 centerPosition, List<IBlock> blocksThatWillBlast, Vector2Int centerIndex)
        {
            List<UniTask> taskList = new List<UniTask>();

            foreach (var blockThatWillBlast in blocksThatWillBlast)
            {
                RemoveBlockOnGridMap(blockThatWillBlast.GetTransform().position);
                var tween = blockThatWillBlast.GetTransform().DOMove(centerPosition, MoveCenterTime)
                    .SetEase(Ease.Linear);
                taskList.Add(tween.ToUniTask());
            }
            
            await UniTask.WhenAll(taskList);
            OnMoveToCenterAnimationEnded?.Invoke(blocksThatWillBlast, centerIndex);
        }

        public void PlaceBlock(Vector2Int centerIndex, IBlock block)
        {
            if (GridMapManager.Instance.CheckCellExist(centerIndex))
            {
                block.GetTransform().position = GridMapManager.Instance.GetCellPosition(centerIndex);
                GridMapManager.Instance.FillCell(centerIndex);
            } 
        }

        public (Vector2,Vector2) GetPositionsOfSideOfRow(int rowIndex)
        {
            return GridMapManager.Instance.GetPositionsOfSideOfRow(rowIndex);
        }
        
        public (Vector2,Vector2) GetPositionsOfSideOfColumn(int columnIndex)
        {
            return GridMapManager.Instance.GetPositionsOfSideOfColumn(columnIndex);
        }

        public float GetDistanceBetweenBlocks()
        {
            return GridMapManager.Instance.GetCellScaleY();
        }
    }
}