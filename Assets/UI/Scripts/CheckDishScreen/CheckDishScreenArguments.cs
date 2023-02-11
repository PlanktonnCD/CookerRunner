using System.Collections.Generic;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;

namespace UI.Scripts.CheckDishScreen
{
    public class CheckDishScreenArguments : UIArgumentsForPanels
    {
        public DishName DishName => _dishName;
        private DishName _dishName;
        public List<IngredientsName> Ingredients => _ingredients;
        private List<IngredientsName> _ingredients;
        public int Score => _score;
        private int _score;

        public CheckDishScreenArguments(List<IngredientsName> ingredients, DishName dishName, int score)
        {
            _score = score;
            _ingredients = ingredients;
            _dishName = dishName;
        }
    }
}