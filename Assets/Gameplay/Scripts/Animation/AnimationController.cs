using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Scripts.Animation
{
    public class AnimationController<T> : IDisposable where T : Enum 
    {
        private Animator _animator;

        public AnimationController(Animator animator)
        {
            _animator = animator;
        }

        public void StartAnimation(IValueType valueType, Enum animationType)
        {
            if(_animator == null) return;
            
            if (valueType is Trigger<T>)
            {
                _animator?.SetTrigger(animationType.ToString());
            }
            else if (valueType is Bool<T>)
            {
                _animator?.SetBool(animationType.ToString(), ((Bool<T>)valueType).Value);
            }
            else if (valueType is Float<T>)
            {
                _animator?.SetFloat(animationType.ToString(), ((Float<T>)valueType).Value);
            }
        }

        public void Dispose()
        {
            if (_animator != null)
            {
                _animator.StopPlayback();
            }

            GC.SuppressFinalize(this);
        }
    }
}