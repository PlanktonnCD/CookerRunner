using System;
using Gameplay.Scripts.DataProfiling;
using TMPro;
using UnityEngine;
using Zenject;

namespace PlayerData
{
    public class CoinContainer : MonoBehaviour, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        private SignalBus _signalBus;
        private DataManager _dataManager;
        
        [Inject]
        private void Construct(SignalBus signalBus, DataManager dataManager)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CoinChangeSignal>(SetCoinText);
            _dataManager = dataManager;
        }

        private void OnEnable()
        {
            SetCoinsFromData();
        }

        public void SetCoinsFromData()
        {
            //_coinText.text = _dataManager.UserProfileData.CoinsInfoModel.Coins.ToString();
        }

        private void SetCoinText(CoinChangeSignal signal)
        {
            _coinText.text = signal.CoinsCount.ToString();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<CoinChangeSignal>(SetCoinText);
        }
    }
}