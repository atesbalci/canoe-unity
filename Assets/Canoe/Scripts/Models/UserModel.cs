using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Models
{
    public class UserModel
    {
        public ClientSocket ClientSocket { get; private set; }
        public int AvatarId { get; set; }

        public UserModel(ClientSocket clientSocket)
        {
            ClientSocket = clientSocket;
        }
    }
}