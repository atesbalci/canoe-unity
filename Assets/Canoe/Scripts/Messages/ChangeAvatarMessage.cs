using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Messages
{
    public class ChangeAvatarMessage : Message
    {
        public int avatarId;
        
        public ChangeAvatarMessage(int code) : base(MessageCodes.ChangeAvatar)
        {
        }
    }
}