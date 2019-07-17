using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Screens.Lobby.Systems.MessageFactory.Messages
{
    public class ChangeAvatarMessage : Message
    {
        public int avatarId;
        
        public ChangeAvatarMessage(int code) : base(MessageCodes.ChangeAvatar)
        {
        }
    }
}