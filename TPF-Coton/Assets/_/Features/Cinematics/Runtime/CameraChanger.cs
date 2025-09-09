using UnityEngine;
using Unity.Cinemachine;

namespace Cinematics.Runtime
{
    public class CameraChanger : MonoBehaviour
    {
        public void StartCinematic(int i)
        {
            _freeLook.SetActive(false);
            _virtualCamera[i].SetActive(true);
        }

        public void StopCinematic(int i)
        {
            _virtualCamera[i].SetActive(false);
            _freeLook.SetActive(true);
        }
        #region Private
        
        [SerializeField]private GameObject _freeLook;
        [SerializeField]private GameObject[] _virtualCamera;
        
        #endregion

        
    }
}
