using System;
using Canoe.Game.Models;
using UnityEngine;
using Zenject;

namespace Canoe.Game.Behaviours.RuntimeTesters
{
    public class CanoeTester : MonoBehaviour
    {
        private Player _leftPlayer;
        private Player _rightPlayer;
        
        [Inject]
        public void Initilize(Models.Canoe canoe)
        {
            _leftPlayer = canoe.AddPlayer();
            _rightPlayer = canoe.AddPlayer();
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