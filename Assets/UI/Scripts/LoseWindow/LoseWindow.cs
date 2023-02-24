using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.LoseWindow
{
    public class LoseWindow : UIScreen
    {
        [field: SerializeField] public Button RetryButton { get; private set; }
        [field: SerializeField] public Button ShefHelpButton { get; private set; }
    }
}