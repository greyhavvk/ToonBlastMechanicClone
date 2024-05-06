using System;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera gameCamera;
        private const float CameraDistanceFromPlayGround = 5;
        public Action<Vector2> OnClickConfirmed;

        private bool _enable;
        
        private Vector3 CalculateTouchPosition(Vector2 touchPosition)
        {
            return gameCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, CameraDistanceFromPlayGround));
            
        }

        private void Update()
        {
            if (_enable && Input.GetMouseButtonDown(0))
            {
                OnClickConfirmed?.Invoke(CalculateTouchPosition(Input.mousePosition));
            }
        }

        public void EnableInputs()
        {
            enabled = true;
            _enable=true;
        }
        
        public void DisableInputs()
        {
            enabled = false;
            _enable=false;
        }
    }
}