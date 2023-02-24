using System.Collections.Generic;
using Gameplay.Scripts.Chapters;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using Pool;
using TMPro;
using UI.Scripts.CheckDishScreen;
using UnityEngine;
using Zenject;

namespace UI.Scripts.BeforeStartScreen
{
    public class DishSmallRecipe : MonoBehaviour
    {
        [SerializeField] private MonoBehaviourPool<IngredientImage> _positiveIngredientImagesPool;
        [SerializeField] private MonoBehaviourPool<IngredientImage> _additionalIngredientImagesPool;
        [SerializeField] private TextMeshProUGUI _dishNameText;
        private IngredientsConfig _ingredientsConfig;
        private DataManager _dataManager;
        private ChapterConfig _chapterConfig;
        private DishesConfig _dishesConfig;

        [Inject]
        private void Construct(IngredientsConfig ingredientsConfig, DataManager dataManager, ChapterConfig chapterConfig, DishesConfig dishesConfig)
        {
            _dishesConfig = dishesConfig;
            _chapterConfig = chapterConfig;
            _dataManager = dataManager;
            _ingredientsConfig = ingredientsConfig;
        }
        
        public void SetCurrentDishRecipe()
        {
            var level = _chapterConfig.GetLevelByIndex(_dataManager.UserProfileData.ChapterInfoModel.ChosenLevel,
                _dataManager.UserProfileData.ChapterInfoModel.ChosenChapter);
            var dish = _dishesConfig.GetDishByName(level.DishOnLevel);
            SetIngredientsImages(_positiveIngredientImagesPool, dish.RequireIngredients);
            SetIngredientsImages(_additionalIngredientImagesPool, dish.AdditionalScoreIngredients);
            _dishNameText.text = dish.Name;
        }
        
        private void SetIngredientsImages(MonoBehaviourPool<IngredientImage> pool, List<IngredientsName> ingredientsNames)
        {
            foreach (var ingredient in ingredientsNames)
            {
                var ingredientImage = pool.GetObject();
                ingredientImage.SetIngredientImage(_ingredientsConfig.GetIngredientByName(ingredient).Sprite);
            }
        }

        public void Release()
        {
            _positiveIngredientImagesPool.ReturnAll();
            _additionalIngredientImagesPool.ReturnAll();
        }
    }
}