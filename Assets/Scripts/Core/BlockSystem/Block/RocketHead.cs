using DG.Tweening;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class RocketHead : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        
        public void TriggerHead(Vector2 destination, float speed)
        {
            Debug.Log("triggered");
            var parent = transform.parent;
            var oldLocalPosition = transform.localPosition;
            transform.parent = null;
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
            transform.DOMove(destination, speed).SetSpeedBased(true).OnComplete((() =>
            {
                transform.parent = parent;
                transform.localPosition = oldLocalPosition;
                particleSystem.gameObject.SetActive(false);
            }));
        }
    }
}