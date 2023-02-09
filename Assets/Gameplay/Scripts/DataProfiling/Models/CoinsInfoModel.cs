using Newtonsoft.Json;
using Zenject;

namespace Gameplay.Scripts.DataProfiling.Models
{
    public class CoinsInfoModel : IUserProfileModel
    {
        [JsonIgnore] public int Coins => _coins;
        [JsonProperty] private int _coins;
        [JsonIgnore] private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
        }

        public bool TrySubstract(int count)
        {
            var isCoinsNotNull = _coins - count >= 0;
            
            if (isCoinsNotNull)
            {
                _coins -= count;
                _signalBus.Fire(new CoinChangeSignal(_coins));
            }
            return isCoinsNotNull;
        }
        
        public void IncreaseCoins(int count)
        {
            _coins += count;
            _signalBus.Fire(new CoinChangeSignal(_coins));
        }
    }
}