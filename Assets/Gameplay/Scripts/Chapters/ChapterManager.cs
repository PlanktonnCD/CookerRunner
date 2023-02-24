using System;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Chapters
{
    public class ChapterManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerPrefab;
        private Level _currentLevel;
        public Level CurrentLevel => _currentLevel;
        private ChapterFactory _chapterFactory;
        private DataManager _dataManager;


        [Inject]
        private void Construct(ChapterFactory chapterFactory, DataManager dataManager)
        {
            _dataManager = dataManager;
            _chapterFactory = chapterFactory;
        }

        public void CreateLevel(int levelIndex, int chapterIndex)
        {
            if (_currentLevel != null)
            {
                Release();
            }
            _dataManager.UserProfileData.ChapterInfoModel.ChooseChapterAndLevel(levelIndex, chapterIndex);
            _currentLevel = _chapterFactory.Create(levelIndex, chapterIndex , transform);
            _currentLevel.SpawnPlayer(_playerPrefab);
        }

        public void CreateCurrentProgressLevel()
        {
            CreateLevel(_dataManager.UserProfileData.ChapterInfoModel.CurrentLevelprogress, _dataManager.UserProfileData.ChapterInfoModel.CurrentChapterProgress);
        }

        private void Release()
        {
            _currentLevel.Release();
            Destroy(_currentLevel.gameObject);
            _currentLevel = null;
        }

        public void Dispose()
        {
            _chapterFactory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}