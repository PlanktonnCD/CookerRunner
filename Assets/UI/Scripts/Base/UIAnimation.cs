using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class UIAnimation : MonoBehaviour
    {
        [SerializeField] protected float _animationDuration;
        [SerializeField] protected Transform _panelTransform;
        [SerializeField] protected Image _backgroundImage;
        
        
        public abstract UniTask ShowAnimation();

        public abstract UniTask HideAnimation();
    }
}