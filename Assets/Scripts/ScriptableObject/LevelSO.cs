using Core.BlockSystem.Block;
using SerializableSetting;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 0)]
    public class LevelSo : UnityEngine.ScriptableObject
    {
        [Header("Goals")]
        public float time;
        public int moveCount;
        public SerializableDictionary<BlockType, int> goal;
        [Header("GridMap")]
        public Vector2 cellSize;
        public Serializable2DArray<BlockType> gridMapBlockSettlement;
    }
}