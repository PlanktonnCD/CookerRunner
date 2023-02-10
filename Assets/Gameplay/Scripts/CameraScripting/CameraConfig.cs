using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Gameplay.Scripts.CameraScripting
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Installers/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [SerializeField] private SerializableDictionaryBase<CameraSide, Vector3> _cameraSideReferenceData;

        [SerializeField] private float _cameraMoveDuration;
        public float CameraMoveDuration => _cameraMoveDuration;

        public Vector3 GetPositionForCamera(CameraSide cameraSide)
        {
            _cameraSideReferenceData.TryGetValue(cameraSide, out Vector3 position);
            return position;
        }
    }

    [Serializable]
    public struct CameraReferenceForFortView
    {
        public Vector3 CameraPosition;
        public Vector3 CameraRotation;
    }
}