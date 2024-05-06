using System;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;

namespace Core.TrackerSystem
{
    public class TimeTracker : MonoBehaviour
    {
        public Action OnTimeOver;
        
        private float _leftTime;
        private bool _timerActive=true;

        public async void StartTimer(float startTime)
        {
            _leftTime = startTime;

            while (_leftTime>=0 && _timerActive)
            {
                _leftTime -= Time.deltaTime;
                UIManager.Instance.UpdateTimePanel(Mathf.CeilToInt(_leftTime));
                await UniTask.Yield();
            }
            StopTimeTracker();
            OnTimeOver?.Invoke();
        }

        public void StopTimeTracker()
        {
            _timerActive = false;
        }
    }
}