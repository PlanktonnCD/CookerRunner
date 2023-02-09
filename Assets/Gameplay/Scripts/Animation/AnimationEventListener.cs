using System;
using UnityEngine;

namespace Gameplay.Scripts.Animation
{
    public class AnimationEventListener : MonoBehaviour
    {
        public Action OnAttackAnimationEnd;
        public Action OnIdleAnimationEnd;

        public void OnAnimationAttack()
        {
            OnAttackAnimationEnd?.Invoke();
        }

        public void OnAnimationIdle()
        {
            OnIdleAnimationEnd?.Invoke();
        }
    }
}