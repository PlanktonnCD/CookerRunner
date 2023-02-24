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
using UI.Scripts.LoseWindow;
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
        private Action _chefHelpAction;
        private float _additionalAccuracy;

        [Inject]
        private void Construct(DishesConfig dishesConfig, IngredientsConfig ingredientsConfig, ChapterConfig chapterConfig, UIManager uiManager, DataManager dataManager)
        {
            _dataManager = dataManager;
            _chapterConfig = chapterConfig;
            _uiManager = uiManager;
            _dishesConfig = dishesConfig;
            _ingredientsConfig = ingredientsConfig;
        }

        public override void Init(UIScreen uiScreen)
        {
            base.Init(uiScreen);
            _chefHelpAction += ChefHelp;
            View.ComebackButton.onClick.AddListener((() =>
            {
                _dataManager.UserProfileData.ChapterInfoModel.TryIncreaseChapterLevelIndex();
                _uiManager.Show<MainMenuScreenController>();
            }));
        }

        public override void Display(UIArgumentsForPanels arguments)
        {
            base.Display(arguments);
            var args = (CheckDishScreenArguments)arguments;
            _additionalAccuracy = 0;
            _dishName = args.DishName;
            _dish = _dishesConfig.GetDishByName(_dishName);
            _ingredients = args.Ingredients;
            _score = args.Score;
            _chapter = _dataManager.UserProfileData.ChapterInfoModel.ChosenChapter;
            _level = _dataManager.UserProfileData.ChapterInfoModel.ChosenLevel;
            View.DishNameText.text = _dish.Name;
            SetIngredientsImages(View.PositiveIngredientImagesPool, _dish.RequireIngredients);
            SetIngredientsImages(View.AdditionalScoreIngredientImagesPool, _dish.AdditionalScoreIngredients);
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
            _isLose = false;
            await CheckExistenceRequiredIngredients();
            if (_isLose == true)
            {
                ToLoseWindow();
                return;
            }
            await CalculateAdditionalIngredientsMultiplier();
            await CheckDishAccuracy();
            if (_isLose == true)
            {
                ToLoseWindow();
                return;
            }
            await CalculateScore();
            View.ComebackButton.gameObject.SetActive(true);
        }

        private void SetIngredientsImages(MonoBehaviourPool<IngredientImage> pool, List<IngredientsName> ingredientsNames)
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
            float accuracy = (((float)correctIngredients / (float)_ingredients.Count) * 100) + _additionalAccuracy;
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
            var levelInitializer = _chapterConfig.GetLevelByDishName(_dishName);
            int stars = 0;
            int visibleScore = 0;
            do
            {
                View.ScoreText.text = visibleScore.ToString("0.");
                visibleScore += 1;
                if (stars < levelInitializer.ScoreForStars.Count && levelInitializer.ScoreForStars[stars] == visibleScore)
                {
                    View.StarsImages[stars++].ActivateStar();
                }
                await UniTask.Delay(TimeSpan.FromMilliseconds(0.05f));
            } while (visibleScore <= realScore);
            _dataManager.UserProfileData.ChapterInfoModel.SetHighscore(_chapter, _level, visibleScore, stars);
        }

        private void ChefHelp()
        {
            var dictionary = new Dictionary<IngredientsName, bool>();
            foreach (var requireIngredient in _dish.RequireIngredients)
            {
                dictionary.Add(requireIngredient, _ingredients.Contains(requireIngredient));
            }

            foreach (var ingredient in dictionary)
            {
                if (ingredient.Value == false)
                {
                    _ingredients.Add(ingredient.Key);
                }
            }
            
            int correctIngredients = 0;
            foreach (var ingredient in _ingredients)
            {
                if (_dish.RequireIngredients.Contains(ingredient) || _dish.AdditionalScoreIngredients.Contains(ingredient))
                {
                    correctIngredients++;
                }
            }
            
            float accuracy = (((float)correctIngredients / (float)_ingredients.Count) * 100);
            
            if (accuracy < 50)
            {
                _additionalAccuracy = 50;
            }
            
            CheckDish();
        }

        private void ToLoseWindow()
        {
            var args = new LoseWindowArguments(_chefHelpAction);
            _uiManager.Show<LoseWindowController>(args);
        }
        
        public override async UniTask OnHide()
        {
            await base.OnHide();
            View.PositiveIngredientImagesPool.ReturnAll();
            View.AdditionalScoreIngredientImagesPool.ReturnAll();
            View.PositiveIngredientImagesPool.ReleaseAll();
            View.AdditionalScoreIngredientImagesPool.ReleaseAll();
            foreach (var starsImage in View.StarsImages)
            {
                starsImage.DeactivateStar();
            }
            View.ScoreText.text = String.Empty;
            _score = 0;
        }
    }
}