using System;

namespace Canoe.Game.Models
{
    public class Player
    {
        public event Action<float, bool> OnRow;

        public void Row(float timeStamp, bool backward)
        {
            OnRow?.Invoke(timeStamp, backward);
        }
    }
}