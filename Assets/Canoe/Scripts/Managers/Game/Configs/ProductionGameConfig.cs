namespace Canoe.Managers.Game.Configs
{
    public class ProductionGameConfig : IGameConfig
    {
        public int MinPlayersToStart => 4;

        public int StartGameDelay => 5;
    }
}