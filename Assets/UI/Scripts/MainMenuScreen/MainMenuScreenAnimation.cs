using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.MainMenuScreen
{
    public class MainMenuScreenAnimation : UIAnimation
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