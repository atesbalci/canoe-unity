using Canoe.Models;
using Framework.Scripts.Managers.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace Canoe.Components
{
    public class LobbyPlayer : MonoBehaviour, IPoolable
    {
        public UserModel User { get; set; }
        private Image _avatarImage;

        private void Awake()
        {
            _avatarImage = transform.Find("AvatarImage").GetComponent<Image>();
        }

        public void OnRelease()
        {
            User = null;
            _avatarImage.sprite = null;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Prepare(UserModel user)
        {
            User = user;
            
            var t = transform;
            t.localPosition = Vector3.zero;
            t.localScale = Vector3.one;
        }

        public void ChangeAvatarSprite(Sprite sprite)
        {
            _avatarImage.sprite = sprite;
        }
    }
}