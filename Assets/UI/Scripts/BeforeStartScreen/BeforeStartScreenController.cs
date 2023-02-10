using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Chapters;
using Movement;
using Zenject;

namespace UI.Scripts.BeforeStartScreen
{
    public class BeforeStartScreenController : UIScreenController<BeforeStartScreen>
    {
        private SignalBus _signalBus;
        private ChapterManager _chapterManager;

        [Inject]
        private void Construct(SignalBus signalBus, ChapterManager chapterManager)
        {
            _chapterManager = chapterManager;
            _signalBus = signalBus;
        }

        public override void Display(UIArgumentsForPanels arguments)
        {
            base.Display(arguments);
            var args = (BeforeStartScreenArguments)arguments;
            _chapterManager.CreateLevel(args.LevelIndex, args.ChapterIndex);
        }

        public override async UniTask OnShow()
        {
            await base.OnShow();
            View.TriggerToStart.OnPointerDown.AddListener(x =>
            {
                _signalBus.Fire(new CanStartRunSignal(true));
            });
        }
    }
}