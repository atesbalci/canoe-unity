using UnityEngine;

namespace Canoe.Helpers.EngineUtilities
{
    public class Billboard : MonoBehaviour
    {
        private Transform _camera;
        
        private void Start()
        {
            var cam = Camera.main;
            if (cam != null) _camera = cam.transform;
        }

        private void Update()
        {
            if(!_camera) return;
            transform.rotation = _camera.rotation;
        }
    }
}