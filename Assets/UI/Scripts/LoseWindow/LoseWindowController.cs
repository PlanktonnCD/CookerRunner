using System;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling;
using UI.Scripts.BeforeStartScreen;
using Zenject;

namespace UI.Scripts.LoseWindow
{
    public class LoseWindowController : UIScreenController<LoseWindow>
    {
        private UIManager _uiManager;
        private DataManager _dataManager;
        private Action _chefHelpAction;

        [Inject]
        private void Construct(UIManager uiManager, DataManager dataManager)
        {
            _dataManager = dataManager;
            _uiManager = uiManager;
        }
        
        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.RetryButton.onClick.AddListener(() =>
            {
                _uiManager.HideLastWindow();
                var args = new BeforeStartScreenArguments(_dataManager.UserProfileData.ChapterInfoModel.ChosenLevel,
                    _dataManager.UserProfileData.ChapterInfoModel.ChosenChapter);
                _uiManager.Show<BeforeStartScreenController>(args);
            });
        }

        public override void Display(UIArgumentsForPanels arguments)
        {
            base.Display(arguments);
            var args = (LoseWindowArguments)arguments;
            _chefHelpAction = args.ChefHelpAction;
        }

        public override async UniTask OnShow()
        {
            await base.OnShow();
            View.ShefHelpButton.onClick.AddListener((() =>
            {
                _uiManager.HideLastWindow();
                _chefHelpAction.Invoke();
            }));
        }

        public override async UniTask OnHide()
        {
            View.ShefHelpButton.onClick.RemoveAllListeners();
            await base.OnHide();
        }
    }
}