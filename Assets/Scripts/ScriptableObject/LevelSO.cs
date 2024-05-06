using System.Collections.Generic;
using Core.SerializableSetting;
using Enums;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 0)]
    public class LevelSo : UnityEngine.ScriptableObject
    {
        [Header("Goals")] public float time;
        public int moveCount;
        public SerializableDictionary<BlockType, int> goals;
        [Header("GridMap")] public Vector2 cellSize;
        public List<SerializableArrayRow<BlockType>> gridMapBlockSettlement;

        public BlockType[][] GetGridMapBlockSettlement()
        {
            var blockTypeArray = new BlockType[gridMapBlockSettlement.Count][];
            for (int i = 0; i < gridMapBlockSettlement.Count; i++)
            {
                blockTypeArray[i] = new BlockType[gridMapBlockSettlement[i].row.Count];
                for (int j = 0; j < gridMapBlockSettlement[i].row.Count; j++)
                {
                    blockTypeArray[i][j] = gridMapBlockSettlement[i].row[j];
                }
            }
            
            return blockTypeArray;
        }
    }
}