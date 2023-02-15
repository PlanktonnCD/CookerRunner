using System.Collections.Generic;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Dishes;
using UnityEngine;
using Zenject;

namespace UI.Scripts.MainMenuScreen
{
    public class Chapter : MonoBehaviour
    {
        [SerializeField] private List<LevelButton> _levelButtons;
        private int _index;
        private DataManager _dataManager;

        [Inject]
        private void Construct(DataManager dataManager)
        {
            _dataManager = dataManager;
        }
        
        public void Init(int index)
        {
            _index = index;
            foreach (var levelButton in _levelButtons)
            {
                levelButton.Init();
            }
        }

        public void UpdateState()
        {
            var chapterInfoModel = _dataManager.UserProfileData.ChapterInfoModel;
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