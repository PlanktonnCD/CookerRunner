using System;
using Gameplay.Scripts.Player;
using UnityEngine;

namespace Gameplay.Scripts.Chapters
{
    public class EndLevelTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerIngredientsStorage player))
            {
                player.GoToCheckDish();
            }
        }
    }
}