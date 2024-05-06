using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.BlockSystem.Block
{
    public class RocketHead : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;
        private const int DelayForGoBackInMs = 100;

        public async void TriggerHead(Vector2 destination, float speed)
        {
            List<UniTask> taskList = new List<UniTask>();
            var particleTransform = transform;
            var parent = particleTransform.parent;
            var oldLocalPosition = particleTransform.localPosition;
            particleTransform.parent = null;
            gameObject.SetActive(true);
            particleSystem.Play();
            var tween=transform.DOMove(destination, speed).SetSpeedBased(true).OnComplete((() =>
            {
                particleTransform.parent = parent;
                particleTransform.localPosition = oldLocalPosition;
                gameObject.SetActive(false);
                particleSystem.Stop();
            }));
            taskList.Add(tween.ToUniTask());
            await UniTask.WhenAll(taskList);
            await UniTask.Delay(DelayForGoBackInMs);
        }
    }
}