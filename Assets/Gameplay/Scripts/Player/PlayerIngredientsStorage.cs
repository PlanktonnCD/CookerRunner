using System;
using System.Collections.Generic;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using UnityEngine;

namespace Gameplay.Scripts.Player
{
    public class PlayerIngredientsStorage : MonoBehaviour
    {
        private DishName _dishName;
        private List<IngredientsName> _ingredients = new List<IngredientsName>();
        private Action<int> _changeScoreAction;

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
            _changeScoreAction?.Invoke(200);
            _ingredients.Add(ingredientsName);
        }
        
        
    }
}