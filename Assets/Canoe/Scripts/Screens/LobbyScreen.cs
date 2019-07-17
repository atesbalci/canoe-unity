using System.Linq;
using System.Threading;
using Canoe.Components;
using Canoe.Managers;
using Canoe.Managers.Game;
using Canoe.Models;
using Canoe.Screens.Lobby.Systems.MessageFactory;
using Canoe.Screens.Lobby.Systems.MessageFactory.Messages;
using Canoe.Screens.Lobby.Systems.UI;
using Framework.Scripts;
using Framework.Scripts.Managers.Pool;
using Framework.Scripts.Managers.WebSocketServer;
using Framework.Scripts.Utils;
using UnityEngine;
using ZXing;

namespace Canoe.Screens
{
    public class LobbyScreen : BaseScreen<LobbyScreenResources>
    {
        private PoolManager _poolManager;
        private WebSocketServerManager _wssManager;
        private GameManager _gameManager;

        private MessageFactorySystem _messageFactorySystem;
        private UISystem _uiSystem;

        protected override void Awake()
        {
            base.Awake();

            _poolManager = FindObjectOfType<PoolManager>();
            _wssManager = FindObjectOfType<WebSocketServerManager>();
            _gameManager = FindObjectOfType<GameManager>();
            
            _wssManager.OnPlayerConnect += OnPlayerConnect;
            _wssManager.OnPlayerDisconnect += OnPlayerDisconnect;
            _wssManager.OnMessageReceive += OnMessageReceive;
            _gameManager.OnUserAdd += OnUserAdd;
            _gameManager.OnUserRemove += OnUserRemove;

            _messageFactorySystem = FindObjectOfType<MessageFactorySystem>();
            _uiSystem = FindObjectOfType<UISystem>();

            _messageFactorySystem.OnChangeAvatarMessage += OnChangeAvatarMessage;
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

        private void OnPlayerConnect(ClientSocket clientSocket)
        {
            Debug.Log("user connected");
            _gameManager.AddUser(clientSocket);
        }

        private void OnPlayerDisconnect(ClientSocket clientSocket)
        {
            Debug.Log("user disconnected");
            _gameManager.RemoveUser(clientSocket);
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

        private void OnUserAdd(int position, UserModel user)
        {
            _uiSystem.AddLobbyPlayerIntoPanel(position, user);
        }
        
        private void OnUserRemove(int position, UserModel user)
        {
            _uiSystem.RemoveLobbyPlayerFromPanel(position, user);
        }
    }
}