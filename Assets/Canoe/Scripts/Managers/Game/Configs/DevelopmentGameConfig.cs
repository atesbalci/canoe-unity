namespace Canoe.Managers.Game.Configs
{
    public class DevelopmentGameConfig : IGameConfig
    {
        public int MaxPlayers => 4;
        public int MinPlayersToStart => 2;
        public int StartGameDelay => 1;
    }
}