using System;
using Unity.Mathematics;

namespace Canoe.Game.Models.Data
{
    public class GameState
    {
        private const float TimeLimit = 5f;
        private const float PointsPerUnit = 0.1f;

        public event Action OnGameOver;
        
        private bool _isSent;
        private double _scoreRaw;

        public long Score => (long) _scoreRaw;

        public float ElapsedTime { get; private set; }
        public float RemainingTime => TimeLimit - ElapsedTime;
        
        public TimeSpan ElapedTimeSpan => TimeSpan.FromSeconds(ElapsedTime);
        public TimeSpan RemainingTimeSpan => TimeSpan.FromSeconds(RemainingTime);

        public bool TimeIsUp => ElapsedTime > TimeLimit - 0.0001f;

        public void Tick(float deltaTime, float distance)
        {
            ElapsedTime = math.min(TimeLimit, ElapsedTime + deltaTime);
            if (!TimeIsUp)
            {
                _scoreRaw = math.max(_scoreRaw, distance * PointsPerUnit);
            }

            if (!_isSent && TimeIsUp)
            {
                _isSent = true;
                OnGameOver?.Invoke();
            }
        }
    }
}