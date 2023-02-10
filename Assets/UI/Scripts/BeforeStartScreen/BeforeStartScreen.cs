using UI.UIUtils;
using UnityEngine;

namespace UI.Scripts.BeforeStartScreen
{
    public class BeforeStartScreen : UIScreen
    {
        [field: SerializeField] public ExtendedButton TriggerToStart { get; private set; }
    }
}