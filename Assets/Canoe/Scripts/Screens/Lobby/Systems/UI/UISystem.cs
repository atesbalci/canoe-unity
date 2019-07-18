using Framework.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Canoe.Screens.Lobby.Systems.UI
{
    public class UISystem : BaseScreenSystem<LobbyScreenResources>
    {
        private RawImage _barcodeImage;

        protected override void Awake()
        {
            base.Awake();

            _barcodeImage = GameObject.Find("BarcodeImage").GetComponent<RawImage>();
        }

        public void ChangeBarcodeImage(Texture2D texture)
        {
            _barcodeImage.texture = texture;
        }
    }
}