using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.LoseWindow
{
    public class LoseWindowAnimation : UIAnimation
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