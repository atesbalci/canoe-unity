using System;
using System.Collections.Generic;
using Canoe.Game.Models;
using Canoe.Managers.Game;
using Canoe.Messages;
using Canoe.Models;
using Canoe.Screens.Game.Systems.MessageFactory;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using UnityEngine;
using Zenject;

namespace Canoe.Screens.Game
{
    public class GameScreen : BaseScreen<GameScreenResources>
    {
        private WebSocketServerManager _wssManager;
        private GameManager _gameManager;

        private MessageFactorySystem _messageFactorySystem;
        private Boat _boat;
        private IDictionary<UserModel, Player> _playersByUser;

        [Inject]
        public void Initialize(Boat boat)
        {
            _boat = boat;
        }

        protected override void Awake()
        {
            base.Awake();

            _wssManager = FindObjectOfType<WebSocketServerManager>();
            _gameManager = FindObjectOfType<GameManager>();
            _wssManager.OnPlayerConnect += OnPlayerConnect;
            _wssManager.OnPlayerDisconnect += OnPlayerDisconnect;
            _wssManager.OnMessageReceive += OnMessageReceive;

            _messageFactorySystem = FindObjectOfType<MessageFactorySystem>();

            _messageFactorySystem.OnSwipeMessage += OnSwipeMessage;

            _playersByUser = new Dictionary<UserModel, Player>();
            foreach (var user in _gameManager.Users)
            {
                if (user != null)
                {
                    _playersByUser.Add(user, _boat.AddPlayer(user.AvatarId));
                }
            }
        }

        private void OnDestroy()
        {
            _wssManager.OnPlayerConnect -= OnPlayerConnect;
            _wssManager.OnPlayerDisconnect -= OnPlayerDisconnect;
            _wssManager.OnMessageReceive -= OnMessageReceive;

            _messageFactorySystem.OnSwipeMessage -= OnSwipeMessage;
        }

        private void OnPlayerConnect(ClientSocket clientSocket, bool isReconnect)
        {
            if (!isReconnect)
            {
                _wssManager.DeviceIds.Remove(clientSocket.DeviceId);
                clientSocket.CloseSocket();
                return;
            }

            var user = _gameManager.FindUserByDeviceId(clientSocket.DeviceId);
            if (user == null) return;
            
            user.UpdateClientSocketForReconnect(clientSocket);
            _playersByUser[user].Active = true;
            
            var position = _gameManager.FindUserPositionByClientSocket(user.ClientSocket);
            clientSocket.SendMessage(new StartGameMessage(position, _gameManager.GetAvatarList()));
        }

        private void OnPlayerDisconnect(ClientSocket clientSocket)
        {
            var user = _gameManager.FindUserByClientSocket(clientSocket);
            if (user == null) return;

            Debug.Log("player: disconnected");

            _playersByUser[user].Active = false;
        }

        private void OnMessageReceive(ClientSocket clientSocket, int code, string data)
        {
            _messageFactorySystem.Produce(clientSocket, code, data);
        }

        private void OnSwipeMessage(ClientSocket clientSocket, SwipeMessage message)
        {
            var direction = Vector2.zero;

            if (message.direction.Equals("up")) direction = Vector2.up;
            else if (message.direction.Equals("down")) direction = Vector2.down;

            var user = _gameManager.FindUserByClientSocket(clientSocket);
            _playersByUser[user].Row(Time.time, direction == Vector2.down);
        }
    }
}