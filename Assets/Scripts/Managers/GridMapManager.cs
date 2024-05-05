using GridSystem;
using UnityEngine;

namespace Managers
{
    public class GridMapManager : MonoBehaviour
    {
        [SerializeField] private GridMapCreator gridMapCreator;
        private CellData[][] _cellDatas;
        private float _cellSizeY;

        public static GridMapManager Instance;

        private void Awake()
        {
            if (Instance==null)
            {
                Instance = this;
            }
        }
        
        public void Initialize(Vector2Int gridMapSize, Vector2 cellSize)
        {
            CreateGridMap(gridMapSize,cellSize);
            _cellSizeY = cellSize.y;
        }

        public void CreateGridMap(Vector2Int gridMapSize, Vector2 cellSize)
        {
            _cellDatas = gridMapCreator.CreateGridMap(gridMapSize,cellSize);
        }

        public bool IsThisPositionAtCellBorders(Vector2 worldPosition)
        {
            var cellIndex=gridMapCreator.GetCellIndex(worldPosition);
            return CheckCellExist(cellIndex);
        }

        public bool IsCellEmpty(Vector2 worldPosition)
        {
            var cellData = GetCellData(worldPosition);
            return cellData.IsEmpty;
        }

        private CellData GetCellData(Vector2 worldPosition)
        {
            Vector2Int cellIndex = gridMapCreator.GetCellIndex(worldPosition);
            var cellData = _cellDatas[cellIndex.x][cellIndex.y];
            return cellData;
        }

        public bool CheckCellExist(Vector2Int cellIndex)
        {
            return (cellIndex.x >= 0 && cellIndex.x < _cellDatas.GetLength(0)) &&
                   (cellIndex.y >= 0 && cellIndex.y < _cellDatas.GetLength(1));
        }

        public Vector2 GetCellPosition(Vector2Int cellIndex)
        {
            return _cellDatas[cellIndex.x][cellIndex.y].Position;
        }
        
        public void FillCell(Vector2Int cellIndex)
        {
            _cellDatas[cellIndex.x][cellIndex.y].FillCell();
        }
        
        public void EmptyCell(Vector2 worldPosition)
        {
            Vector2Int cellIndex = gridMapCreator.GetCellIndex(worldPosition);
            _cellDatas[cellIndex.x][cellIndex.y].EmptyCell();
        }

        public float GetCellScaleY()
        {
            return _cellSizeY;
        }

        public Vector2Int GetCellIndex(Vector2 worldPosition)
        {
            return gridMapCreator.GetCellIndex(worldPosition);
        }

        public (Vector2, Vector2) GetPositionsOfSideOfRow(int rowIndex)
        {
            return (GetCellPosition(new Vector2Int(rowIndex, 0)),
                GetCellPosition(new Vector2Int(rowIndex, _cellDatas.GetLength(1))));
        }

        public (Vector2, Vector2) GetPositionsOfSideOfColumn(int columnIndex)
        {
            return (GetCellPosition(new Vector2Int(0, columnIndex)),
                GetCellPosition(new Vector2Int(_cellDatas.GetLength(0), columnIndex)));
        }
    }
}
