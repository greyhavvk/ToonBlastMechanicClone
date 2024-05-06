using System;
using System.Collections.Generic;
using Core.BlockSystem.Block;
using Core.SerializableSetting;
using UnityEngine;

namespace Core.UI
{
    public class GoalPanel : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<BlockType, GoalPanelPart> goalPanels;
        

        public void UpdateGoalPanel(Dictionary<BlockType, int> goals)
        {
            foreach (var goal in goals)
            {
                if (goalPanels.ContainsKey(goal.Key))
                {
                    goalPanels.GetValue(goal.Key).UpdateGoalPanel(goal.Value);
                }
            }
        }

        public void InitializeGoalPanel(Dictionary<BlockType, int> goals)
        {
            foreach (var goalPanel in goalPanels)
            {
                if (goals.ContainsKey(goalPanel.Key))
                {
                    goalPanel.Value.UpdateGoalPanel(goals[goalPanel.Key]);
                }
                goalPanel.Value.gameObject.SetActive(goals.ContainsKey(goalPanel.Key));
            }
        }
    }
}