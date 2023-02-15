using System;
using System.Collections.Generic;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using UI;
using UI.Scripts.CheckDishScreen;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Player
{
    public class PlayerIngredientsStorage : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        private int _score;
        private DishName _dishName;
        private List<IngredientsName> _ingredients = new List<IngredientsName>();
        private Action<int> _changeScoreAction;
        private UIManager _uiManager;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public void InitChangeScoreAction(Action<int> changeScoreAction)
        {
            _changeScoreAction = changeScoreAction;
        }

        public void SetCurrentDish(DishName dishName)
        {
            _dishName = dishName;
        }
        
        public void PickUpIngredient(Transform ingredientTransform, IngredientsName ingredientsName)
        {
            _score += 50;
            _changeScoreAction?.Invoke(_score);
            _ingredients.Add(ingredientsName);
        }

        public void GoToCheckDish()
        {
            _playerMovement.StopMove();
            var args = new CheckDishScreenArguments(_ingredients, _dishName, _score);
            _uiManager.Show<CheckDishScreenController>(args);
        }

        public void Release()
        {
            _score = 0;
        }
    }
}