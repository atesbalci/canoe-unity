using Canoe.Game.Models;
using UnityEngine;

namespace Canoe.Game.Views
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Renderer _model;
        [SerializeField] private Transform _row;

        private Player _player;

        public void Bind(Player player)
        {
            _player = player;
            if (_player == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
        }
    }
}