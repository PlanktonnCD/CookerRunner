using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling;
using Zenject;

namespace UI.Scripts.MainMenuScreen
{
    public class MainMenuScreenController : UIScreenController<MainMenuScreen>
    {
        private DataManager _dataManager;

        [Inject]
        private void Construct(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            foreach (var chapter in View.Chapters)
            {
                chapter.Init(View.Chapters.IndexOf(chapter)+1);
            }
        }

        public override async UniTask OnShow()
        {
            await base.OnShow();
            foreach (var chapter in View.Chapters)
            {
                chapter.UpdateState();
            }
        }

        public override void UpdateScreen()
        {
            base.UpdateScreen();
            foreach (var chapter in View.Chapters)
            {
                chapter.UpdateState();
            }

            View.StarsCountText.text = _dataManager.UserProfileData.ChapterInfoModel.StarsCount.ToString();
        }
        
    }
}