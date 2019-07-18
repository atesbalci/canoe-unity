using Canoe.Messages;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using UnityEngine;
using UnityEngine.Events;

namespace Canoe.Screens.Game.Systems.MessageFactory
{
    public class MessageFactorySystem : BaseScreenSystem<GameScreenResources>
    {
        public UnityAction<ClientSocket, SwipeMessage> OnSwipeMessage;

        public void Produce(ClientSocket socket, int code, string data)
        {
            switch (code)
            {
                case MessageCodes.Swipe:
                    OnSwipeMessage?.Invoke(socket, JsonUtility.FromJson<SwipeMessage>(data));
                    break;
                default:
                    Debug.Log("Unknown message");
                    break;
            }
        }
    }
}