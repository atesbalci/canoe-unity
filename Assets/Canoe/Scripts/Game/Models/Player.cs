namespace Canoe.Game.Models
{
    public class Player
    {
        public delegate void PlayerRow(float time, bool backward);
        public delegate void PlayerActivenessChanged(bool active);
        
        public event PlayerRow OnRow;
        public event PlayerActivenessChanged OnActivenessChanged;
        
        private bool _active;
        
        public int Id { get; }

        public Player(int id)
        {
            Id = id;
            _active = true;
        }

        public bool Active
        {
            get => _active;
            set
            {
                if(_active == value) return;
                _active = value;
                OnActivenessChanged?.Invoke(value);
            }
        }

        public void Row(float timeStamp, bool backward)
        {
            OnRow?.Invoke(timeStamp, backward);
        }
    }
}