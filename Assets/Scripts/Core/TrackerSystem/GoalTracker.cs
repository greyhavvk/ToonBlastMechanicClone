using System;
using System.Collections.Generic;
using Core.SerializableSetting;
using Enums;
using Managers;
using UnityEngine;

namespace Core.TrackerSystem
{
    public class GoalTracker : MonoBehaviour
    {
        public static GoalTracker Instance;
        private Dictionary<BlockType, int> _goals;
        private int _completedGoals;

        public Action OnGoalComplete;

        private void Awake()
        {
            if (Instance==null)
            {
                Instance = this;
            }
        }

        public void SetGoals(SerializableDictionary<BlockType, int> goals)
        {
            _goals=new Dictionary<BlockType, int>();
            foreach (var goal in goals)
            {
                if (goal.Value!=0)
                {
                    _goals.Add(goal.Key, goal.Value);
                }
            }
            UIManager.Instance.InitializeGoalPanel(_goals);
        }

        public void BlockDestroyed(BlockType blockType)
        {
            if (_goals.ContainsKey(blockType) && _goals[blockType]>0)
            {
                ReduceGoal(blockType);
            }
        }

        private void ReduceGoal(BlockType blockType)
        {
            _goals[blockType] = _goals[blockType] - 1;
            if (_goals[blockType]==0)
            {
                _completedGoals++;
                if (_completedGoals==_goals.Count)
                {
                    OnGoalComplete?.Invoke();
                }
            }
            UIManager.Instance.UpdateGoalPanel(_goals);
        }
    }
}