using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class GoalPanelPart : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite completeIcon;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private void SetText(int number)
        {
            if (number==0)
            {
                image.sprite = completeIcon;
                textMeshProUGUI.gameObject.SetActive(false);
            }
            else
            {
                textMeshProUGUI.text = number.ToString();
            }
        }

        public void UpdateGoalPanel(int goalValue)
        {
            SetText(goalValue);
        }
    }
}