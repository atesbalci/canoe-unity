using Canoe.Game.Models;
using Canoe.Game.Views.Data;
using UnityEngine;
using Zenject;

namespace Canoe.Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Sprite[] _playerSprites;
        
        public override void InstallBindings()
        {
            Container.BindInstance(new Boat(1f));
            Container.BindInstance(new PlayerViewData(_playerSprites));
        }
    }
}