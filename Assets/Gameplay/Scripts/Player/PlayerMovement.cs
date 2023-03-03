using System;
using System.Collections.Generic;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Scripts.Animation;
using Gameplay.Scripts.CameraScripting;
using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Input;
using Movement;
using UI;
using UI.Scripts.RunningScreen;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour, IDisposable
    {
        [SerializeField] private GameObject _playerModel;
        [SerializeField] private TrackName _movementSound;
        [SerializeField] private PlayerIngredientsStorage _playerIngredientsStorage;
        [SerializeField] private Animator _animator;

        private AnimationController<PlayerAnimationType> _animationController;
        private float _speed = 9;
        private float _posLimit;
        private const float _posLimitConst = 5f;
        private bool _isRun;
        private bool _itCanMoveLeft = true;
        private bool _itCanMoveRight = true;
        private const float _rotationTime = 0.4f;

        private AudioManager _audioManager;
        private SignalBus _signalBus;
        private UIManager _uiManager;
        private InputController _inputController;
        private CameraController _cameraController;

        [Inject]
        private void Construct(InputController inputController, UIManager uiManager, SignalBus signalBus,
            AudioManager audioManager, CameraController cameraController)
        {
            _cameraController = cameraController;
            _audioManager = audioManager;
            _signalBus = signalBus;
            _uiManager = uiManager;
            _inputController = inputController;

            _signalBus.Subscribe<CanStartRunSignal>(SetItCanRun);

        }

        public virtual void SetOnStartLevel(DishName dishName)
        {
            _animationController = new AnimationController<PlayerAnimationType>(_animator);
            _cameraController.ChangeTargetWithoutAnimation(transform, CameraSide.Behind);
            _playerIngredientsStorage.Init(dishName, _animationController);
            _playerModel.transform.Rotate(0, 180, 0);
            _isRun = false;
        }

        private void SetItCanRun(CanStartRunSignal signal)
        {
            if (signal.CanRun)
            {
                StartRunning();
            }
        }

        public virtual void SubscribeInputEvent()
        {
            //_audioManager.PlaySound(_movementSound, true);
            if (transform.position.x > _posLimit)
            {
                transform.position = new Vector3(_posLimit, transform.position.y, transform.position.z);
            }

            if (transform.position.x < -_posLimit)
            {
                transform.position = new Vector3(-_posLimit, transform.position.y, transform.position.z);
            }

            _inputController.DeltaInputPositionEvent += InputControllerOnDeltaInputPositionEvent;
            _isRun = true;
        }

        public virtual async UniTask StartRunning()
        {
            var args = new RunningScreenArguments(_playerIngredientsStorage);
            _uiManager.Show<RunningScreenController>(args);
            _playerModel.transform.DORotate(Vector3.zero, _rotationTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_rotationTime));
            SubscribeInputEvent();
            _animationController.StartAnimation(new Bool<PlayerAnimationType>(){Value = _isRun}, PlayerAnimationType.Walk);
        }

        private void InputControllerOnDeltaInputPositionEvent(float position)
        {
            var xPlayerPosition = transform.position.x;
            var isPlayerOnLeftCorner = xPlayerPosition + position * Time.deltaTime < -_posLimitConst;
            var isPlayerOnRightCorner = xPlayerPosition + position * Time.deltaTime > _posLimitConst;
            position = isPlayerOnLeftCorner || isPlayerOnRightCorner ? 0 : position;

            var deltaPosition = Vector3.right * position * Time.deltaTime;

            if ((transform.position + deltaPosition).x > xPlayerPosition && _itCanMoveRight == false) return;
            if ((transform.position + deltaPosition).x < xPlayerPosition && _itCanMoveLeft == false) return;
            transform.position += deltaPosition;
        }

        private void FixedUpdate()
        {
            if (_isRun)
            {
                transform.position += Vector3.forward * _speed * Time.deltaTime;
            }
        }

        public void GoToMapCenter()
        {
            transform.DOMoveX(0, 0.5f);
        }

        public void Release()
        {
            //_audioManager.TurnOffSound(_movementSound, true);
            _inputController.DeltaInputPositionEvent -= InputControllerOnDeltaInputPositionEvent;
            _signalBus.TryUnsubscribe<CanStartRunSignal>(SetItCanRun);
            _cameraController.SetDefaultParent();
            _playerIngredientsStorage.Release();
            StopMove();
            gameObject.SetActive(false);
        }

        public virtual void StopMove()
        {
            //_audioManager.TurnOffSound(_movementSound, true);
            _isRun = false;
            _inputController.DeltaInputPositionEvent -= InputControllerOnDeltaInputPositionEvent;
            _animationController.StartAnimation(new Bool<PlayerAnimationType>(){Value = _isRun}, PlayerAnimationType.Walk);
        }

        public void Dispose()
        {
            _inputController?.Dispose();
            _inputController.DeltaInputPositionEvent += InputControllerOnDeltaInputPositionEvent;
            _signalBus.Unsubscribe<CanStartRunSignal>(SetItCanRun);
        }
    }
}