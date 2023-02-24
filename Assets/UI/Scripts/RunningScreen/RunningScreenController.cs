using System;
using Cysharp.Threading.Tasks;

namespace UI.Scripts.RunningScreen
{
    public class RunningScreenController : UIScreenController<RunningScreen>
    {
        private Action<int> _changeScoreAction;
        public override void Display(UIArgumentsForPanels arguments)
        {
            base.Display(arguments);
            var args = (RunningScreenArguments)arguments;
            SetScoreText(0);
            _changeScoreAction += SetScoreText;
            args.PlayerIngredientsStorage.InitChangeScoreAction(_changeScoreAction);
            
        }

        public override async UniTask OnShow()
        {
            View.DishSmallRecipe.SetCurrentDishRecipe();
            await base.OnShow();
        }

        public override async UniTask OnHide()
        {
            View.DishSmallRecipe.Release();
            await base.OnHide();
        }

        private void SetScoreText(int score)
        {
            View.ScoreText.text = score.ToString();
        }
        
    }
}