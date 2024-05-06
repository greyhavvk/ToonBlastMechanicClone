using System;
using System.Collections.Generic;
using Core.BlockSystem.Block;
using Core.UI;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameEndPanel successPanel;
        [SerializeField] private GameEndPanel failPanel;
        [SerializeField] private GoalPanel goalPanel;
        [SerializeField] private FailTrackerPanel timeTracker;
        [SerializeField] private FailTrackerPanel moveCountTracker;
        public Action OnReloadButtonClicked;
        public static UIManager Instance;
        private void Awake()
        {
            SetInstance();
        }

        private void SetInstance()
        {
            if (Instance==null)
            {
                Instance = this;
            }
        }
        
        public void Initialize()
        {
            successPanel.SetOnButtonClicked(OnReloadButtonClicked);
            failPanel.SetOnButtonClicked(OnReloadButtonClicked);
        }

        public void UpdateGoalPanel(Dictionary<BlockType, int> goals)
        {
            goalPanel.UpdateGoalPanel(goals);
        }

        public void UpdateTimePanel(int time)
        {
            timeTracker.UpdatePanel(time);
        }

        public void UpdateMovePanel(int moveCount)
        {
            moveCountTracker.UpdatePanel(moveCount);
        }

        public void OpenSuccessPanel()
        {
            successPanel.OpenPanel();
        }

        public void OpenFailPanel()
        {
            failPanel.OpenPanel();
        }

        public void InitializeGoalPanel(Dictionary<BlockType, int> goals)
        {
            goalPanel.InitializeGoalPanel(goals);
        }
    }
}