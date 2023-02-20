using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Player;
using Gameplay.Scripts.Player.Ingredients;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Scripts.PickUp
{
    public class IngredientPickUp : PickUpItem
    {
        [SerializeField] private IngredientsName _ingredientsName;
        [SerializeField] private bool _isSlice;
        [SerializeField] private Transform _1stSlicedPart;
        [SerializeField] private Transform _2stSlicedPart;
        
        protected override async UniTask OnPickUp(PlayerIngredientsStorage player)
        {
            var list = new List<Transform>();
            if (_isSlice)
            {
                list.Add(_1stSlicedPart);
                list.Add(_2stSlicedPart);
            }
            else
            {
                list.Add(_1stSlicedPart);
            }
            
            player.PickUpIngredient(list, _ingredientsName, _isSlice);
        }
    }
}