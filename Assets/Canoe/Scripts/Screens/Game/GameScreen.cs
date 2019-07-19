using System;
using Canoe.Managers.Game;
using Canoe.Messages;
using Canoe.Screens.Game.Systems.MessageFactory;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using UnityEngine;

namespace Canoe.Screens.Game
{
    public class GameScreen : BaseScreen<GameScreenResources>
    {
        private WebSocketServerManager _wssManager;
        private GameManager _gameManager;

        private MessageFactorySystem _messageFactorySystem;

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
            if (!isReconnect) return;

            var user = _gameManager.FindUserByDeviceId(clientSocket.DeviceId);
            if (user == null) return;
            
            clientSocket.SendMessage(new StartGameMessage());
            Debug.Log("reconnect");
        }

        private void OnPlayerDisconnect(ClientSocket clientSocket)
        {
            var user = _gameManager.FindUserByClientSocket(clientSocket);
            if (user == null) return;
            
            Debug.Log("disconnect");
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

            Debug.Log(direction);
        }
    }
}