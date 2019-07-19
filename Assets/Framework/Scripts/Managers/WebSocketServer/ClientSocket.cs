using System;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Framework.Scripts.Managers.WebSocketServer
{
    public class ClientSocket : WebSocketBehavior
    {
        public string DeviceId { get; private set; }
        private readonly WebSocketServerManager _manager;

        public ClientSocket(WebSocketServerManager manager)
        {
            _manager = manager;
        }

        protected override void OnOpen()
        {
            DeviceId = QueryString.Get("device_id");
            var isReconnect = _manager.DeviceIds.Contains(DeviceId);
            if (!isReconnect) _manager.DeviceIds.Add(DeviceId);
            
            _manager.ClientSockets.Add(this);
            UnityMainThreadDispatcher.Instance().Enqueue(() => _manager.OnPlayerConnect?.Invoke(this, isReconnect));
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _manager.ClientSockets.Remove(this);
            UnityMainThreadDispatcher.Instance().Enqueue(() => _manager.OnPlayerDisconnect?.Invoke(this));
        }

        protected override void OnError(ErrorEventArgs e)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => _manager.OnErrorOccur?.Invoke(this));
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsPing) return;

            var message = JsonUtility.FromJson<Message>(e.Data);
            UnityMainThreadDispatcher.Instance()
                .Enqueue(() => _manager.OnMessageReceive?.Invoke(this, message.code, e.Data));
        }

        public String GetId()
        {
            return ID;
        }

        public void SendMessage(Message message)
        {
            Send(JsonUtility.ToJson(message));
        }
    }
}