using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Messages
{
    public class StartGameMessage : Message
    {
        public StartGameMessage() : base(MessageCodes.StartGame)
        {
        }
    }
}