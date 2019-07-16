using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Screens.Lobby.Systems.MessageFactory.Messages
{
    public class ChangeAvatarMessage : Message
    {
        public ChangeAvatarMessage(int code) : base(MessageCodes.ChangeAvatar)
        {
        }
    }
}