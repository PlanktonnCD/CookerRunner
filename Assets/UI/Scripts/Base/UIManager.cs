using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour, IDisposable
    {
        private static int _maxWindowsOnTheScreen = 5;
        private UIFactory _uiFactory;

        private Dictionary<UIType,List<UIScreenController>> _uiDictionary = new Dictionary<UIType,List<UIScreenController>>
        {
            {UIType.Window, new List<UIScreenController>()},
            {UIType.Screen, new List<UIScreenController>()},
            {UIType.Widget, new List<UIScreenController>()}
        };

        private UIStorage _uiStorage;
        

        [Inject]
        private void Construct(UIFactory uiFactory)
        {
            _uiStorage = new UIStorage(uiFactory, transform);
        }

        public async UniTask Show<T>(UIArgumentsForPanels arguments = null) where T : UIScreenController
        {
            var screen = await _uiStorage.GetScreenController<T>();
            screen.Display(arguments);
            await ProcessShowPanel(screen);
        }

        private UIScreenController GetPreviousScreen(UIType uiType) =>
            _uiDictionary[uiType].Count > 0 ? _uiDictionary[uiType].Last() : null;
        
        private async UniTask ProcessShowPanel(UIScreenController screenController)
        {
            var previousScreen = GetPreviousScreen(screenController.UIType);
            if (previousScreen != null && previousScreen.GetType() != screenController.GetType())
            {
                await previousScreen.OnHide();
            }
            
            await screenController.OnShow();

            _uiDictionary[screenController.UIType].Add(screenController);
        }

        public UIScreenController GetCurrentScreen()
        {
            return GetPreviousScreen(UIType.Screen);
        }
        
        public async UniTask HideLastWindow()
        {
            GetPreviousScreen(UIType.Window).OnHide();
            GetPreviousScreen(UIType.Screen).UpdateScreen();
        }

        public async UniTask HideLastWidget()
        {
            GetPreviousScreen(UIType.Widget).OnHide();
            GetPreviousScreen(UIType.Window).UpdateScreen();
        }

        public void Dispose()
        {
            _uiStorage.OnDispose();
            GC.SuppressFinalize(this);
        }
    }
}
