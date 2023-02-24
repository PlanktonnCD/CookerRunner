using System.Collections.Generic;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Dishes;
using UI.Scripts.BeforeStartScreen;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.MainMenuScreen
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Image _dishImage;
        [SerializeField] private Button _button;
        [SerializeField] private int _chapter;
        [SerializeField] private int _level;
        [SerializeField] private List<StarImage> _starsImages;
        private UIManager _uiManager;
        private DataManager _dataManager;

        [Inject]
        private void Construct(UIManager uiManager, DataManager dataManager)
        {
            _dataManager = dataManager;
            _uiManager = uiManager;
        }

        public void Init()
        {
            _button.onClick.AddListener(OnClick);
        }
        
        public void SetClosed()
        {
            _button.interactable = false;
            _dishImage.color = Color.black;
        }
        
        public void SetAvailable()
        {
            _button.interactable = true;
            _dishImage.color = Color.white;
        }

        public void SetCompleted()
        {
            _button.interactable = true;
            _dishImage.color = Color.white;
            for (int i = 0; i < _dataManager.UserProfileData.ChapterInfoModel.GetHighscoreForLevel(_chapter, _level).Stars; i++)
            {
                _starsImages[i].ActivateStar();
            }
        }
        
        private void OnClick()
        {
            var args = new BeforeStartScreenArguments(_level, _chapter);
            _uiManager.Show<BeforeStartScreenController>(args);
        }
    }
}