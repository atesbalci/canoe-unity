using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Framework.Scripts.Managers.WebSocketServer
{
    public class WebSocketServerManager : BaseManager
    {
        public UnityAction OnPlayerConnect;
        public UnityAction OnPlayerDisconnect;
        public UnityAction OnErrorOccur;
        public UnityAction<ClientSocket, int, String> OnMessageReceive;

        public int port = 81;

        private WebSocketSharp.Server.WebSocketServer _wsServer;
        private ClientSocket[] _clientSockets;

        protected override void Awake()
        {
            base.Awake();

            InitializeServer();
        }

        private void InitializeServer()
        {
            _wsServer = new WebSocketSharp.Server.WebSocketServer($"ws://0.0.0.0:{port}");
            _wsServer.AddWebSocketService("/", () => new ClientSocket(this));

            _clientSockets = new ClientSocket[8];
        }

        public void Start()
        {
            _wsServer?.Start();
        }

        public void Stop()
        {
            _wsServer?.Stop();
        }
    }
}