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
        [SerializeField] private GameObject _hudOnLocation;
    
        private float _moveDuration = 0.1f;

        private void Start()
        {
            var seq = DOTween.Sequence();
            seq.Append(transform.DORotate(Vector3.up * 360 + transform.eulerAngles, 2).SetEase(Ease.Linear));
            seq.SetLoops(-1);
            seq.Play();
        }

        protected virtual UniTask OnPickUp(PlayerIngredientsStorage player)
        {
            if(_hudOnLocation != null) _hudOnLocation.SetActive(false);
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