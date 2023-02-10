using System;

namespace UI.Scripts.RunningScreen
{
    public class RunningScreenController : UIScreenController<RunningScreen>
    {
        private int _score;
        private Action<int> _changeScoreAction;
        public override void Display(UIArgumentsForPanels arguments)
        {
            base.Display(arguments);
            var args = (RunningScreenArguments)arguments;
            args.PlayerIngredientsStorage.InitChangeScoreAction(_changeScoreAction);
            _changeScoreAction += ChangeScore;
        }

        private void SetScoreText()
        {
            View.ScoreText.text = _score.ToString();
        }
        
        private void ChangeScore(int score)
        {
            _score += score;
            SetScoreText();
        }
    }
}