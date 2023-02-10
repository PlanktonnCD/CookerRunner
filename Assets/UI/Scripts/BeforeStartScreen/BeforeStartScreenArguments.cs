namespace UI.Scripts.BeforeStartScreen
{
    public class BeforeStartScreenArguments : UIArgumentsForPanels
    {
        public int LevelIndex => _levelIndex;
        private int _levelIndex;
        public int ChapterIndex => _chapterIndex;
        private int _chapterIndex;

        public BeforeStartScreenArguments(int levelIndex, int chapterIndex)
        {
            _chapterIndex = chapterIndex;
            _levelIndex = levelIndex;
        }
    }
}