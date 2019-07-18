using UnityEngine;

namespace Canoe.Helpers.Waves
{
    public class WaveRotationAdapter : MonoBehaviour
    {
        [SerializeField] private WaveBehaviour _waveBehaviour;
        [SerializeField] private bool _syncHeight;

        private void Start()
        {
            if (_waveBehaviour == null)
            {
                _waveBehaviour = FindObjectOfType<WaveBehaviour>();
            }
        }

        private void Update()
        {
            var trans = transform;
            var pos = trans.position;
            var forward = trans.rotation * Vector3.forward * 0.5f;
            var p1 = pos - forward;
            var p2 = pos + forward;
            var h1 = _waveBehaviour.GetHeight(p1);
            var h2 = _waveBehaviour.GetHeight(p2);
            p1.y = h1;
            p2.y = h2;
            trans.forward = p2 - p1;
            
            if (_syncHeight)
            {
                pos.y = Mathf.Max(h1, h2);
                trans.position = pos;
            }
        }
    }
}