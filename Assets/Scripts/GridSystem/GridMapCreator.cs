using System.Collections.Generic;
using GridSystem;
using UnityEngine;

public class GridMapCreator : MonoBehaviour
{
    [SerializeField] private Transform gridMapStartPointer;
    [SerializeField] private Transform gridFrame;
    [SerializeField] private GridMapSO gridMapSo;
    
    public Dictionary<Vector2, CellData> CreateGridMap()
    {
        var cellDataDictionary= FillCellDataDictionary();

        SetGridFrameSize();
        
        return cellDataDictionary;
    }

    private void SetGridFrameSize()
    {
        Vector2 gridFrameSize = new Vector2(gridMapSo.cellSize.x * gridMapSo.gridMapSize.x, gridMapSo.cellSize.y * gridMapSo.gridMapSize.y);
        //TODO çerçeve boyut ve gerekli diğer ayarlamalar halledilecek.
    }

    private Dictionary<Vector2, CellData> FillCellDataDictionary()
    {
        Dictionary<Vector2, CellData> cellDataDictionary= new Dictionary<Vector2, CellData>();
        for (int i = 0; i < gridMapSo.gridMapSize.x; i++)
        {
            for (int j = 0; j < gridMapSo.gridMapSize.y; j++)
            {
                Vector2 position = (Vector2)gridMapStartPointer.position +
                                   new Vector2(i * gridMapSo.cellSize.x, j * gridMapSo.cellSize.y * (-1));
                var cell = new CellData(position);
                cellDataDictionary.Add(new Vector2(i, j), cell);
            }
        }
        return cellDataDictionary;
    }

    public Vector2 GetCellIndex(Vector2 worldPosition)
    {
        var localPosition= worldPosition - (Vector2)gridMapStartPointer.position;
        var x = Mathf.FloorToInt((localPosition.x / gridMapSo.cellSize.x) + gridMapSo.cellSize.x * .5f);
        var y = -1*Mathf.FloorToInt((localPosition.y / gridMapSo.cellSize.y) + gridMapSo.cellSize.y * .5f);
        return new Vector2(x, y);
    }
}
