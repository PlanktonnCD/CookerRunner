using System.Collections.Generic;
using UnityEngine;

namespace UI.Scripts.MainMenuScreen
{
    public class MainMenuScreen : UIScreen
    {
        [field: SerializeField] public List<Chapter> Chapters { get; private set; }
    }
}