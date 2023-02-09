using System;
using Pool;
using UniRx;
using UnityEngine;
using Zenject;

namespace Particles
{
    public class ParticleManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private ParticlesPool _particlesPool;
        private ParticleFactory _particleFactory;
        private CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        private void Construct(ParticleFactory particleFactory)
        {
            _particleFactory = particleFactory;
        }

        private void Start()
        {
            _particlesPool.Init(_particleFactory);
        }

        public void PlayParticleInTransform(ParticleType particleType, Transform parent, Quaternion rotation)
        {
            var particle = _particlesPool.GetObject(particleType);
            particle.transform.parent = parent;
            particle.transform.position = parent.position;
            particle.transform.localRotation = rotation;
            particle.transform.localScale = Vector3.one;

            PlayParticle(particle);
        }

        public void PlayParticleInPosition(ParticleType particleType, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var particle = _particlesPool.GetObject(particleType);
            particle.transform.position = position;
            particle.transform.localRotation = rotation;
            particle.transform.localScale = scale;
            
            PlayParticle(particle);
        }

        private void PlayParticle(ParticleSystem particle)
        {
            particle.Play();
            
            if (particle.main.loop == false)
            {
                int timeWait = (int)Math.Ceiling(particle.main.duration);
                Observable.Timer(TimeSpan.FromSeconds(timeWait)).Subscribe(_ =>
                {
                    if(particle == null || particle.gameObject == null) return;
                    particle.transform.parent = transform;
                    _particlesPool.ReturnObject(particle);
                }).AddTo(_disposables);
            }
        }

        public void Dispose()
        {
            _disposables.Clear();
            _particlesPool.Dispose();
        }
    }
}