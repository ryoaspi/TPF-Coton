using System;
using Enemy.Runtime;
using UnityEngine;

namespace Environment.Runtime
{
    public class Activator : MonoBehaviour
    {
        #region Unity Api

        private void Awake()
        {
            
        }

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
                    var enemyStat = objectToDeactivate.GetComponentInChildren<EnemyStat>();
                    if (enemyStat is not null)
                    {
                        enemyStat.Kill();
                    }
                    else Debug.LogWarning($"EnemyStat component not found on {objectToDeactivate.name}");
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
