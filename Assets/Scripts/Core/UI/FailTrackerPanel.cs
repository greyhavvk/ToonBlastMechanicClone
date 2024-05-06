using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class FailTrackerPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private string specialTextForZero;
        [SerializeField] private string specialAdd;
        public void UpdatePanel(int value)
        {
            textMeshProUGUI.text = value > 0 ? value+ specialAdd : specialTextForZero;
        }
    }
}