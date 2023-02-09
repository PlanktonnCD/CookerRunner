using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public abstract class UIScreenController<T> : UIScreenController where T : UIScreen
    {
        protected T View;

        public override void Init(UIScreen uiScreen)
        {
            View = uiScreen as T;
            _uiType = View.UIType;
        }

        public override void Display(UIArgumentsForPanels arguments)
        {
            
        }

        public override async UniTask OnShow()
        {
            View.transform.SetSiblingIndex(999);

            await View.OnShow();
        }
        public override async UniTask OnHide()
        {
            await View.OnHide();
        }

        public override async UniTask DestroyScreen()
        {
            await View.OnDispose();
        }

        public override void UpdateScreen()
        {
            
        }
    }
    
    public abstract class UIScreenController
    {
        public abstract void Init(UIScreen uiScreen);
        public abstract void Display(UIArgumentsForPanels arguments);
        public abstract UniTask OnShow();
        public abstract UniTask OnHide();
        public abstract UniTask DestroyScreen();
        public abstract void UpdateScreen();
        protected UIType _uiType;
        public UIType UIType => _uiType;
    }
}