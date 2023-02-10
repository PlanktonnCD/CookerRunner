using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.RunningScreen
{
    public class RunningScreenAnimation : UIAnimation
    {
        public override UniTask ShowAnimation()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask HideAnimation()
        {
            return UniTask.CompletedTask;
        }
    }
}