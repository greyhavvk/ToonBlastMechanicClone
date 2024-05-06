using Core.Factory_and_ObjectPool;
using Cysharp.Threading.Tasks;
using Factory_and_ObjectPool;
using UnityEngine;

namespace Core.ParticleSystems
{
    public class ParticleEntity : PoolableObject
    {
        [SerializeField] private ParticleSystem particle;
        private const int LifeTimeInMs = 2000;

        public async void Play()
        {
            particle.Play();

            await UniTask.Delay(LifeTimeInMs);
        }
    }
}