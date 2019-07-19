using System;
using Canoe.Game.Models;
using Canoe.Game.Views.Data;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Canoe.Game.Views
{
    public class PlayerView : MonoBehaviour
    {
        private const float StateTimeoutDuration = 0.25f;
        private const float RevertSpeed = 200f;
        private const float RowSpeed = 1000f;
        private const float SpriteJumpScale = 1.5f;
        private const float SpriteJumpDuration = 0.05f;
        private const float SpriteJumpRevertDuration = 0.25f;
        private const float ArrowFadeDuration = 1f;

        private static readonly Color[] Colors =
            {Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan};
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _spritePivot;
        [SerializeField] private Transform _row;
        [SerializeField] private SpriteRenderer _arrow;
        [SerializeField] private Transform _arrowPivot;

        private Player _player;
        private State _state;
        private float _currentRowAngle;
        private PlayerViewData _playerViewData;
        private Tween _spriteJumpTween;

        [Inject]
        public void Initialize(PlayerViewData playerViewData)
        {
            _playerViewData = playerViewData;
            _arrow.color = new Color(1f, 1f, 1f, 0f);
        }

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

            _spriteRenderer.sprite = _playerViewData.GetPlayerSprite(_player.Id);
            _player.OnRow += OnRow;
            gameObject.SetActive(true);
            _state.Time = -StateTimeoutDuration;
        }

        private void Update()
        {
            const float fullAngle = 360f;
            
            if (Time.time - _state.Time > StateTimeoutDuration)
            {
                var target = (_currentRowAngle > (fullAngle * 0.5f)) ? (fullAngle + 0.001f) : 0f;
                _currentRowAngle = Mathf.MoveTowards(_currentRowAngle, target, RevertSpeed * Time.deltaTime);
            }
            else
            {
                _currentRowAngle += RowSpeed * (_state.Backward ? -1f : 1f) * Time.deltaTime;
            }

            #region Snap angle back in to between 0 and 360

            while (_currentRowAngle < -0.001f)
            {
                _currentRowAngle += fullAngle;
            }
            _currentRowAngle -= Mathf.Floor(_currentRowAngle / fullAngle) * fullAngle;

            #endregion
            
            _row.localEulerAngles = new Vector3(_currentRowAngle, 0f, 0f);
        }

        private void OnRow(float time, bool backward)
        {
            _state.Time = time;
            _state.Backward = backward;
            _spriteJumpTween.Kill();
            _spritePivot.localScale = Vector3.one;
            _arrowPivot.localScale = SpriteJumpScale * Vector3.one;
            _arrow.color = new Color(1f, 1f, 1f, 0f);
            _arrow.flipY = backward;
            _spriteJumpTween = DOTween.Sequence()
                .Append(_spritePivot.DOScale(Vector3.one * SpriteJumpScale, SpriteJumpDuration))
                .Join(_arrow.DOFade(1f, SpriteJumpDuration))
                .Join(_arrowPivot.DOScale(Vector3.one, SpriteJumpDuration))
                .Append(_spritePivot.DOScale(Vector3.one, SpriteJumpRevertDuration))
                .Append(_arrow.DOFade(0f, ArrowFadeDuration));
        }

        private void OnDisable()
        {
            _spriteJumpTween.Kill(true);
        }

        private struct State
        {
            public float Time;
            public bool Backward;
        }
    }
}