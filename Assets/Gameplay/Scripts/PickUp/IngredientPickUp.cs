using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Player;
using Gameplay.Scripts.Player.Ingredients;
using UnityEngine;

namespace Gameplay.Scripts.PickUp
{
    public class IngredientPickUp : PickUpItem
    {
        [SerializeField] private IngredientsName _ingredientsName;
        
        protected override async UniTask OnPickUp(PlayerIngredientsStorage player)
        {
            player.PickUpIngredient(transform, _ingredientsName);
            await base.OnPickUp(player);
        }
    }
}