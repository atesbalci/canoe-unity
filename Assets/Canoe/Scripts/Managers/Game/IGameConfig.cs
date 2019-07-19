namespace Canoe.Managers.Game
{
    public interface IGameConfig
    {
        int MinPlayersToStart { get; }
        int StartGameDelay { get; }
    }
}