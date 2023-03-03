using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Settings
{
    public class SettingsWindowAnimation : UIAnimation
    {
        [SerializeField] private Image _mainPanel;
        [SerializeField] private List<Image> _images;
        [SerializeField] private List<TextMeshProUGUI> _texts;
        public override UniTask ShowAnimation()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask HideAnimation()
        {
            return UniTask.CompletedTask;
        }
    }
}