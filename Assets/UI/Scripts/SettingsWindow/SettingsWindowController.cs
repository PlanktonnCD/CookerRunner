using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.Models;
using UI.Scripts.MainMenuScreen;
using UnityEngine;
using Zenject;

namespace UI.Settings
{
    public class SettingsWindowController : UIScreenController<SettingsWindow>
    {
        private DataManager _dataManager;
        private UIManager _uiManager;
        private AudioManager _audioManager;

        private SettingsInfoModel SettingsInfoModel => _dataManager.UserProfileData.SettingsInfoModel;

        [Inject]
        private void Construct(DataManager dataManager, UIManager uiManager, AudioManager audioManager)
        {
            _audioManager = audioManager;
            _uiManager = uiManager;
            _dataManager = dataManager;
        }
        
        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.CloseWindowButton.onClick.AddListener(() =>
            {
                _uiManager.HideLastWindow();
            });
            foreach (var settingButton in View.SettingButtons)
            {
                settingButton.Value.onClick.AddListener(() =>
                {
                    ChangeSetting(settingButton.Key);
                });
            }
            View.GoToMainMenuButton.onClick.AddListener(() =>
            {
                _uiManager.Show<MainMenuScreenController>();
            });
        }

        public override async UniTask OnShow()
        {
            if (_uiManager.GetCurrentScreen().GetType() == typeof(MainMenuScreenController))
            {
                View.GoToMainMenuButton.gameObject.SetActive(false);
            }
            else
            {
                View.GoToMainMenuButton.gameObject.SetActive(true);
            }

            foreach (var settingButton in View.SettingButtons)
            {
                SetButton(settingButton.Key);
            }
            await base.OnShow();
        }

        private void SetButton(SettingType settingType)
        {
            var settingButton = View.SettingButtons[settingType];
            if (SettingsInfoModel.IsSettingEnabled(settingType) == true)
            {
                var randomTime = Random.Range(0.5f, 1f);
                settingButton.image.color = Color.white;
                return;
            }
            settingButton.image.color = new Color(0.2735f, 0.2735f, 0.2735f, 1);
        }
        
        private void ChangeSetting(SettingType settingType)
        {
            var button = View.SettingButtons[settingType];
            var isEnabled = SettingsInfoModel.IsSettingEnabled(settingType);
            
            if (settingType == SettingType.Music)
            {
                _audioManager.MuteMusicPlayer(isEnabled);
            }

            if (settingType == SettingType.Sound)
            {
                _audioManager.MuteSoundPlayer(isEnabled);
            }
            SettingsInfoModel.SetSetting(!isEnabled, settingType);
            
            if (isEnabled == true)
            {
                button.image.color = new Color(0.2735f, 0.2735f, 0.2735f, 1);
                return;
            }
            button.image.color = Color.white;
        }
    }
}