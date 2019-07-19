using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Messages
{
    public class SwipeMessage : Message
    {
        public string direction;

        public SwipeMessage() : base(MessageCodes.StartGame)
        {
        }
    }
}