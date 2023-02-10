using Newtonsoft.Json;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class ChapterInfoModel : IUserProfileModel
    {
        [JsonIgnore] public int CurrentChapterProgress => _currentChapterProgress;
        [JsonProperty] private int _currentChapterProgress = 1;
        
        [JsonIgnore] public int CurrentLevelprogress => _currentLevelProgress;
        [JsonProperty] private int _currentLevelProgress = 1;
        
        [JsonIgnore] private int _countLevelsInChapter = 5;
        [JsonIgnore] private int _currentChapterCount = 2;

        public void Initialize()
        {
        }
        
        public bool TryIncreaseChapterLevelIndex(int count = 1)
        {
            var currentProgress = ((_currentChapterProgress - 1) * _countLevelsInChapter) + _currentLevelProgress;

            if (_currentLevelProgress >= _countLevelsInChapter)
            {
                IncreaseChapterIndex();
                return false;
            }
            
            _currentLevelProgress += count;
            return true;
        }

        private void IncreaseChapterIndex(int count = 1)
        {
            if(_currentChapterProgress + count > _currentChapterCount) return;
            _currentChapterProgress += count;
            _currentLevelProgress = 1;
        }
    }
}