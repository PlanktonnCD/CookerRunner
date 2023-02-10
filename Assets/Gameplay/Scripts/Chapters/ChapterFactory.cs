using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Chapters
{
    public class ChapterFactory : IDisposable
    {
        private DiContainer _container;
        private ChapterConfig _chapterConfig;

        public ChapterFactory(DiContainer diContainer, ChapterConfig chapterConfig)
        {
            _container = diContainer;
            _chapterConfig = chapterConfig;
        }

        public Level Create(int levelIndex, int chapterIndex, Transform parent)
        {
            var reference = _chapterConfig.GetLevelByIndex(levelIndex, chapterIndex);
            var level = _container.InstantiatePrefab(reference.Level, parent).GetComponent<Level>();
            level.Init(reference.DishOnLevel);
            return level;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}