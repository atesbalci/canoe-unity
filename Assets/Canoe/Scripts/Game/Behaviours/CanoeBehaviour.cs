using UnityEngine;
using Zenject;

namespace Canoe.Game.Behaviours
{
    public class CanoeBehaviour : MonoBehaviour
    {
        private const float RowStrength = 10f;
        private const float CurrentStrength = 2f;
        private const float TurnRate = 20f;
        private const float BackwardsRowStrength = 0.5f;

        private Models.Canoe _canoe;
        private Rigidbody _body;
        
        [Inject]
        public void Initialize(Models.Canoe canoe)
        {
            _canoe = canoe;
            _body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _canoe.Tick(Time.time);

            var trans = transform;
            var velocity = CurrentStrength * Vector3.forward;
            trans.Rotate(0f, TurnRate * (_canoe.RightRowAmount - _canoe.LeftRowAmount) * Time.deltaTime, 0f);
            var forward = trans.rotation * Vector3.forward;
            var rowTotal = _canoe.LeftRowAmount + _canoe.RightRowAmount;
            velocity += rowTotal * (rowTotal > 0f ? RowStrength : BackwardsRowStrength) * forward;
            _body.velocity = velocity;
        }
    }
}