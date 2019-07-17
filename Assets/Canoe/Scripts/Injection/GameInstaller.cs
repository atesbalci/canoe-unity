using Canoe.Game.Models;
using Zenject;

namespace Canoe.Injection
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(new Boat(1f));
        }
    }
}