using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Chapters;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using Pool;
using UI.Scripts.BeforeStartScreen;
using UI.Scripts.MainMenuScreen;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts.CheckDishScreen
{
    public class CheckDishScreenController : UIScreenController<CheckDishScreen>
    {
        private IngredientsConfig _ingredientsConfig;
        private DishesConfig _dishesConfig;
        private Dish _dish;
        private List<IngredientsName> _ingredients;
        private int _score;
        private float _accuracyScoreMultiplier = 1;
        private float _additionalScoreMultiplier = 1;
        private float _timeScoreMultiplier = 1;
        private bool _isLose;
        private UIManager _uiManager;
        private ChapterConfig _chapterConfig;
        private DishName _dishName;
        private DataManager _dataManager;
        private int _chapter;
        private int _level;
        private ChapterManager _chapterManager;

        [Inject]
        private void Construct(DishesConfig dishesConfig, IngredientsConfig ingredientsConfig, ChapterConfig chapterConfig, UIManager uiManager, ChapterManager chapterManager, DataManager dataManager)
        {
            _chapterManager = chapterManager;
            _dataManager = dataManager;
            _chapterConfig = chapterConfig;
            _uiManager = uiManager;
            _dishesConfig = dishesConfig;
            _ingredientsConfig = ingredientsConfig;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            View.ComebackButton.onClick.AddListener((() =>
            {
                _chapterManager.Release();
                if (_dataManager.UserProfileData.ChapterInfoModel.TryIncreaseChapterLevelIndex() == true)
                {
                    
                    _uiManager.Show<BeforeStartScreenController>();
                    return;
                }
                _uiManager.Show<MainMenuScreenController>();
            }));
        }

        public override void Display(UIArgumentsForPanels arguments)
        {
            base.Display(arguments);
            var args = (CheckDishScreenArguments)arguments;
            _dishName = args.DishName;
            _dish = _dishesConfig.GetDishByName(_dishName);
            _ingredients = args.Ingredients;
            _score = args.Score;
            _chapter = _dataManager.UserProfileData.ChapterInfoModel.ChosenChapter;
            _level = _dataManager.UserProfileData.ChapterInfoModel.ChosenLevel;
            SetIngredientsImages(View.PositiveIngredientImagesPool, _dish.RequireIngredients, _ingredients);
            SetIngredientsImages(View.AdditionalScoreIngredientImagesPool, _dish.AdditionalScoreIngredients, _ingredients);

            List<IngredientsName> neededIngredientsList = new List<IngredientsName>();
            foreach (var ingredient in _dish.RequireIngredients)
            {
                neededIngredientsList.Add(ingredient);
            }
            foreach (var ingredient in _dish.AdditionalScoreIngredients)
            {
                neededIngredientsList.Add(ingredient);
            }
            
            SetIngredientsImages(View.StoredIngredientImagesPool, _ingredients, neededIngredientsList);
        }

        public override async UniTask OnShow()
        {
            View.ComebackButton.gameObject.SetActive(false);
            View.AccuracyDishText.gameObject.SetActive(false);
            View.ScoreText.gameObject.SetActive(false);
            await base.OnShow();
            CheckDish();
        }

        private async void CheckDish()
        {
            await CheckExistenceRequiredIngredients();
            await CalculateAdditionalIngredientsMultiplier();
            await CheckDishAccuracy();
            if (_isLose == true)
            {
                return;
            }
            await CalculateScore();
            View.ComebackButton.gameObject.SetActive(true);
        }

        private void SetIngredientsImages(MonoBehaviourPool<IngredientImage> pool, List<IngredientsName> ingredientsNames, List<IngredientsName> checkList)
        {
            foreach (var ingredient in ingredientsNames)
            {
                var ingredientImage = pool.GetObject();
                ingredientImage.SetIngredientImage(_ingredientsConfig.GetIngredientByName(ingredient).Sprite);
            }
        }
        
        private async UniTask SetIngredientsStateImages(MonoBehaviourPool<IngredientImage> pool, List<IngredientsName> ingredientsNames, List<IngredientsName> checkList)
        {
            var ingredientImages = pool.GetAllActiveObjects();
            for (int i = 0; i < ingredientImages.Count; i++)
            {
                await ingredientImages[i].SetObtainState(checkList.Contains(ingredientsNames[i]));
            }
        }
        
        private async UniTask CheckExistenceRequiredIngredients()
        {
            var dictionary = new Dictionary<IngredientsName, bool>();
            foreach (var requireIngredient in _dish.RequireIngredients)
            {
                dictionary.Add(requireIngredient, _ingredients.Contains(requireIngredient));
            }

            await SetIngredientsStateImages(View.PositiveIngredientImagesPool, _dish.RequireIngredients, _ingredients);
            
            if (dictionary.ContainsValue(false))
            {
                _isLose = true;
            }
        }

        private async UniTask CalculateAdditionalIngredientsMultiplier()
        {
            _additionalScoreMultiplier = 1f;
            await SetIngredientsStateImages(View.AdditionalScoreIngredientImagesPool, _dish.AdditionalScoreIngredients, _ingredients);
            int correctIngredients = 0;
            foreach (var ingredient in _ingredients)
            {
                if ( _dish.AdditionalScoreIngredients.Contains(ingredient))
                {
                    correctIngredients++;
                    _additionalScoreMultiplier += 0.1f;
                }
            }

            if (correctIngredients == _dish.AdditionalScoreIngredients.Count)
            {
                _additionalScoreMultiplier += 0.5f;
            }
        }
        
        private async UniTask CheckDishAccuracy()
        {
            int correctIngredients = 0;
            foreach (var ingredient in _ingredients)
            {
                if (_dish.RequireIngredients.Contains(ingredient) || _dish.AdditionalScoreIngredients.Contains(ingredient))
                {
                    correctIngredients++;
                }
            }

            float accuracy = (float)correctIngredients / (float)_ingredients.Count * 100;
            float visibleAccuracy = 0;
            View.AccuracyDishText.gameObject.SetActive(true);
            do
            {
                View.AccuracyDishText.text = visibleAccuracy.ToString("0.") + "%";
                visibleAccuracy += 1;
                await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
            } while (visibleAccuracy <= accuracy);
            
            if (accuracy < 50)
            {
                _isLose = true;
            }

            if (accuracy < 75)
            {
                _accuracyScoreMultiplier = 0.5f;
            }

            if (accuracy < 100)
            {
                _accuracyScoreMultiplier = 1f;
            }

            if (accuracy >= 100)
            {
                _accuracyScoreMultiplier = 2f;
            }
        }

        private async UniTask CalculateScore()
        {
            View.ScoreText.gameObject.SetActive(true);
            var realScore = _score * (_accuracyScoreMultiplier * _timeScoreMultiplier * _additionalScoreMultiplier);
            var scoreForStars = _chapterConfig.GetScoreForStarsByDishName(_dishName);
            int stars = 0;
            int visibleScore = 0;
            do
            {
                View.ScoreText.text = visibleScore.ToString("0.");
                visibleScore += 1;
                if (stars < scoreForStars.Count && scoreForStars[stars] == visibleScore)
                {
                    View.StarsImages[stars++].color = Color.white;
                }
                await UniTask.Delay(TimeSpan.FromMilliseconds(0.05f));
            } while (visibleScore <= realScore);
            _dataManager.UserProfileData.ChapterInfoModel.SetHighscore(_chapter, _level, visibleScore, stars);
        }

        public override async UniTask OnHide()
        {
            await base.OnHide();
            View.PositiveIngredientImagesPool.ReturnAll();
            View.StoredIngredientImagesPool.ReturnAll();
            View.AdditionalScoreIngredientImagesPool.ReturnAll();
            foreach (var starsImage in View.StarsImages)
            {
                starsImage.color = Color.black;
            }
            View.ScoreText.text = String.Empty;
            _score = 0;
        }
    }
}