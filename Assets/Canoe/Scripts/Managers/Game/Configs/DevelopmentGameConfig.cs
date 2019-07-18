namespace Canoe.Managers.Game.Configs
{
    public class DevelopmentGameConfig : IGameConfig
    {
        public int MinPlayersToStart => 1;

        public int StartGameDelay => 1;
    }
}