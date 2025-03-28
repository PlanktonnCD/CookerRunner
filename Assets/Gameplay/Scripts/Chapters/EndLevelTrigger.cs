﻿using System;
using DG.Tweening;
using Gameplay.Scripts.CameraScripting;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player;
using UnityEngine;
using Utils;
using Zenject;

namespace Gameplay.Scripts.Chapters
{
    public class EndLevelTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _dishPosition;
        private bool _isTriggered;
        private DishesConfig _dishesConfig;
        private DataManager _dataManager;
        private ChapterConfig _chapterConfig;

        [Inject]
        private void Construct(DishesConfig dishesConfig, DataManager dataManager, ChapterConfig chapterConfig)
        {
            _chapterConfig = chapterConfig;
            _dataManager = dataManager;
            _dishesConfig = dishesConfig;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(_isTriggered == true) return;
            if (other.TryGetComponent(out PlayerIngredientsStorage player))
            {
                _isTriggered = true;

                var levelInitializer = _chapterConfig.GetLevelByIndex(_dataManager.UserProfileData.ChapterInfoModel.ChosenLevel, _dataManager.UserProfileData.ChapterInfoModel.ChosenChapter);
                var dish = _dishesConfig.GetDishByName(levelInitializer.DishOnLevel);
                var dishObject = Instantiate(dish.ModelPrefab, _dishPosition);
                TransformUtils.SetLayerRecursively(dishObject, LayerMask.NameToLayer("DishPresentation"));
                dishObject.transform.DORotate(Vector3.up * 360, 5, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                player.GoToCheckDish();
            }
        }
    }
}