using Cysharp.Threading.Tasks;

namespace UI.Scripts.MainMenuScreen
{
    public class MainMenuScreenController : UIScreenController<MainMenuScreen>
    {
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
        }
        
    }
}