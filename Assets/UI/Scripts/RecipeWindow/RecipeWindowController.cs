using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Chapters;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using Pool;
using UI.Scripts.CheckDishScreen;
using Zenject;

namespace UI.Scripts.RecipeWindow
{
    public class RecipeWindowController : UIScreenController<RecipeWindow>
    {
        private ChapterConfig _chapterConfig;
        private DishesConfig _dishesConfig;
        private DataManager _dataManager;
        private IngredientsConfig _ingredientsConfig;
        private UIManager _uiManager;

        [Inject]
        private void Construct(DishesConfig dishesConfig, ChapterConfig chapterConfig, DataManager dataManager, IngredientsConfig ingredientsConfig, UIManager uiManager)
        {
            _uiManager = uiManager;
            _ingredientsConfig = ingredientsConfig;
            _dataManager = dataManager;
            _dishesConfig = dishesConfig;
            _chapterConfig = chapterConfig;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.CloseButton.onClick.AddListener(() =>
            {
                _uiManager.HideLastWindow();
            });
        }

        public override async UniTask OnShow()
        {
            var levelInitializer = _chapterConfig.GetLevelByIndex(_dataManager.UserProfileData.ChapterInfoModel.ChosenLevel, _dataManager.UserProfileData.ChapterInfoModel.ChosenChapter);
            var dish = _dishesConfig.GetDishByName(levelInitializer.DishOnLevel);

            View.DishImage.sprite = dish.Sprite;
            View.DishNameText.text = dish.Name;
            SetIngredientsImages(View.PositiveIngredientImagesPool, dish.RequireIngredients);
            SetIngredientsImages(View.AdditionalScoreIngredientImagesPool, dish.AdditionalScoreIngredients);
            for (int i = 0; i < View.ScoreForStarsTexts.Count; i++)
            {
                View.ScoreForStarsTexts[i].text = levelInitializer.ScoreForStars[i].ToString();
            }
            await base.OnShow();
        }

        public override async UniTask OnHide()
        {
            await base.OnHide();
            View.PositiveIngredientImagesPool.ReturnAll();
            View.AdditionalScoreIngredientImagesPool.ReturnAll();
        }

        private void SetIngredientsImages(MonoBehaviourPool<IngredientImage> pool, List<IngredientsName> ingredientsNames)
        {
            foreach (var ingredient in ingredientsNames)
            {
                var ingredientImage = pool.GetObject();
                ingredientImage.SetIngredientImage(_ingredientsConfig.GetIngredientByName(ingredient).Sprite);
            }
        }
    }
}