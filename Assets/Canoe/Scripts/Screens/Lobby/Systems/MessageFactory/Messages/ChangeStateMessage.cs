using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Screens.Lobby.Systems.MessageFactory.Messages
{
    public class ChangeStateMessage : Message
    {
        public bool isReady;

        public ChangeStateMessage(bool isReady) : base(MessageCodes.ChangeState)
        {
            this.isReady = isReady;
        }
    }
}