using Canoe.Game.Models;
using UnityEngine;
using Zenject;

namespace Canoe.Game.Behaviours
{
    public class BoatBehaviour : MonoBehaviour
    {
        private const float RowStrength = 10f;
        private const float CurrentStrength = 2f;
        private const float TurnRate = 20f;
        private const float BackwardsRowStrength = 0.5f;

        private Boat _boat;
        private Rigidbody _body;
        
        [Inject]
        public void Initialize(Boat boat)
        {
            _boat = boat;
            _body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _boat.Tick(Time.time);

            var trans = transform;
            var velocity = CurrentStrength * Vector3.forward;
            trans.Rotate(0f, TurnRate * (_boat.RightRowAmount - _boat.LeftRowAmount) * Time.deltaTime, 0f);
            var forward = trans.rotation * Vector3.forward;
            var rowTotal = _boat.LeftRowAmount + _boat.RightRowAmount;
            velocity += rowTotal * (rowTotal > 0f ? RowStrength : BackwardsRowStrength) * forward;
            _body.velocity = velocity;
        }
    }
}