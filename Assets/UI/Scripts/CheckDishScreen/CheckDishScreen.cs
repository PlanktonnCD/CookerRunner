using System.Collections.Generic;
using Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.CheckDishScreen
{
    public class CheckDishScreen : UIScreen
    {
        [field: SerializeField] public MonoBehaviourPool<IngredientImage> PositiveIngredientImagesPool { get; private set; }
        [field: SerializeField] public MonoBehaviourPool<IngredientImage> AdditionalScoreIngredientImagesPool { get; private set; }
        [field: SerializeField] public TextMeshProUGUI AccuracyDishText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI DishNameText { get; private set; }
        [field: SerializeField] public TextMeshProUGUI ScoreText { get; private set; }
        [field: SerializeField] public Button ComebackButton { get; private set; }
        [field: SerializeField] public List<StarImage> StarsImages { get; private set; }
    }
}