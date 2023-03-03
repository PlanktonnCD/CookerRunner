using System;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Settings
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private AudioManager _audioManager;
        private UIManager _uiManager;

        [Inject]
        private void Construct(UIManager uiManager, AudioManager audioManager)
        {
            _uiManager = uiManager;
            _audioManager = audioManager;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(() =>
            {
                _uiManager.Show<SettingsWindowController>();
            });
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}