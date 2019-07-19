using Canoe.Game.Behaviours;
using Canoe.Game.Models.Data;
using UnityEngine;
using Zenject;

namespace Canoe.Game.Controllers
{
    public class GameController : ITickable
    {
        private readonly GameState _gameState;
        private readonly BoatBehaviour _boatBehaviour;
        private readonly float _boatStartZ;

        public GameController(GameState gameState, BoatBehaviour boatBehaviour)
        {
            _gameState = gameState;
            _boatBehaviour = boatBehaviour;
            _boatStartZ = boatBehaviour.transform.position.z;
        }
        
        public void Tick()
        {
            _gameState.Tick(Time.deltaTime, _boatBehaviour.transform.position.z - _boatStartZ);
        }
    }
}