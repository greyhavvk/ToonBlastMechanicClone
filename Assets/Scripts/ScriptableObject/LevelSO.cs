using BlockSystem.Block;
using Core.BlockSystem.Block;
using SerializableSetting;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 0)]
public class LevelSo : ScriptableObject
{
    public Vector2 cellSize;
    public Serializable2DArray<BlockType> gridMapBlockSettlement;
}