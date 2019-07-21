using System.Linq;
using Canoe.Managers.Game;
using Canoe.Messages;
using Canoe.Models;
using Canoe.Screens.Lobby.Systems.MessageFactory;
using Canoe.Screens.Lobby.Systems.UI;
using Framework.Scripts;
using Framework.Scripts.Managers.Pool;
using Framework.Scripts.Managers.WebSocketServer;
using Framework.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZXing;

namespace Canoe.Screens.Lobby
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
            _messageFactorySystem.OnChangeStateMessage += OnChangeStateMessage;
            _uiSystem.OnCountingFinish += OnCountingFinish;
        }

        private void Start()
        {
            _wssManager.Start();

            SetQrCode();
            _uiSystem.UpdateLobbyState(_gameManager.State);
        }

        private void OnDestroy()
        {
            _wssManager.OnPlayerConnect -= OnPlayerConnect;
            _wssManager.OnPlayerDisconnect -= OnPlayerDisconnect;
            _wssManager.OnMessageReceive -= OnMessageReceive;
            _gameManager.OnUserAdd -= OnUserAdd;
            _gameManager.OnUserRemove -= OnUserRemove;

            _messageFactorySystem.OnChangeAvatarMessage -= OnChangeAvatarMessage;
            _messageFactorySystem.OnChangeStateMessage -= OnChangeStateMessage;
            _uiSystem.OnCountingFinish -= OnCountingFinish;
        }

        private void SetQrCode()
        {
            var url = $"http://{NetworkHelper.GetLocalIpAddresses().First()}/Canoe/index.html";
            var texture = Barcode.Generate(url, BarcodeFormat.QR_CODE, 512, 512);
            _uiSystem.ChangeBarcodeImage(texture);
        }

        private void OnPlayerConnect(ClientSocket clientSocket, bool isReconnect)
        {
            if (_gameManager.Users.CountWithoutNull() >= _gameManager.GameConfig.MaxPlayers)
            {
                clientSocket.CloseSocket();
                return;
            }

            _gameManager.AddUser(clientSocket);
        }

        private void OnPlayerDisconnect(ClientSocket clientSocket)
        {
            var user = _gameManager.FindUserByClientSocket(clientSocket);
            if (user == null) return;

            _gameManager.RemoveUser(clientSocket);
        }

        private void OnMessageReceive(ClientSocket socket, int code, string data)
        {
            _messageFactorySystem.Produce(socket, code, data);
        }

        private void OnUserAdd(int position, UserModel user)
        {
            _uiSystem.AddLobbyPlayerIntoPanel(position, user);
            _uiSystem.UpdateLobbyState(_gameManager.State);
        }

        private void OnUserRemove(int position, UserModel user)
        {
            _uiSystem.RemoveLobbyPlayerFromPanel(position, user);
            _uiSystem.UpdateLobbyState(_gameManager.State);
        }

        private void OnChangeAvatarMessage(ClientSocket clientSocket, ChangeAvatarMessage message)
        {
            var user = _gameManager.FindUserByClientSocket(clientSocket);
            if (user == null) return;

            user.AvatarId = message.avatarId;
            _uiSystem.ChangeLobbyPlayerAvatar(user);
        }

        private void OnChangeStateMessage(ClientSocket clientSocket, ChangeStateMessage message)
        {
            var user = _gameManager.FindUserByClientSocket(clientSocket);
            if (user == null) return;

            user.IsReady = message.isReady;
            _uiSystem.ChangeLobbyPlayerState(user);

            _gameManager.UpdateLobbyState();
            _uiSystem.UpdateLobbyState(_gameManager.State);
        }

        private void OnCountingFinish()
        {
            SceneManager.LoadScene(ScreenCodes.Game);
            SendStartGameMessageToAllUsers();
        }

        private void SendStartGameMessageToAllUsers()
        {
            var avatars = _gameManager.GetAvatarList();

            for (var i = 0; i < _gameManager.Users.Length; i++)
            {
                var user = _gameManager.Users[i];
                var message = new StartGameMessage(i, avatars);
                user?.ClientSocket.SendMessage(message);
            }
        }
    }
}