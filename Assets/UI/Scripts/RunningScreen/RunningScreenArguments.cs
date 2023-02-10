using Gameplay.Scripts.Player;

namespace UI.Scripts.RunningScreen
{
    public class RunningScreenArguments : UIArgumentsForPanels
    {
        public PlayerIngredientsStorage PlayerIngredientsStorage => _playerIngredientsStorage;
        private PlayerIngredientsStorage _playerIngredientsStorage;

        public RunningScreenArguments(PlayerIngredientsStorage playerIngredientsStorage)
        {
            _playerIngredientsStorage = playerIngredientsStorage;
        }
    }
}