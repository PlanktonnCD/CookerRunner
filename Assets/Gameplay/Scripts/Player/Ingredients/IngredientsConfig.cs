using System;
using SolidUtilities.Collections;
using UnityEngine;

namespace Gameplay.Scripts.Player.Ingredients
{
    public class IngredientsConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<IngredientsName, Ingredient> _ingredients;

        public Ingredient GetIngredientByName(IngredientsName ingredientsName)
        {
            _ingredients.TryGetValue(ingredientsName, out var ingredient);
            return ingredient;
        }
    }

    [Serializable]
    public struct Ingredient
    {
        public Sprite Sprite;
    }
}