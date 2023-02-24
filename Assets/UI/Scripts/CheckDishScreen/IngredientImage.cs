using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pool;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.CheckDishScreen
{
    public class IngredientImage : MonoBehaviour, IPoolable
    {
        [SerializeField] private Image _ingredientImage;
        [SerializeField] private Image _successObtainImage;
        [SerializeField] private Image _failObtainImage;

        public void SetIngredientImage(Sprite sprite)
        {
            _ingredientImage.sprite = sprite;
            _ingredientImage.preserveAspect = true;
        }

        public async UniTask SetObtainState(bool isObtained)
        {
            if (isObtained == true)
            {
                if(_successObtainImage.enabled == true) return;
                
                _successObtainImage.enabled = true;
                _successObtainImage.transform.DOScale(Vector3.one, 0.5f).From(Vector3.one * 1.5f).SetEase(Ease.Linear);
                _failObtainImage.enabled = false;
                await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
                return;
            }
            
            if(_failObtainImage.enabled == true) return;
            _successObtainImage.enabled = false;
            _failObtainImage.enabled = true;
            _failObtainImage.transform.DOScale(Vector3.one, 0.5f).From(Vector3.one * 1.5f).SetEase(Ease.Linear);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
        }

        public void Release()
        {
            _successObtainImage.enabled = false;
            _failObtainImage.enabled = false;
        }
    }
}