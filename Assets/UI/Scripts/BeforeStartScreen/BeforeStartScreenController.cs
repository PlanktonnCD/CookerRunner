using Audio;
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
        private AudioManager _audioManager;

        [Inject]
        private void Construct(SignalBus signalBus, ChapterManager chapterManager, UIManager uiManager, AudioManager audioManager)
        {
            _audioManager = audioManager;
            _uiManager = uiManager;
            _chapterManager = chapterManager;
            _signalBus = signalBus;
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
            _audioManager.PlayMusic(TrackName.levels_music);
            View.DishSmallRecipe.SetCurrentDishRecipe();
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
            View.DishSmallRecipe.Release();
            await base.OnHide();
        }
    }
}