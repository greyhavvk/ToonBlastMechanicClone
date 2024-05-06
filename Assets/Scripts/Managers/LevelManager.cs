using Core.SerializableSetting;
using Enums;
using ScriptableObject;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSo[] levels;

        private int _currentLevelIndex;
        private LevelSo _currentLevel;
        private int NextLevelIndex => _currentLevelIndex + 1;

        public void Initialize()
        {
            _currentLevelIndex=PlayerPrefs.GetInt("Level",0) % levels.Length;
            _currentLevel = levels[_currentLevelIndex];
        }

        private void SaveNewLevelIndex()
        {
            PlayerPrefs.SetInt("Level",NextLevelIndex);
        }

        public Vector2 GetGridCellSize()
        {
            return _currentLevel.cellSize;
        }

        public BlockType[][] GetGridMapBlockSettlement()
        {
            return _currentLevel.GetGridMapBlockSettlement();
        }

        public int GetMoveCount()
        {
            return _currentLevel.moveCount;
        }

        public float GetLevelTime()
        {
            return _currentLevel.time;
        }

        public SerializableDictionary<BlockType, int> GetGoals()
        {
            return _currentLevel.goals;
        }

        public void IncreaseLevelIndex()
        {
            SaveNewLevelIndex();
        }
    }
}