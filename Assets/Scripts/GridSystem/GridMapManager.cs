using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridMapManager : MonoBehaviour
    {
        [SerializeField] private GridMapCreator gridMapCreator;
        private Dictionary<Vector2, CellData> _cellDataDictionary;

        public static GridMapManager Instance;

        private void Awake()
        {
            if (Instance==null)
            {
                Instance = this;
            }
            FillCellDataDictionary();
        }

        private void FillCellDataDictionary()
        {
            _cellDataDictionary = gridMapCreator.CreateGridMap();
        }

        public bool IsThisPositionAtCellBorders(Vector2 worldPosition)
        {
            var cellIndex=gridMapCreator.GetCellIndex(worldPosition);
            return _cellDataDictionary.ContainsKey(cellIndex);
        }

        public GameObject GetHoldingObject(Vector2 worldPosition)
        {
            var cellData = GetCellData(worldPosition);
            return cellData.HoldingObject;
        }

        public bool IsCellEmpty(Vector2 worldPosition)
        {
            var cellData = GetCellData(worldPosition);
            return cellData.IsEmpty;
        }

        private CellData GetCellData(Vector2 worldPosition)
        {
            Vector2 cellIndex = gridMapCreator.GetCellIndex(worldPosition);
            var cellData = _cellDataDictionary[cellIndex];
            return cellData;
        }
    }
}
