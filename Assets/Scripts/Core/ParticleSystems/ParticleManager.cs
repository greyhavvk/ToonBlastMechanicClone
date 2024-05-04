using DG.Tweening;
using SerializableSetting;
using UnityEngine;

namespace ParticleSystems
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<ParticleType, ParticleFactory> particleFactories;
        public static ParticleManager Instance { get; private set; }

        private void Awake()
        {
            SetInstance();
        }
    
        private void SetInstance()
        {
            Instance = this;
        }

        public void PlayParticle(ParticleType particleType, Vector2 position)
        {
            if (!particleFactories.ContainsKey(particleType)) return;
            var particle = particleFactories[particleType].GetProduct() as ParticleEntity;
            particle.transform.position = position;
            particle.Play();

        }
    }
}