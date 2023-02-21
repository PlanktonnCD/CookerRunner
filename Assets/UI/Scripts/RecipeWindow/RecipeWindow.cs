using System.Collections.Generic;
using Pool;
using TMPro;
using UI.Scripts.CheckDishScreen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.RecipeWindow
{
    public class RecipeWindow : UIScreen
    {
        [field: SerializeField] public Image DishImage { get; private set; }
        [field: SerializeField] public TextMeshProUGUI DishNameText { get; private set; }
        [field: SerializeField] public MonoBehaviourPool<IngredientImage> PositiveIngredientImagesPool { get; private set; }
        [field: SerializeField] public MonoBehaviourPool<IngredientImage> AdditionalScoreIngredientImagesPool { get; private set; }
        [field: SerializeField] public List<TextMeshProUGUI> ScoreForStarsTexts { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
    }
}