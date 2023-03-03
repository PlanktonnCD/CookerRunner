using Gameplay.Scripts.DataProfiling;
using Newtonsoft.Json;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class SettingsInfoModel : IUserProfileModel
    {
        [JsonProperty] private bool _isSoundEnabled = true;
        [JsonProperty] private bool _isVibrationEnabled = true;
        [JsonProperty] private bool _isMusicEnabled = true;
        [JsonProperty] private int _languageIndex;
        
        public void Initialize()
        {
            
        }

        public bool IsSettingEnabled(SettingType settingType)
        {
            switch (settingType)
            {
                case SettingType.Music:
                {
                    return _isMusicEnabled;
                    break;
                }
                case SettingType.Sound:
                {
                    return _isSoundEnabled;
                    break;
                }
                case SettingType.Vibration:
                {
                    return _isVibrationEnabled;
                    break;
                }
            }
            return false;
        }

        public int GetCurrentLanguageIndex() => _languageIndex;
        public void SetLanguage(int index) => _languageIndex = index;

        public void SetSetting(bool setting, SettingType settingType)
        {
            switch (settingType)
            {
                case SettingType.Music:
                {
                    _isMusicEnabled = setting;
                    break;
                }
                case SettingType.Sound:
                {
                    _isSoundEnabled = setting;
                    break;
                }
                case SettingType.Vibration:
                {
                    _isVibrationEnabled = setting;
                    break;
                }
            }
        }
    }
}