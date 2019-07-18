using Canoe.Messages;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using UnityEngine;
using UnityEngine.Events;
using static Canoe.Messages.MessageCodes;

namespace Canoe.Screens.Lobby.Systems.MessageFactory
{
    public class MessageFactorySystem : BaseScreen<LobbyScreenResources>
    {
        public UnityAction<ClientSocket, ChangeAvatarMessage> OnChangeAvatarMessage;
        public UnityAction<ClientSocket, ChangeStateMessage> OnChangeStateMessage;
        
        public void Produce(ClientSocket socket, int code, string data)
        {
            switch (code)
            {
                case ChangeAvatar:
                    OnChangeAvatarMessage?.Invoke(socket, JsonUtility.FromJson<ChangeAvatarMessage>(data));
                    break;
                case ChangeState:
                    OnChangeStateMessage?.Invoke(socket, JsonUtility.FromJson<ChangeStateMessage>(data));
                    break;
                default:
                    Debug.Log("Unknown message");
                    break;
            }
        }
    }
}