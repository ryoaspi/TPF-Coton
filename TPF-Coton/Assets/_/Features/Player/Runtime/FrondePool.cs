using System.Collections.Generic;
using UnityEngine;

namespace Player.Runtime
{
    public class FrondePool : MonoBehaviour
    {
        #region Public
        
        public static FrondePool Instance;
        
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            Instance = this;
            
            for (int i = 0; i < _ammoCount; i++)
            {
                GameObject ammo = Instantiate(_ammoPrefab);
                ammo.transform.SetParent(_ammoParent.transform);
                ammo.SetActive(false);
                _ammoQueue.Enqueue(ammo);
            }
        }

        #endregion
        
        
        #region Utils

        public GameObject GetFromPool()
        {
            GameObject ammo;
            
            if (_ammoQueue.Count > 0)
            {
                ammo = _ammoQueue.Dequeue();
                ammo.SetActive(true);
            }

            else
            {
                ammo = Instantiate(_ammoPrefab);
                ammo.transform.SetParent(_ammoParent.transform);
            }
            
            return ammo;
        }

        public void ReturnToPool(GameObject ammo)
        {
            ammo.SetActive(false);
            ammo.transform.SetParent(_ammoParent.transform);
            _ammoQueue.Enqueue(ammo);
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject _ammoPrefab;
        [SerializeField] private int _ammoCount = 10;
        [SerializeField] private GameObject _ammoParent;
        private Queue<GameObject> _ammoQueue = new ();
        
        #endregion   
    }
}
