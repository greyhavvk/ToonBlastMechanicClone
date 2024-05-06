using System;
using UnityEngine;

namespace Core.UI
{
    public class GameEndPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private Action _onButtonClicked;

        public void SetOnButtonClicked(Action onButtonClicked)
        {
            _onButtonClicked += onButtonClicked;
        }
        
        public void OpenPanel()
        {
            panel.SetActive(true);
        }

        public void ButtonClicked()
        {
            _onButtonClicked?.Invoke();
        }
    }
}