using System;
using Gameplay.Scripts.DataProfiling;
using UI;
using UI.Scripts.BeforeStartScreen;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Bootstrap
{
    public class GameBootstrap : MonoBehaviour
    {
        private DataManager _dataManager;
        private UIManager _uiManager;

        [Inject]
        private void Construct(UIManager uiManager, DataManager dataManager)
        {
            _uiManager = uiManager;
            _dataManager = dataManager;
        }

        private async void Start()
        {
            await _dataManager.ReadData();

            var args = new BeforeStartScreenArguments(_dataManager.UserProfileData.ChapterInfoModel.CurrentLevelprogress, _dataManager.UserProfileData.ChapterInfoModel.CurrentChapterProgress);
            _uiManager.Show<BeforeStartScreenController>(args);
        }
        
        private void OnDisable()
        {
            _dataManager.SaveData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus == true)
            {
                _dataManager.SaveData();
                return;
            }
            _dataManager.ReadData();
        }
    }
}