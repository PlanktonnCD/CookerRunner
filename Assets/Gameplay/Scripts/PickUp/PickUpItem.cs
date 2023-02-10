using System;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.PickUp
{
    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] protected TrackName _trackOnPickName;
        [SerializeField] private GameObject _hudOnLocation;
    
        private float _moveDuration = 0.1f;
        protected AudioManager _audioManager;
        protected DataManager _dataManager;

        [Inject]
        private void Construct(AudioManager audioManager, DataManager dataManager)
        {
            _audioManager = audioManager;
            _dataManager = dataManager;
        }

        private void Start()
        {
            transform.DORotate(Vector3.up * 360 + transform.eulerAngles, 2).SetEase(Ease.Linear).SetLoops(-1);
        }

        protected virtual UniTask OnPickUp(PlayerIngredientsStorage player)
        {
            if(_hudOnLocation != null) _hudOnLocation.SetActive(false);
            //_audioManager.PlaySound(_trackOnPickName);
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerIngredientsStorage player))
            {
                OnPickUp(player);
            }
        }
    }
}