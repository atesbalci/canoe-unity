using System;
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
        public UnityAction<String> OnMessageReceive;

        public int port = 81;

        private WebSocketSharp.Server.WebSocketServer _wsServer;

        protected override void Awake()
        {
            base.Awake();

            GetLocalIPAddress();

            InitializeServer();
        }

        private void InitializeServer()
        {
            _wsServer = new WebSocketSharp.Server.WebSocketServer($"ws://0.0.0.0:{port}");
            _wsServer.AddWebSocketService("/", () => new GameServer(this));
        }

        public void Start()
        {
            _wsServer?.Start();
        }

        public void Stop()
        {
            _wsServer?.Stop();
        }

        private class GameServer : WebSocketBehavior
        {
            private readonly WebSocketServerManager _manager;

            public GameServer(WebSocketServerManager manager)
            {
                _manager = manager;
            }

            protected override void OnOpen()
            {
                _manager.OnPlayerConnect?.Invoke();
            }

            protected override void OnClose(CloseEventArgs e)
            {
                _manager.OnPlayerDisconnect?.Invoke();
            }

            protected override void OnError(ErrorEventArgs e)
            {
                _manager.OnErrorOccur?.Invoke();
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                _manager.OnMessageReceive?.Invoke(e.Data);
            }
        }

        public void GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.Log(ip.ToString());
                }
            }
        }
    }
}