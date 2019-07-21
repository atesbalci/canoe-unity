namespace Canoe.Managers.Game
{
    public interface IGameConfig
    {
        int MaxPlayers { get; }
        int MinPlayersToStart { get; }
        int StartGameDelay { get; }
    }
}