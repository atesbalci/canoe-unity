using System.Collections;
using System.Collections.Generic;
using Canoe.Components;
using Canoe.Managers.Game;
using Canoe.Models;
using Framework.Scripts;
using Framework.Scripts.Managers.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Canoe.Managers.Game.GameManager.LobbyState;

namespace Canoe.Screens.Lobby.Systems.UI
{
    public class UISystem : BaseScreenSystem<LobbyScreenResources>
    {
        public UnityAction OnCountingFinish;

        private PoolManager _poolManager;

        private RawImage _barcodeImage;
        private TextMeshProUGUI _counterText;
        private TextMeshProUGUI _infoText;

        private RectTransform[] _panelTransforms;
        private Dictionary<UserModel, LobbyPlayer> lobbyPlayersByUser;

        protected override void Awake()
        {
            base.Awake();

            _poolManager = FindObjectOfType<PoolManager>();

            _barcodeImage = GameObject.Find("BarcodeImage").GetComponent<RawImage>();
            _counterText = GameObject.Find("CounterText").GetComponent<TextMeshProUGUI>();
            _infoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();

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

        public void UpdateLobbyState(GameManager.LobbyState state)
        {
            StopCoroutine(nameof(StartCounting));

            _barcodeImage.enabled = state != Ready;
            _counterText.enabled = state == Ready;

            switch (state)
            {
                case NotReady:
                    break;
                case NotEnoughPlayers:
                    break;
                case Ready:
                    StopCoroutine(nameof(StartCounting));
                    StartCoroutine(StartCounting());
                    break;
            }
        }

        private IEnumerator StartCounting()
        {
            _counterText.SetText("5");
            for (int i = 5; i >= 0; i--)
            {
                _counterText.SetText(i.ToString());
                yield return new WaitForSeconds(1);
            }

            yield return null;

            OnCountingFinish?.Invoke();
        }
    }
}