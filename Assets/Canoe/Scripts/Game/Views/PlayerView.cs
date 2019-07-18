using Canoe.Game.Models;
using UnityEngine;

namespace Canoe.Game.Views
{
    public class PlayerView : MonoBehaviour
    {
        private const float StateTimeoutDuration = 0.25f;
        private const float RevertSpeed = 200f;
        private const float RowSpeed = 1000f;

        private static readonly Color[] Colors =
            {Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan};
        
        [SerializeField] private Renderer _model;
        [SerializeField] private Transform _row;

        private Player _player;
        private State _state;
        [SerializeField] private float _currentAngle;

        public void Bind(Player player)
        {
            if (_player != null)
            {
                _player.OnRow -= OnRow;
            }
            
            _player = player;
            if (_player == null)
            {
                gameObject.SetActive(false);
                return;
            }

            _model.material.color = Colors[_player.Id];
            _player.OnRow += OnRow;
            gameObject.SetActive(true);
            _state.Time = -StateTimeoutDuration;
        }

        private void Update()
        {
            const float fullAngle = 360f;
            
            if (Time.time - _state.Time > StateTimeoutDuration)
            {
                var target = (_currentAngle > (fullAngle * 0.5f)) ? (fullAngle + 0.001f) : 0f;
                _currentAngle = Mathf.MoveTowards(_currentAngle, target, RevertSpeed * Time.deltaTime);
            }
            else
            {
                _currentAngle += RowSpeed * (_state.Backward ? -1f : 1f) * Time.deltaTime;
            }

            #region Snap angle back in to between 0 and 360

            while (_currentAngle < -0.001f)
            {
                _currentAngle += fullAngle;
            }
            _currentAngle -= Mathf.Floor(_currentAngle / fullAngle) * fullAngle;

            #endregion
            
            _row.localEulerAngles = new Vector3(_currentAngle, 0f, 0f);
        }

        private void OnRow(float time, bool backward)
        {
            _state.Time = time;
            _state.Backward = backward;
        }

        private struct State
        {
            public float Time;
            public bool Backward;
        }
    }
}