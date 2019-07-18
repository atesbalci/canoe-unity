using System;

namespace Framework.Scripts.Managers.WebSocketServer
{
    [Serializable]
    public class Message
    {
        public int code;

        public Message(int code)
        {
            this.code = code;
        }
    }
}