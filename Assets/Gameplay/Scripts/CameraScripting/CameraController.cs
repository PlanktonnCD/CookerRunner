using System;
using Configs;
using DG.Tweening;
using Gameplay.Scripts.CameraScripting;
using UniRx;
using UnityEngine;

namespace Gameplay.Scripts.CameraScripting
{
    public class CameraController : IDisposable
    {
        private Camera _camera;
        private CameraConfig _cameraConfig;
        private CompositeDisposable _disposables = new CompositeDisposable();

        public CameraController(CameraConfig cameraConfig)
        {
            _camera = Camera.main;
            _cameraConfig = cameraConfig;
        }

        public void ChangeTargetWithoutAnimation(Transform target, CameraSide cameraSide)
        {
            SetCameraParent(target);
            _camera.transform.localPosition = _cameraConfig.GetPositionForCamera(cameraSide);
            _camera.transform.LookAt(target);
        }

        public void ChangeTarget(Transform target, CameraSide cameraSide)
        {
            _disposables.Clear();
            SetCameraParent(target);
            ChangeCameraPosition(cameraSide);
        }

        public void ChangeCameraPosition(CameraSide cameraSide, bool withLookAt = true)
        {
            _camera.transform.DOLocalMove(_cameraConfig.GetPositionForCamera(cameraSide),
                _cameraConfig.CameraMoveDuration);
            if (withLookAt == false) return;
            _camera.transform.DODynamicLookAt(_camera.transform.parent.position, _cameraConfig.CameraMoveDuration);
        }

        public void SetCameraParent(Transform parent)
        {
            _camera.transform.SetParent(parent);
        }

        public void SetCameraParentPos()
        {
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        public void Dispose()
        {
            _disposables.Clear();
        }
    }
}
