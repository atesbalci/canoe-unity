using System.Linq;
using Canoe.Screens.Lobby.Systems.MessageFactory;
using Canoe.Screens.Lobby.Systems.MessageFactory.Messages;
using Canoe.Screens.Lobby.Systems.UI;
using Framework.Scripts;
using Framework.Scripts.Managers.WebSocketServer;
using Framework.Scripts.Utils;
using UnityEngine;
using ZXing;

namespace Canoe.Screens
{
    public class LobbyScreen : BaseScreen<LobbyScreenResources>
    {
        private WebSocketServerManager _wssManager;

        private MessageFactorySystem _messageFactorySystem;
        private UISystem _uiSystem;

        protected override void Awake()
        {
            base.Awake();

            _wssManager = FindObjectOfType<WebSocketServerManager>();
            _wssManager.OnPlayerConnect = OnPlayerConnect;
            _wssManager.OnPlayerDisconnect = OnPlayerDisconnect;
            _wssManager.OnMessageReceive = OnMessageReceive;

            _messageFactorySystem = FindObjectOfType<MessageFactorySystem>();
            _uiSystem = FindObjectOfType<UISystem>();
            
            _messageFactorySystem.OnChangeAvatarMessage = OnChangeAvatarMessage;
        }

        private void Start()
        {
            _wssManager.Start();

            SetQrCode();
        }

        private void OnDestroy()
        {
            _wssManager.Stop();
        }

        private void SetQrCode()
        {
            var url = $"http://{NetworkHelper.GetLocalIpAddresses().First()}/Canoe/index.html";
            var texture = Barcode.Generate(url, BarcodeFormat.QR_CODE, 512, 512);
            _uiSystem.ChangeBarcodeImage(texture);
        }

        private void OnPlayerConnect()
        {
            Debug.Log("user connected");
        }

        private void OnPlayerDisconnect()
        {
            Debug.Log("user disconnected");
        }

        private void OnMessageReceive(ClientSocket socket, int code, string data)
        {
            Debug.Log("received");
            _messageFactorySystem.Produce(socket, code, data);
        }
        
        private void OnChangeAvatarMessage(ClientSocket socket, ChangeAvatarMessage message)
        {
            Debug.Log("change avatar");
        }
    }
}