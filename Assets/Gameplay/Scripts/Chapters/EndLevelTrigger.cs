using System;
using Gameplay.Scripts.Player;
using UnityEngine;

namespace Gameplay.Scripts.Chapters
{
    public class EndLevelTrigger : MonoBehaviour
    {
        private bool _isTriggered;
        private void OnTriggerEnter(Collider other)
        {
            if(_isTriggered == true) return;
            if (other.TryGetComponent(out PlayerIngredientsStorage player))
            {
                _isTriggered = true;
                player.GoToCheckDish();
            }
        }
    }
}