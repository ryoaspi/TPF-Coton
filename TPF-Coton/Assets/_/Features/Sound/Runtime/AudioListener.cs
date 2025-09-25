using UnityEngine;

namespace Sound.Runtime
{
    public class AudioListener : MonoBehaviour
    {
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            gameObject.transform.forward = _camera.transform.forward;
        }

        private Camera _camera;
    }
}
