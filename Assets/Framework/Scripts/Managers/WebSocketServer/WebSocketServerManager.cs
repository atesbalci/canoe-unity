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
        public UnityAction<ClientSocket, bool> OnPlayerConnect;
        public UnityAction<ClientSocket> OnPlayerDisconnect;
        public UnityAction<ClientSocket> OnErrorOccur;
        public UnityAction<ClientSocket, int, String> OnMessageReceive;

        public int port = 81;

        private WebSocketSharp.Server.WebSocketServer _wsServer;
        public List<ClientSocket> ClientSockets { get; private set; }
        public HashSet<string> DeviceIds { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            InitializeServer();
        }

        private void OnDestroy()
        {
            Stop();
        }

        private void InitializeServer()
        {
            _wsServer = new WebSocketSharp.Server.WebSocketServer($"ws://0.0.0.0:{port}");
            _wsServer.AddWebSocketService("/", () => new ClientSocket(this));

            ClientSockets = new List<ClientSocket>();
            DeviceIds = new HashSet<string>();
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