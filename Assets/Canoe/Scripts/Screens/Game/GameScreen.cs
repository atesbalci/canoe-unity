using System;
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

        private MessageFactorySystem _messageFactorySystem;

        protected override void Awake()
        {
            base.Awake();

            _wssManager = FindObjectOfType<WebSocketServerManager>();
            _wssManager.OnMessageReceive += OnMessageReceive;

            _messageFactorySystem = FindObjectOfType<MessageFactorySystem>();

            _messageFactorySystem.OnSwipeMessage += OnSwipeMessage;
        }

        private void OnDestroy()
        {
            _wssManager.OnMessageReceive -= OnMessageReceive;
            
            _messageFactorySystem.OnSwipeMessage -= OnSwipeMessage;
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