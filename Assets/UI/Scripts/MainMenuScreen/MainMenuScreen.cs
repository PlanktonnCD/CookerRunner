using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Scripts.MainMenuScreen
{
    public class MainMenuScreen : UIScreen
    {
        [field: SerializeField] public List<Chapter> Chapters { get; private set; }
        [field: SerializeField] public TextMeshProUGUI StarsCountText { get; private set; }
    }
}