using System;
using System.Collections.Generic;
using TypeReferences;
using UI;
using UnityEngine;

[CreateAssetMenu(fileName = "UiConfig", menuName = "Installers/UiConfig")]
public class UIConfig : ScriptableObject
{
    [SerializeField] private List<ScreenReference> _uiScreens;
    
    public GameObject GetUIPrefab<T>() where T : UIScreenController
    {
        foreach (var screen in _uiScreens)
        {
            if (screen.ClassReference.Type == typeof(T))
            {
                return screen.Prefab;
            }
        }

        return null;
    }

    [Serializable]
    public class ScreenReference
    {
        public GameObject Prefab;

        [Inherits(typeof(UIScreenController))] 
        public TypeReference ClassReference;
    }
}