using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Chapters;
using Movement;
using UI.Scripts.RecipeWindow;
using Zenject;

namespace UI.Scripts.BeforeStartScreen
{
    public class BeforeStartScreenController : UIScreenController<BeforeStartScreen>
    {
        private SignalBus _signalBus;
        private ChapterManager _chapterManager;
        private UIManager _uiManager;

        [Inject]
        private void Construct(SignalBus signalBus, ChapterManager chapterManager, UIManager uiManager)
        {
            _uiManager = uiManager;
            _chapterManager = chapterManager;
            _signalBus = signalBus;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.DishRecipeButton.onClick.AddListener((() =>
            {
                _uiManager.Show<RecipeWindowController>();
            }));
        }

        public override void Display(UIArgumentsForPanels arguments)
        {
            base.Display(arguments);
            if (arguments != null)
            {
                var args = (BeforeStartScreenArguments)arguments;
                _chapterManager.CreateLevel(args.LevelIndex, args.ChapterIndex);
                return;
            }
            _chapterManager.CreateCurrentProgressLevel();
        }

        public override async UniTask OnShow()
        {
            await base.OnShow();
            _uiManager.Show<RecipeWindowController>();
            View.TriggerToStart.OnPointerDown.AddListener(x =>
            {
                _signalBus.Fire(new CanStartRunSignal(true));
            });
        }

        public override async UniTask OnHide()
        {
            View.TriggerToStart.OnPointerDown.RemoveAllListeners();
            await base.OnHide();
        }
    }
}