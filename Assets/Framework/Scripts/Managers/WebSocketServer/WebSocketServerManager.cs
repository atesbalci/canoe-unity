using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Framework.Scripts.Managers.WebSocketServer
{
    public class WebSocketServerManager : BaseManager
    {
        private WebSocketSharp.Server.WebSocketServer _wsServer;
        
        protected override void Awake()
        {
            base.Awake();

            GetLocalIPAddress();
//            Debug.Log(networkAddress);
            
            _wsServer = new WebSocketSharp.Server.WebSocketServer("ws://0.0.0.0:9999");
            _wsServer.AddWebSocketService<Game>("/game");
            _wsServer.Start();
        }

        private class Game : WebSocketBehavior
        {
            protected override void OnOpen()
            {
                Debug.Log("OnOpen: " + ID);
            }

            protected override void OnClose(CloseEventArgs e)
            {
                Debug.Log("OnClose: " + ID);
            }

            protected override void OnError(ErrorEventArgs e)
            {
                Debug.Log(e.Message);
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                Debug.Log(e.Data);
            }
        }

        private void OnDestroy()
        {
            _wsServer.Stop();
        }
        
        public void GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.Log(ip.ToString());
//                    return ip.ToString();
                }
            }
        }
    }
}