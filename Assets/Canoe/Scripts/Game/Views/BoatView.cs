using Canoe.Game.Models;
using UnityEngine;
using Zenject;

namespace Canoe.Game.Views
{
    public class BoatView : MonoBehaviour
    {
        [SerializeField] private PlayerView[] _leftPlayerViews;
        [SerializeField] private PlayerView[] _rightPlayerViews;

        private Boat _boat;

        [Inject]
        public void Initialize(Boat boat)
        {
            _boat = boat;
            foreach (var playerView in _leftPlayerViews)
            {
                playerView.Bind(null);
            }

            foreach (var playerView in _rightPlayerViews)
            {
                playerView.Bind(null);
            }
            
            _boat.OnPlayerAdded += (side, index, player) =>
            {
                var viewList = side == CanoeSide.Left ? _leftPlayerViews : _rightPlayerViews;
                viewList[index].Bind(player);
            };
        }
    }
}