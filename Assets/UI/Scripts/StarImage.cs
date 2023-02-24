using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class StarImage : MonoBehaviour
    {
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _unactiveSprite;
        [SerializeField] private Image _image;

        public void DeactivateStar()
        {
            _image.sprite = _unactiveSprite;
        }
        
        public void ActivateStar()
        {
            _image.sprite = _activeSprite;
        }
    }
}