using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class GridMapCreator : MonoBehaviour
    {
        [SerializeField] private RectTransform gridFrame;
        [SerializeField] private GridMapSO gridMapSo;

        private const float FrameMargin = .1f;
        private Vector2 _gridMapStartPoint;
    
        public Dictionary<Vector2Int, CellData> CreateGridMap()
        {
            SetGridFrame();
            var cellDataDictionary= FillCellDataDictionary();
        
            return cellDataDictionary;
        }

        private void SetGridFrame()
        {
            _gridMapStartPoint = new Vector2(gridMapSo.cellSize.x / 2 * (gridMapSo.gridMapSize.x + 1),
                gridMapSo.cellSize.y / 2 * (gridMapSo.gridMapSize.y + 1));
            Vector2 gridFrameSize = new Vector2(gridMapSo.cellSize.x * gridMapSo.gridMapSize.x+ FrameMargin, gridMapSo.cellSize.y * gridMapSo.gridMapSize.y+FrameMargin);
            gridFrame.sizeDelta = gridFrameSize;
        }

        private Dictionary<Vector2Int, CellData> FillCellDataDictionary()
        {
            Dictionary<Vector2Int, CellData> cellDataDictionary= new Dictionary<Vector2Int, CellData>();
            for (int i = 0; i < gridMapSo.gridMapSize.x; i++)
            {
                for (int j = 0; j < gridMapSo.gridMapSize.y; j++)
                {
                    Vector2 position = _gridMapStartPoint +
                                       new Vector2(i * gridMapSo.cellSize.x, j * gridMapSo.cellSize.y * (-1));
                    var cell = new CellData(position);
                    cellDataDictionary.Add(new Vector2Int(i, j), cell);
                }
            }
            return cellDataDictionary;
        }

        public Vector2Int GetCellIndex(Vector2 worldPosition)
        {
            var localPosition= worldPosition - _gridMapStartPoint;
            var x = Mathf.FloorToInt((localPosition.x / gridMapSo.cellSize.x) + gridMapSo.cellSize.x * .5f);
            var y = -1*Mathf.FloorToInt((localPosition.y / gridMapSo.cellSize.y) + gridMapSo.cellSize.y * .5f);
            return new Vector2Int(x, y);
        }
    }
}
