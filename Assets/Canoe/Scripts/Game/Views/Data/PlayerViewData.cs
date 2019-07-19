using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Canoe.Game.Views.Data
{
    public class PlayerViewData
    {
        private readonly ReadOnlyCollection<Sprite> _playerSprites;

        public PlayerViewData(IList<Sprite> playerSprites)
        {
            _playerSprites = new ReadOnlyCollection<Sprite>(playerSprites);
        }

        public Sprite GetPlayerSprite(int id)
        {
            return _playerSprites[id];
        }
    }
}