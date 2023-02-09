using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIFactory : IDisposable
    {
        private DiContainer _container;
        private UIConfig _uiConfig;

        public UIFactory(DiContainer container, UIConfig uiConfig)
        {
            _container = container;
            _uiConfig = uiConfig;
        }

        public UIScreenController Create<T>(Transform parent) where T : UIScreenController
        {
            var uiScreen = GetUIScreen<T>(parent);
            
            uiScreen.gameObject.SetActive(false);

            var screenController = _container.Instantiate<T>();
            
            screenController.Init(uiScreen);
            
            return screenController;
        }
        
        private UIScreen GetUIScreen<T>(Transform parent) where T : UIScreenController
        {
            var screenReference = _uiConfig.GetUIPrefab<T>();
            var screen = _container.InstantiatePrefab(screenReference, parent);

            return screen.GetComponent<UIScreen>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}