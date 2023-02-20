using System;
using UnityEngine;

namespace Gameplay.Scripts.Animation
{
    public class AnimationEventListener : MonoBehaviour
    {
        public Action OnAnimationEndAction;

        public void OnAnimationEnd()
        {
            OnAnimationEndAction?.Invoke();
        }
    }
}