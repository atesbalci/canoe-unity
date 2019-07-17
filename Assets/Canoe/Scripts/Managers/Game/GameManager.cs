using Canoe.Models;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using Framework.Scripts.Utils;
using UnityEngine.Events;

namespace Canoe.Managers.Game
{
    public class GameManager : BaseManager
    {
        public UnityAction<int, UserModel> OnUserAdd;
        public UnityAction<int, UserModel> OnUserRemove;

        public UserModel[] Users { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Users = new UserModel[8];
        }

        public void AddUser(ClientSocket clientSocket)
        {
            var i = Users.GetFirstAvailablePosition();
            if (i == -1) return;

            var user = new UserModel(clientSocket);
            Users[i] = user;
            OnUserAdd?.Invoke(i, user);
        }

        public void RemoveUser(ClientSocket clientSocket)
        {
            var position = FindUserPositionByClientSocket(clientSocket);
            if (position == -1) return;

            var user = Users[position];
            Users[position] = null;
            OnUserRemove?.Invoke(position, user);
        }

        public UserModel FindUserByClientSocket(ClientSocket clientSocket)
        {
            var i = FindUserPositionByClientSocket(clientSocket);
            return i == -1 ? null : Users[i];
        }

        private int FindUserPositionByClientSocket(ClientSocket clientSocket)
        {
            for (var i = 0; i < Users.Length; i++)
            {
                var user = Users[i];
                if (user != null && user.ClientSocket == clientSocket)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}