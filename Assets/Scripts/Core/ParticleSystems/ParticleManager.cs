using Core.SerializableSetting;
using ParticleSystems;
using UnityEngine;

namespace Core.ParticleSystems
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ParticleType, ParticleFactory> particleFactories;
        public static ParticleManager Instance { get; private set; }

        private void Awake()
        {
            SetInstance();
            foreach (var particleFactory in particleFactories)
            {
                particleFactory.Value.Initialize();
            }
        }
    
        private void SetInstance()
        {
            Instance = this;
        }

        public void PlayParticle(ParticleType particleType, Vector2 position)
        {
            if (!particleFactories.ContainsKey(particleType)) return;
            var particle = particleFactories.GetValue(particleType).GetProduct() as ParticleEntity;
            particle.transform.position = position;
            particle.Play();

        }
    }
}