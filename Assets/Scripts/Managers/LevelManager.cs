using System;
using Core.BlockSystem.Block;
using ScriptableObject;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSo[] levels;

        private int _currentLevelIndex = 0;
        private LevelSo _currentLevel;

        public void Initialize()
        {
            _currentLevelIndex=PlayerPrefs.GetInt("Level",0) % levels.Length;
            _currentLevel = levels[_currentLevelIndex];
        }

        public void SaveNewLevelIndex()
        {
            _currentLevelIndex++;
            PlayerPrefs.SetInt("Level",_currentLevelIndex);
        }

        public Vector2 GetGridCellSize()
        {
            return _currentLevel.cellSize;
        }

        public BlockType[][] GetGridMapBlockSettlement()
        {
            return _currentLevel.gridMapBlockSettlement.ToArray();
        }
    }
}