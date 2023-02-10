using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.BeforeStartScreen
{
    public class BeforeStartScreenAnimation : UIAnimation
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