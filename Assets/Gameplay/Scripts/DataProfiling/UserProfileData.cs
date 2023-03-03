using System.Xml;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.DataProfiling.Models;
using Gameplay.Scripts.Utils;
using Zenject;

namespace Gameplay.Scripts.DataProfiling
{
    public class UserProfileData
    {
        public SettingsInfoModel SettingsInfoModel => _settingsInfoModel;
        private SettingsInfoModel _settingsInfoModel = new();

        public ChapterInfoModel ChapterInfoModel => _chapterInfoModel;
        private ChapterInfoModel _chapterInfoModel = new();

        public async UniTask Initialize(DiContainer container)
        {
            foreach (var model in ReflectionUtils.GetFieldsOfType<IUserProfileModel>(this))
            {
                container.Inject(model);
                model.Initialize();
                await UniTask.CompletedTask;
            }
        }
    }
}