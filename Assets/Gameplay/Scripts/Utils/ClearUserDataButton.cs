#if UNITY_EDITOR
using Gameplay.Scripts.DataProfiling;
using UnityEditor;

namespace Gameplay.Scripts.Utils
{
    public static class ClearUserDataButton
    {
        [MenuItem("Tools/ResetProgress")]
        public static void ResetProgress()
        {
            DataController.ResetProgress();
        }
    }
}
#endif