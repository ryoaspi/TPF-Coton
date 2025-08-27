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
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject[] _objectToActivate;
        
        #endregion
    }
}
