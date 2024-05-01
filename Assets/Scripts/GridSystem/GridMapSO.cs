using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "GridMap", menuName = "GridSystem", order = 0)]
    public class GridMapSO : ScriptableObject
    {
        public Vector2 cellSize;
        public Vector2 gridMapSize;
    }
}