using Canoe.Game.Behaviours;
using Canoe.Game.Controllers;
using Canoe.Game.Models;
using Canoe.Game.Models.Data;
using Canoe.Game.Views.Data;
using UnityEngine;
using Zenject;

namespace Canoe.Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private BoatBehaviour _boatBehaviour;
        [SerializeField] private Sprite[] _playerSprites;
        
        public override void InstallBindings()
        {
            Container.BindInstance(new Boat(1f));
            Container.BindInstance(new PlayerViewData(_playerSprites));
            Container.BindInstance(_boatBehaviour);
            Container.Bind<GameState>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
        }
    }
}