using System;
using System.Collections.Generic;
using Gameplay.Scripts.Player.Ingredients;
using SolidUtilities.Collections;
using UnityEngine;

namespace Gameplay.Scripts.Dishes
{
    public class DishesConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionary< DishName,Dish> _dishes;

        public Dish GetDishByName(DishName dishName)
        {
            _dishes.TryGetValue(dishName, out var dish);
            return dish;
        }
    }

    [Serializable]
    public struct Dish
    {
        public string Name;
        public List<IngredientsName> RequireIngredients;
        public List<IngredientsName> AdditionalScoreIngeredients;
    }
}