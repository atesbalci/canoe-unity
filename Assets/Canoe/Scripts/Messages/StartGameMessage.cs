using Boo.Lang;
using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Messages
{
    public class StartGameMessage : Message
    {
        public int position;
        public int[] avatars;

        public StartGameMessage(int position, List<int> avatars) : base(MessageCodes.StartGame)
        {
            this.position = position;
            this.avatars = avatars.ToArray();
        }
    }
}