using System;
using Gameplay.Scripts.DataProfiling;
using SolidUtilities.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{
    public class SettingsWindow : UIScreen
    {
        [field: SerializeField] public Button CloseWindowButton { get; private set; }
        [field: SerializeField] public Button GoToMainMenuButton { get; private set; }
        [field: SerializeField] public SerializableDictionary<SettingType, Button> SettingButtons { get; private set; }
    }
}