using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class ChapterInfoModel : IUserProfileModel
    {
        [JsonIgnore] public int CurrentChapterProgress => _currentChapterProgress;
        [JsonProperty] private int _currentChapterProgress = 1;
        
        [JsonIgnore] public int CurrentLevelprogress => _currentLevelProgress;
        [JsonProperty] private int _currentLevelProgress = 1;
        
        [JsonIgnore] public int ChosenChapter => _chosenChapter;
        [JsonProperty] private int _chosenChapter = 1;
        [JsonIgnore] public int ChosenLevel => _chosenLevel;
        [JsonProperty] private int _chosenLevel = 1;
        
        [JsonIgnore] private Dictionary<int, int> _countLevelsInChapter =  new ()
        {
            [1] = 4,
            [2] = 4
        };

        [JsonProperty] private Dictionary<int, ChapterHighscores> _highscores = new Dictionary<int, ChapterHighscores>();

        public void Initialize()
        {
        }
        
        public bool TryIncreaseChapterLevelIndex(int count = 1)
        {
            if (_chosenChapter != _currentChapterProgress && _chosenLevel != _currentLevelProgress)
            {
                return false;
            }
            
            if (_currentLevelProgress >= _countLevelsInChapter[_currentChapterProgress])
            {
                IncreaseChapterIndex();
                return false;
            }
            
            _currentLevelProgress += count;
            return true;
        }

        private void IncreaseChapterIndex(int count = 1)
        {
            if(_currentChapterProgress + count > _countLevelsInChapter.Count) return;
            _currentChapterProgress += count;
            _currentLevelProgress = 1;
        }

        public void SetHighscore(int chapter, int level, int score, int stars)
        {
            var highscore = GetHighscoreForLevel(chapter, level);
            if (highscore.Score < score || highscore.Equals(default))
            {
                highscore.Score = score;
                highscore.Stars = stars;
            }
            _highscores[chapter].Highscores[level-1] = highscore;
        }

        public Highscore GetHighscoreForLevel(int chapter, int level)
        {
            if (_highscores.ContainsKey(chapter) == false)
            {
                _highscores.Add(chapter, new ChapterHighscores()
                {
                    Highscores = new List<Highscore>()
                });
            }

            var highscores = _highscores[chapter].Highscores;
            if (highscores.Count < level)
            {
                highscores.Add(new Highscore());
            }
            return highscores[level-1];
        }
        
        public void ChooseChapterAndLevel(int level, int chapter)
        {
            _chosenChapter = chapter;
            _chosenLevel = level;
        }
    }

    public struct ChapterHighscores
    {
        public List<Highscore> Highscores;
    }
    
    public struct Highscore
    {
        public int Score;
        public int Stars;
    }
}