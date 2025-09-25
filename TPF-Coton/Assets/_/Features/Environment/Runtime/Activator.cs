using UnityEngine;

namespace Environment.Runtime
{
    public class Activator : MonoBehaviour
    {
        #region Unity Api

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                foreach (GameObject objectToActivate in _objectToActivate)
                {
                    objectToActivate.SetActive(true);
                }

                foreach (GameObject objectToDeactivate in _objectToDeactivate)
                {
                    objectToDeactivate.SetActive(false);
                }
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject[] _objectToActivate;
        [SerializeField] private GameObject[] _objectToDeactivate;
        
        #endregion
    }
}
