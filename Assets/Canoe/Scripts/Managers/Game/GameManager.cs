using Boo.Lang;
using Canoe.Managers.Game.Configs;
using Canoe.Models;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using Framework.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Canoe.Managers.Game
{
    public class GameManager : BaseManager
    {
        public enum LobbyState
        {
            NotReady,
            NotEnoughPlayers,
            Ready,
        }

        public UnityAction<int, UserModel> OnUserAdd;
        public UnityAction<int, UserModel> OnUserRemove;

        public IGameConfig GameConfig { get; private set; }
        public LobbyState State { get; private set; }
        public UserModel[] Users { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            GameConfig = Application.isEditor ? (IGameConfig) new DevelopmentGameConfig() : new ProductionGameConfig();
            Users = new UserModel[8];
        }

        public void AddUser(ClientSocket clientSocket)
        {
            var i = Users.GetFirstNullPosition();
            if (i == -1) return;

            var user = new UserModel(clientSocket);
            Users[i] = user;
            UpdateLobbyState();
            OnUserAdd?.Invoke(i, user);
        }

        public void RemoveUser(ClientSocket clientSocket)
        {
            var position = FindUserPositionByClientSocket(clientSocket);
            if (position == -1) return;

            var user = Users[position];
            Users[position] = null;
            UpdateLobbyState();
            OnUserRemove?.Invoke(position, user);
        }

        public UserModel FindUserByClientSocket(ClientSocket clientSocket)
        {
            var i = FindUserPositionByClientSocket(clientSocket);
            return i == -1 ? null : Users[i];
        }

        public UserModel FindUserByDeviceId(string deviceId)
        {
            foreach (var user in Users)
            {
                if (user.ClientSocket.DeviceId.Equals(deviceId))
                {
                    return user;
                }
            }

            return null;
        }

        public int FindUserPositionByClientSocket(ClientSocket clientSocket)
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

        public void UpdateLobbyState()
        {
            State = LobbyState.Ready;

            if (Users.CountWithoutNull() < GameConfig.MinPlayersToStart)
            {
                State = LobbyState.NotEnoughPlayers;
            }
            else
            {
                foreach (var user in Users)
                {
                    if (user != null && !user.IsReady)
                    {
                        State = LobbyState.NotReady;
                        break;
                    }
                }
            }
        }

        public List<int> GetAvatarList()
        {
            var avatars = new List<int>();
            
            foreach (var user in Users)
            {
                if (user == null) continue;
                avatars.Add(user.AvatarId);
            }

            return avatars;
        }
    }
}