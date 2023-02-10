using System;
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


        [Inject]
        private void Construct(ChapterFactory chapterFactory)
        {
            _chapterFactory = chapterFactory;
        }

        public void CreateLevel(int locationIndex, int chapterIndex)
        { 
            _currentLevel = _chapterFactory.Create(locationIndex, chapterIndex , transform);
            _currentLevel.SpawnPlayer(_playerPrefab);
        }

        public void DestroyLevel()
        {
            Destroy(_currentLevel);
        }

        public void Dispose()
        {
            _chapterFactory?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}