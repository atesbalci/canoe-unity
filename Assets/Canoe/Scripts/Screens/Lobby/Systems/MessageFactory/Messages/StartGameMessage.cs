using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Screens.Lobby.Systems.MessageFactory.Messages
{
    public class StartGameMessage : Message
    {
        public StartGameMessage() : base(MessageCodes.StartGame)
        {
        }
    }
}