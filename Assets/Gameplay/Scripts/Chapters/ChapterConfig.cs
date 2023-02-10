using System;
using System.Collections.Generic;
using Gameplay.Scripts.Dishes;
using UnityEngine;

namespace Gameplay.Scripts.Chapters
{
    public class ChapterConfig : ScriptableObject
    {
        [SerializeField] private List<ChapterInitializer> _chapters;

        public ChapterInitializer GetChapterByIndex(int chapter)
        {
            foreach (var chapterInitializer in _chapters)
            {
                if (chapterInitializer.Index == chapter)
                {
                    return chapterInitializer;
                }
            }

            return default;
        }
        
        public LevelInitializer GetLevelByIndex(int level, int chapter)
        {
            foreach (var levelInitializer in GetChapterByIndex(chapter).Levels)
            {
                if (levelInitializer.Index == level)
                {
                    return levelInitializer;
                }
            }

            return default;
        }
    }

    [Serializable]
    public struct ChapterInitializer
    {
        public int Index;
        public LocationName LocationName;
        public List<LevelInitializer> Levels;
    }

    [Serializable]
    public struct LevelInitializer
    {
        public int Index;
        public DishName DishOnLevel;
        public Level Level;
    }
}