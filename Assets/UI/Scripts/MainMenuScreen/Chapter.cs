using System.Collections.Generic;
using Gameplay.Scripts.Chapters;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Dishes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.MainMenuScreen
{
    public class Chapter : MonoBehaviour
    {
        [SerializeField] private List<LevelButton> _levelButtons;
        [SerializeField] private Image _closeImage;
        [SerializeField] private TextMeshProUGUI _needStarsText;
        private int _index;
        private DataManager _dataManager;
        private ChapterConfig _chapterConfig;

        [Inject]
        private void Construct(DataManager dataManager, ChapterConfig chapterConfig)
        {
            _chapterConfig = chapterConfig;
            _dataManager = dataManager;
        }
        
        public void Init(int index)
        {
            _index = index;
            _needStarsText.text = _chapterConfig.GetChapterByIndex(_index).StarsForOpen.ToString();
            foreach (var levelButton in _levelButtons)
            {
                levelButton.Init();
            }
        }

        public void UpdateState()
        {
            _closeImage.gameObject.SetActive(false);
            var chapterInfoModel = _dataManager.UserProfileData.ChapterInfoModel;
            if (chapterInfoModel.StarsCount < _chapterConfig.GetChapterByIndex(_index).StarsForOpen)
            {
                _closeImage.gameObject.SetActive(true);
            }
            if (_index > chapterInfoModel.CurrentChapterProgress)
            {
                foreach (var levelButton in _levelButtons)
                {
                    levelButton.SetClosed();
                }
                return;
            }
            foreach (var levelButton in _levelButtons)
            {
                if (chapterInfoModel.CurrentLevelprogress > _levelButtons.IndexOf(levelButton)+1)
                {
                    levelButton.SetCompleted();
                }
                else if (chapterInfoModel.CurrentLevelprogress == _levelButtons.IndexOf(levelButton)+1)
                {
                    levelButton.SetAvailable();
                }
                else
                {
                    levelButton.SetClosed();
                }
            }
        }
    }
}