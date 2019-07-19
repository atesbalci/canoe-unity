using Framework.Scripts.Managers.WebSocketServer;

namespace Canoe.Models
{
    public class UserModel
    {
        public ClientSocket ClientSocket { get; private set; }
        public int AvatarId { get; set; }
        public bool IsReady { get; set; }

        public UserModel(ClientSocket clientSocket)
        {
            ClientSocket = clientSocket;
        }

        public void UpdateClientSocketForReconnect(ClientSocket clientSocket)
        {
            ClientSocket = clientSocket;
        }
    }
}