using UnityEngine;

namespace BlockSystem
{
    public class BlockScanner : MonoBehaviour
    {
        public static BlockScanner Instance;
        
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
    }
}