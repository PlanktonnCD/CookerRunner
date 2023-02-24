using TMPro;
using UI.Scripts.BeforeStartScreen;
using UnityEngine;

namespace UI.Scripts.RunningScreen
{
    public class RunningScreen : UIScreen
    {
        [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
        [field: SerializeField] public DishSmallRecipe DishSmallRecipe { get; private set; }
    }
}