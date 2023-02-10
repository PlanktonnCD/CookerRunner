using TMPro;
using UnityEngine;

namespace UI.Scripts.RunningScreen
{
    public class RunningScreen : UIScreen
    {
        [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
    }
}