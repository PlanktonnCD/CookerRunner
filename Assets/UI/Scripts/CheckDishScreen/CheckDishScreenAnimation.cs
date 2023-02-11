using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.CheckDishScreen
{
    public class CheckDishScreenAnimation : UIAnimation
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