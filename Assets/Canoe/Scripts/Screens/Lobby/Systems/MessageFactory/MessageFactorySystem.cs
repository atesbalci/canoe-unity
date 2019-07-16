using Canoe.Screens.Lobby.Systems.MessageFactory.Messages;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using UnityEngine;
using UnityEngine.Events;
using static Canoe.Screens.Lobby.Systems.MessageFactory.MessageCodes;

namespace Canoe.Screens.Lobby.Systems.MessageFactory
{
    public class MessageFactorySystem : BaseScreen<LobbyScreenResources>
    {
        public UnityAction<ClientSocket, ChangeAvatarMessage> OnChangeAvatarMessage;

        protected override void Awake()
        {
            base.Awake();
        }

        public void Produce(ClientSocket socket, int code, string data)
        {
            switch (code)
            {
                case ChangeAvatar:
                    var message = JsonUtility.FromJson<ChangeAvatarMessage>(data);
                    OnChangeAvatarMessage?.Invoke(socket, message);
                    break;
                default:
                    Debug.Log("Unknown message");
                    break;
            }
        }
    }
}