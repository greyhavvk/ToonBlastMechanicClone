using System;
using Managers;
using UnityEngine;

namespace Core.TrackerSystem
{
    public class MoveCountTracker : MonoBehaviour
    {
        public static MoveCountTracker Instance;
        
        public Action OnNoMoveLeft;
        
        private int _moveCount;
        
        private void Awake()
        {
            if (Instance==null)
            {
                Instance = this;
            }
        }

        public void SetMoveCount(int moveCount)
        {
            _moveCount = moveCount;
            UIManager.Instance.UpdateMovePanel(_moveCount);
        }

        public void DetectMove()
        {
            _moveCount--;
            UIManager.Instance.UpdateMovePanel(_moveCount);
            if (_moveCount==0)
            {
                OnNoMoveLeft?.Invoke();
            }
        }
    }
}