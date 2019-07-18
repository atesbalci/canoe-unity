namespace Canoe.Game.Models
{
    public class Player
    {
        public delegate void PlayerRow(float time, bool backward);
        
        public event PlayerRow OnRow;
        
        public int Id { get; }

        public Player(int id)
        {
            Id = id;
        }

        public void Row(float timeStamp, bool backward)
        {
            OnRow?.Invoke(timeStamp, backward);
        }
    }
}