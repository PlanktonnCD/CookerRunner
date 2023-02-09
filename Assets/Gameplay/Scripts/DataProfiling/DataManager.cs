using Cysharp.Threading.Tasks;
using Zenject;

namespace Gameplay.Scripts.DataProfiling
{
    public class DataManager
    {
        public UserProfileData UserProfileData => _userProfileData;
        private UserProfileData _userProfileData = new UserProfileData();
        private DiContainer _container;
        
        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }
        
        public void SaveData()
        {
            DataController.SaveIntoJson(_userProfileData);
        }

        public async UniTask ReadData()
        {
            var data = DataController.ReadFromJson();
            if (data == null)
            {
                _userProfileData = new UserProfileData();
                await _userProfileData.Initialize(_container);
                return;
            }
            _userProfileData = data;
            await _userProfileData.Initialize(_container);
        }
    }
}