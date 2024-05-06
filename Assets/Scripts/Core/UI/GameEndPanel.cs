using System;
using UnityEngine;

namespace Core.UI
{
    public class GameEndPanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        private Action OnButtonClicked;

        public void SetOnButtonClicked(Action onButtonClicked)
        {
            OnButtonClicked += onButtonClicked;
        }
        
        public void OpenPanel()
        {
            panel.SetActive(true);
        }

        public void ButtonClicked()
        {
            OnButtonClicked?.Invoke();
        }

        public void ClosePanel()
        {
            panel.SetActive(false);
        }
    }
}