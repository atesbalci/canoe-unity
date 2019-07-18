using Canoe.Game.Models;
using UnityEngine;
using Zenject;

namespace Canoe.Game.Behaviours.RuntimeTesters
{
    public class BoatTester : MonoBehaviour
    {
        private Player _leftPlayer;
        private Player _rightPlayer;

        private Boat _boat;
        
        [Inject]
        public void Initialize(Boat boat)
        {
            _boat = boat;
        }

        private void Start()
        {
            _leftPlayer = _boat.AddPlayer(0);
            _rightPlayer = _boat.AddPlayer(1);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _leftPlayer.Row(Time.time, false);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _rightPlayer.Row(Time.time, false);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                _leftPlayer.Row(Time.time, true);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                _rightPlayer.Row(Time.time, true);
            }
        }
    }
}