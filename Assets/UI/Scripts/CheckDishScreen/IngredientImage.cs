using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.CheckDishScreen
{
    public class IngredientImage : MonoBehaviour
    {
        [SerializeField] private Image _ingredientImage;
        [SerializeField] private Image _successObtainImage;
        [SerializeField] private Image _failObtainImage;

        public void SetIngredientImage(Sprite sprite)
        {
            _ingredientImage.sprite = sprite;
            _ingredientImage.preserveAspect = true;
        }

        public void SetObtainState(bool isObtained)
        {
            if (isObtained == true)
            {

                _successObtainImage.enabled = true;
                _failObtainImage.enabled = false;
                return;
            }

            _successObtainImage.enabled = false;
            _failObtainImage.enabled = true;
        }
    }
}