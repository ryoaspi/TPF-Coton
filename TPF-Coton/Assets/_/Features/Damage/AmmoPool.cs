using System.Collections.Generic;
using UnityEngine;

namespace Damage.Runtime
{
    [DefaultExecutionOrder(-100)]
    public class AmmoPool : MonoBehaviour
    {
        #region Public
        
        public static AmmoPool Instance;
        
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            Instance = this;
            for (int i = 0; i < _ammoCount; i++)
            {
                GameObject ammo = Instantiate(_ammoPrefab);
                ammo.SetActive(false);
                _ammoQueue.Enqueue(ammo);
            }
        }

        #endregion
        
        
        #region Utils

        public GameObject GetFromPool()
        {
            if (_ammoQueue.Count > 0)
            {
                GameObject ammo = _ammoQueue.Dequeue();
                ammo.SetActive(true);
                return ammo;
            }
            
            GameObject newAmmo = Instantiate(_ammoPrefab);
            return newAmmo;
        }

        public void ReturnToPool(GameObject ammo)
        {
            ammo.SetActive(false);
            _ammoQueue.Enqueue(ammo);
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject _ammoPrefab;
        [SerializeField] private int _ammoCount = 10;
        private Queue<GameObject> _ammoQueue = new ();
        
        #endregion   
    }
}
