using System.Collections.Generic;
using Audio;
using Cysharp.Threading.Tasks;
using Gameplay.Scripts.Player;
using Gameplay.Scripts.Player.Ingredients;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.PickUp
{
    public class IngredientPickUp : PickUpItem
    {
        [SerializeField] private TrackName _trackOnPick;
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

            var ingredient = new IngredientObject()
            {
                Name = _ingredientsName,
                Transforms = list,
                IsSlice = _isSlice,
                TrackName = _trackOnPick
            };
            player.PickUpIngredient(ingredient);
        }
    }
}