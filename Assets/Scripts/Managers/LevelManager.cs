using System;
using System.Collections.Generic;
using Core.BlockSystem.Block;
using Core.SerializableSetting;
using ScriptableObject;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSo[] levels;

        private int _currentLevelIndex = 0;
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

        public int GetLevelIndex()
        {
            return NextLevelIndex;
        }

        public void IncreaseLevelIndex()
        {
            SaveNewLevelIndex();
        }
    }
}