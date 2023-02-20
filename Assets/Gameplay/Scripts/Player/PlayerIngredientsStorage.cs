using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Scripts.Animation;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using UI;
using UI.Scripts.CheckDishScreen;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Scripts.Player
{
    public class PlayerIngredientsStorage : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Transform _potTransform;
        [SerializeField] private AnimationEventListener _animationEventListener;
        private int _score;
        private DishName _dishName;
        private List<IngredientsName> _ingredients = new List<IngredientsName>();
        private Action<int> _changeScoreAction;
        private UIManager _uiManager;
        private AnimationController<PlayerAnimationType> _animationController;
        private IngredientObject _currentIngredient;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        public void InitChangeScoreAction(Action<int> changeScoreAction)
        {
            _changeScoreAction = changeScoreAction;
        }

        public void Init(DishName dishName, AnimationController<PlayerAnimationType> animationController)
        {
            _animationController = animationController;
            _dishName = dishName;
            _animationEventListener.OnAnimationEndAction += MoveIngedients;
        }
        
        public void PickUpIngredient(List<Transform> ingredientTransforms, IngredientsName ingredientsName, bool isSlice)
        {
            GetIngredient();
            _currentIngredient = new IngredientObject()
            {
                Name = ingredientsName,
                Transforms = ingredientTransforms
            };
            if (isSlice)
            {
                _animationController.StartAnimation(new Trigger<PlayerAnimationType>(), PlayerAnimationType.Slice);
                return;
            }
            _animationController.StartAnimation(new Trigger<PlayerAnimationType>(), PlayerAnimationType.Toss);
            
        }

        private void MoveIngedients()
        {
            if (_currentIngredient.Transforms.Count == 1)
            {
                MoveIngredient(_currentIngredient.Transforms[0], IngredientFlyingDirection.Center);
                return;
            }
            
            MoveIngredient(_currentIngredient.Transforms[0], IngredientFlyingDirection.Left);
            MoveIngredient(_currentIngredient.Transforms[1], IngredientFlyingDirection.Right);
        }

        private void MoveIngredient(Transform ingredientTransform, IngredientFlyingDirection direction)
        {
            ingredientTransform.parent = transform;
            var seq = DOTween.Sequence();
            switch (direction)
            {
                case IngredientFlyingDirection.Left:
                    ingredientTransform.DOLocalMoveZ(_potTransform.localPosition.z, 2);
                    ingredientTransform.DORotate(Vector3.one*360, 1.5f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                    seq.Append(ingredientTransform.DOLocalMoveY(transform.localPosition.y + 3, 1f).SetEase(Ease.Linear));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x - 1, 1f).SetEase(Ease.Linear));
                    seq.Append(ingredientTransform.DOLocalMoveY(_potTransform.localPosition.y, 1f).SetEase(Ease.Linear));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x , 1f).SetEase(Ease.Linear));
                    break;
                case IngredientFlyingDirection.Center:
                    ingredientTransform.DOLocalMoveZ(_potTransform.localPosition.z, 2).SetEase(Ease.Linear);
                    ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x, 2).SetEase(Ease.Linear);
                    ingredientTransform.DORotate(Vector3.one*360, 1.5f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                    seq.Append(ingredientTransform.DOLocalMoveY(transform.localPosition.y + 3, 1f).SetEase(Ease.Linear));
                    seq.Append(ingredientTransform.DOLocalMoveY(_potTransform.localPosition.y, 1f).SetEase(Ease.Linear));
                    break;
                case IngredientFlyingDirection.Right:
                    ingredientTransform.DOLocalMoveZ(_potTransform.localPosition.z, 2).SetEase(Ease.Linear);
                    ingredientTransform.DORotate(Vector3.one*-360, 1.5f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                    seq.Append(ingredientTransform.DOLocalMoveY(transform.localPosition.y + 3, 1f).SetEase(Ease.Linear));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x + 1, 1f).SetEase(Ease.Linear));
                    seq.Append(ingredientTransform.DOLocalMoveY(_potTransform.localPosition.y, 1f).SetEase(Ease.Linear));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x , 1f).SetEase(Ease.Linear));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
           
            seq.AppendCallback((() =>
            {
                Destroy(ingredientTransform.gameObject);
            }));
        }
        
        private void GetIngredient()
        {
            _score += 50;
            _changeScoreAction?.Invoke(_score);
            _ingredients.Add(_currentIngredient.Name);
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

    public struct IngredientObject
    {
        public IngredientsName Name;
        public List<Transform> Transforms;
    }

    public enum IngredientFlyingDirection
    {
        Left,
        Center,
        Right
    }
}