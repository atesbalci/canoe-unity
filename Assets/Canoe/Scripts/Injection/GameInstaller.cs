using Zenject;

namespace Canoe.Injection
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(new Game.Models.Canoe(1f));
        }
    }
}