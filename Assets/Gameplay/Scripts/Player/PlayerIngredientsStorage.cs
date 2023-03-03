using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Animation;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player.Ingredients;
using Particles;
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
        private AudioManager _audioManager;
        private ParticleManager _particleManager;

        [Inject]
        private void Construct(UIManager uiManager, AudioManager audioManager, ParticleManager particleManager)
        {
            _particleManager = particleManager;
            _audioManager = audioManager;
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
        
        public void PickUpIngredient(IngredientObject ingredientObject)
        {
            _currentIngredient = ingredientObject;
            GetIngredient();
            if (_currentIngredient.IsSlice)
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
                _audioManager.PlaySound(TrackName.throw_sound);
                MoveSolidIngredient();
                return;
            }
            
            _audioManager.PlaySound(_currentIngredient.TrackName);
            if (_currentIngredient.IsNeedParticle)
            {
                _particleManager.PlayParticleInPosition(_currentIngredient.ParticleType, _currentIngredient.Transforms[0].position, Quaternion.identity, Vector3.one*0.5f); 
            }

            MoveIngredient(_currentIngredient.Transforms[0], IngredientFlyingDirection.Left);
            MoveIngredient(_currentIngredient.Transforms[1], IngredientFlyingDirection.Right);
        }

        private async void MoveSolidIngredient()
        {
            var ingr = _currentIngredient;
            await MoveIngredient(_currentIngredient.Transforms[0], IngredientFlyingDirection.Center);
            _audioManager.PlaySound(ingr.TrackName);
        }
        
        private async UniTask MoveIngredient(Transform ingredientTransform, IngredientFlyingDirection direction)
        {
            ingredientTransform.parent = transform;
            var xDelta = Random.Range(-0.5f, 0.5f);
            var seq = DOTween.Sequence();
            ingredientTransform.DOScale(ingredientTransform.localScale * 0.5f, 2).SetEase(Ease.Linear);
            switch (direction)
            {
                case IngredientFlyingDirection.Left:
                    ingredientTransform.DOLocalMoveZ(_potTransform.localPosition.z, 2).SetEase(Ease.OutCirc);
                    ingredientTransform.DORotate(Vector3.one*360, 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                    seq.Append(ingredientTransform.DOLocalMoveY(transform.localPosition.y + 3, 1f).SetEase(Ease.OutCirc));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x - 1 + xDelta, 1f).SetEase(Ease.Linear));
                    seq.Append(ingredientTransform.DOLocalMoveY(_potTransform.localPosition.y+0.25f, 1f).SetEase(Ease.InCirc));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x-0.2f , 1f).SetEase(Ease.Linear));
                    break;
                case IngredientFlyingDirection.Center:
                    ingredientTransform.DOLocalMoveZ(_potTransform.localPosition.z, 2).SetEase(Ease.OutCirc);
                    ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x, 2).SetEase(Ease.Linear);
                    ingredientTransform.DORotate(Vector3.one*360, 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                    seq.Append(ingredientTransform.DOLocalMoveY(transform.localPosition.y + 3, 1f).SetEase(Ease.OutCirc));
                    seq.Append(ingredientTransform.DOLocalMoveY(_potTransform.localPosition.y+0.25f, 1f).SetEase(Ease.InCirc));
                    break;
                case IngredientFlyingDirection.Right:
                    ingredientTransform.DOLocalMoveZ(_potTransform.localPosition.z, 2).SetEase(Ease.OutCirc);
                    ingredientTransform.DORotate(Vector3.one*-360, 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                    seq.Append(ingredientTransform.DOLocalMoveY(transform.localPosition.y + 3, 1f).SetEase(Ease.OutCirc));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x + 1+ xDelta, 1f).SetEase(Ease.Linear));
                    seq.Append(ingredientTransform.DOLocalMoveY(_potTransform.localPosition.y+0.25f, 1f).SetEase(Ease.InCirc));
                    seq.Join(ingredientTransform.DOLocalMoveX(_potTransform.localPosition.x+0.2f , 1f).SetEase(Ease.Linear));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
           
            seq.AppendCallback((() =>
            {
                _particleManager.PlayParticleInTransform(ParticleType.water_splash, _potTransform, Quaternion.Euler(Vector3.left*90));
                Destroy(ingredientTransform.gameObject);
            }));
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
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
        public bool IsSlice;
        public List<Transform> Transforms;
        public TrackName TrackName;
        public bool IsNeedParticle;
        public ParticleType ParticleType;
    }

    public enum IngredientFlyingDirection
    {
        Left,
        Center,
        Right
    }
}