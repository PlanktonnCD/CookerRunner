using Gameplay.Scripts.Dishes;
using Gameplay.Scripts.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Scripts.Chapters
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _playerStartPos;
        private DiContainer _container;
        private PlayerMovement _player;
        private DishName _dishName;

        [Inject]
        private void Construct(DiContainer container)
        {
            _container = container;
        }
        
        public void Init(DishName dishName)
        {
            _dishName = dishName;
        }
        
        public void SpawnPlayer(PlayerMovement playerPrefab)
        {
            _player = _container.InstantiatePrefab(playerPrefab, _playerStartPos.position, Quaternion.identity, transform ).GetComponent<PlayerMovement>();
            _player.SetOnStartLevel(_dishName);
        }
    }
}