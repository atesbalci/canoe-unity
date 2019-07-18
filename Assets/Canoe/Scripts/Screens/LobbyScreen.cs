using System;
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

        private UISystem _uiSystem;

        protected override void Awake()
        {
            base.Awake();

            _wssManager = FindObjectOfType<WebSocketServerManager>();
            _wssManager.OnPlayerConnect = OnPlayerConnect;
            _wssManager.OnPlayerDisconnect = OnPlayerDisconnect;

            _uiSystem = FindObjectOfType<UISystem>();
        }
        
        private void OnPlayerConnect()
        {
            Debug.Log("user connected");
        }
        
        private void OnPlayerDisconnect()
        {
            Debug.Log("user disconnected");
        }

        private void Start()
        {
            var texture = Barcode.Generate("Ertan", BarcodeFormat.QR_CODE, 512, 512);
            _uiSystem.ChangeBarcodeImage(texture);
            
            _wssManager.Start();
        }

        private void OnDestroy()
        {
            _wssManager.Stop();
        }
        
        
    }
}