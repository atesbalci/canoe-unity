namespace Canoe.Managers.Game.Configs
{
    public class ProductionGameConfig : IGameConfig
    {
        public int MaxPlayers => 8;
        public int MinPlayersToStart => 2;
        public int StartGameDelay => 5;
    }
}