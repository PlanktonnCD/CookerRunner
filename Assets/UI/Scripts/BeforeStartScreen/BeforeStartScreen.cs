using UI.UIUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts.BeforeStartScreen
{
    public class BeforeStartScreen : UIScreen
    {
        [field: SerializeField] public ExtendedButton TriggerToStart { get; private set; }
        [field: SerializeField] public Button DishRecipeButton { get; private set; }
    }
}