using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Scripts.RecipeWindow
{
    public class RecipeWindowAnimation : UIAnimation
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