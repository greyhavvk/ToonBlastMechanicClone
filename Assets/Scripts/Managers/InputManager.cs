using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InputManager : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Camera gameCamera;
        private const float CameraDistanceFromPlayGround = 5;
        public Action<Vector2> OnClickConfirmed;

        private bool _enable;
        
        private Vector3 CalculateTouchPosition(Vector2 touchPosition)
        {
            return gameCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, CameraDistanceFromPlayGround));
            
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_enable)
            {
                OnClickConfirmed?.Invoke(CalculateTouchPosition(Input.mousePosition));
            }
        }

        public void EnableInputs()
        {
            _enable=true;
        }
        
        public void DisableInputs()
        {
            _enable=false;
        }
    }
}