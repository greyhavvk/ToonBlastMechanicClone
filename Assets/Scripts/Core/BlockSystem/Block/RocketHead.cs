using DG.Tweening;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class RocketHead : MonoBehaviour
    {
        public void TriggerHead(Vector2 destination, float speed)
        {
            var parent = transform.parent;
            transform.localPosition=Vector3.zero;
            transform.parent = null;
            gameObject.SetActive(true);
            transform.DOMove(destination, speed).SetSpeedBased(true).OnComplete((() =>
            {
                gameObject.SetActive(false);
                transform.parent = parent;
            }));
        }
    }
}