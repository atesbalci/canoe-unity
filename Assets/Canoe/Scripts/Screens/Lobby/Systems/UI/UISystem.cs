using System.Collections.Generic;
using System.Threading;
using Canoe.Components;
using Canoe.Models;
using Framework.Scripts;
using Framework.Scripts.Managers.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Canoe.Screens.Lobby.Systems.UI
{
    public class UISystem : BaseScreenSystem<LobbyScreenResources>
    {
        private PoolManager _poolManager;

        private RawImage _barcodeImage;

        private RectTransform[] _panelTransforms;
        private Dictionary<UserModel, LobbyPlayer> lobbyPlayersByUser;

        protected override void Awake()
        {
            base.Awake();

            _poolManager = FindObjectOfType<PoolManager>();

            _barcodeImage = GameObject.Find("BarcodeImage").GetComponent<RawImage>();
            var lobbyTransform = GameObject.Find("Lobby").transform;
            _panelTransforms = new RectTransform[2]
            {
                lobbyTransform.GetChild(0).GetComponent<RectTransform>(),
                lobbyTransform.GetChild(1).GetComponent<RectTransform>()
            };
            lobbyPlayersByUser = new Dictionary<UserModel, LobbyPlayer>();
        }

        public void ChangeBarcodeImage(Texture2D texture)
        {
            _barcodeImage.texture = texture;
        }

        public void AddLobbyPlayerIntoPanel(int position, UserModel user)
        {
            var panelIndex = position % 2;
            var panelTransform = _panelTransforms[panelIndex];

            var lobbyPlayer = _poolManager.GetGameObject<LobbyPlayer>(Resources.lobbyPlayerPrefab, panelTransform);
            lobbyPlayer.Prepare(user);

            var sprite = Resources.avatarSprites[user.AvatarId];
            lobbyPlayer.ChangeAvatarSprite(sprite);

            lobbyPlayersByUser.Add(user, lobbyPlayer);
        }

        public void RemoveLobbyPlayerFromPanel(int position, UserModel user)
        {
            var panelIndex = position % 2;
            var panelTransform = _panelTransforms[panelIndex];

            foreach (Transform lobbyPlayerTransform in panelTransform)
            {
                var lobbyPlayer = lobbyPlayerTransform.GetComponent<LobbyPlayer>();
                if (lobbyPlayer.User != user) continue;

                _poolManager.ReleaseGameObject(lobbyPlayer);
                lobbyPlayersByUser.Remove(user);
                break;
            }
        }

        public void ChangeLobbyPlayerAvatar(UserModel user)
        {
            if (lobbyPlayersByUser.TryGetValue(user, out var lobbyPlayer) == false) return;

            lobbyPlayer.ChangeAvatarSprite(Resources.avatarSprites[user.AvatarId]);
        }

        public void ChangeLobbyPlayerState(UserModel user)
        {
            if (lobbyPlayersByUser.TryGetValue(user, out var lobbyPlayer) == false) return;

            lobbyPlayer.SetState(user.IsReady);
        }
    }
}