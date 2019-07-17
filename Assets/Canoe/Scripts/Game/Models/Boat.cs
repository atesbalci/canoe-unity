using System;
using System.Collections.Generic;

namespace Canoe.Game.Models
{
    public class Boat
    {
        public event Action<CanoeSide, int, Player> OnPlayerAdded;
        
        public IList<Player> LeftPlayers { get; }
        public IList<Player> RightPlayers { get; }
        public float SampleDuration { get; }

        public float LeftRowAmount { get; private set; }
        public float RightRowAmount { get; private set; }

        private readonly LinkedList<RowEntry> _rowEntries;

        public Boat(float sampleDuration)
        {
            LeftPlayers = new List<Player>();
            RightPlayers = new List<Player>();
            _rowEntries = new LinkedList<RowEntry>();
            SampleDuration = sampleDuration;
        }

        public Player AddPlayer()
        {
            var retVal = new Player();
            var side = LeftPlayers.Count <= RightPlayers.Count ? CanoeSide.Left : CanoeSide.Right;
            (side == CanoeSide.Left ? LeftPlayers : RightPlayers).Add(retVal);
            retVal.OnRow += (timeStamp, backward) =>
            {
                var listCount = side == CanoeSide.Left ? LeftPlayers.Count : RightPlayers.Count;
                _rowEntries.AddLast(new RowEntry
                {
                    Player = retVal,
                    Side = side,
                    Timestamp = timeStamp,
                    Value = (backward ? -1f : 1f) / listCount
                });
            };

            OnPlayerAdded?.Invoke(side, (side == CanoeSide.Left ? LeftPlayers.Count : RightPlayers.Count) - 1, retVal);
            
            return retVal;
        }

        public void Tick(float timeStamp)
        {
            LeftRowAmount = 0;
            RightRowAmount = 0;
            for (var cur = _rowEntries.First; cur != null;)
            {
                var next = cur.Next;
                if (timeStamp - cur.Value.Timestamp > SampleDuration)
                {
                    _rowEntries.Remove(cur);
                }
                else
                {
                    if (cur.Value.Side == CanoeSide.Left)
                    {
                        LeftRowAmount += cur.Value.Value;
                    }
                    else
                    {
                        RightRowAmount += cur.Value.Value;
                    }
                }
                cur = next;
            }
        }

        private struct RowEntry
        {
            public Player Player;
            public CanoeSide Side;
            public float Timestamp;
            public float Value;
        }
    }

    public enum CanoeSide
    {
        None,
        Left,
        Right
    }
}