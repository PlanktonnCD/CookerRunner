using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class UIScreen : MonoBehaviour, IDisposable
    {
        [SerializeField] private UIAnimation _animation;
        [SerializeField] private UIType _uiType;
        public UIType UIType => _uiType;

        public async virtual UniTask OnShow()
        {
            gameObject.SetActive(true);
            if (!_animation)
            {
                await UniTask.CompletedTask;
            }
            await _animation.ShowAnimation();
        }
        
        public async virtual UniTask OnHide()
        {
            await _animation.HideAnimation();
            gameObject.SetActive(false);
        }

        public async virtual UniTask OnDispose()
        {
            
        }

        public void Dispose()
        {
            OnDispose();
        }
    }
}