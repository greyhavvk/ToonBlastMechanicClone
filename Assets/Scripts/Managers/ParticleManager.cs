using Core.Factory_and_ObjectPool;
using Core.ParticleSystems;
using Core.SerializableSetting;
using Enums;
using UnityEngine;

namespace Managers
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ParticleType, ParticleFactory> particleFactories;

        public void Initialize()
        {
            foreach (var particleFactory in particleFactories)
            {
                particleFactory.Value.Initialize(new ParticleInitializeData());
            }
        }

        public void PlayParticle(ParticleType particleType, Vector2 position)
        {
            if (!particleFactories.ContainsKey(particleType)) return;
            var particle = particleFactories.GetValue(particleType).GetProduct() as ParticleEntity;
            if (!particle) return;
            particle.transform.position = position;
            particle.Play();
        }
    }
}