using UnityEngine;

namespace GridSystem
{
    public class GridMapCreator : MonoBehaviour
    {
        [SerializeField] private RectTransform gridFrame;

        private const float FrameMargin = .2f;
        private Vector2 _gridMapStartPoint;
        private Vector2 _cellSize;
    
        public CellData[][] CreateGridMap(Vector2Int gridMapSize, Vector2 cellSize)
        {
            _cellSize = cellSize;
            SetGridFrame(gridMapSize);
            var cellDatas= FillCellDatas(gridMapSize);
        
            return cellDatas;
        }

        private void SetGridFrame(Vector2Int gridMapSize)
        {
            _gridMapStartPoint = new Vector2(_cellSize.x / 2 * (gridMapSize.y-1)*(-1),
                _cellSize.y / 2 * (gridMapSize.x-1));
            Vector2 gridFrameSize = new Vector2(_cellSize.x * gridMapSize.y + FrameMargin,
                _cellSize.y * gridMapSize.x + FrameMargin);
            gridFrame.sizeDelta = gridFrameSize;
        }

        private CellData[][] FillCellDatas(Vector2Int gridMapSize)
        {
            CellData[][] cellDatas= new CellData[gridMapSize.x][];
            for (int i = 0; i < gridMapSize.x; i++)
            {
                cellDatas[i]= new CellData[gridMapSize.y];
                for (int j = 0; j < gridMapSize.y; j++)
                {
                    Vector2 position = _gridMapStartPoint +
                                       new Vector2(j * _cellSize.x, i * _cellSize.y * (-1));
                    var cell = new CellData(position);
                    cellDatas[i][j]=cell;
                }
            }
            return cellDatas;
        }

        public Vector2Int GetCellIndex(Vector2 worldPosition)
        {
            var localPosition= worldPosition - _gridMapStartPoint;
            var x = -1*Mathf.FloorToInt((localPosition.y / _cellSize.x) + _cellSize.x * .5f);
            var y = Mathf.FloorToInt((localPosition.x / _cellSize.y) + _cellSize.y * .5f);
            return new Vector2Int(x, y);
        }
    }
}
