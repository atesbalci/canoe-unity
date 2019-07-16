using System;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Framework.Scripts.Managers.WebSocketServer
{
    public class ClientSocket : WebSocketBehavior
    {
        private readonly WebSocketServerManager _manager;

        public ClientSocket(WebSocketServerManager manager)
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
            if (e.IsPing) return;

            var message = JsonUtility.FromJson<Message>(e.Data);
            _manager.OnMessageReceive?.Invoke(this, message.code, e.Data);
        }

        public String GetId()
        {
            return ID;
        }

        public void SendMessage(Message message)
        {
        }
    }
}