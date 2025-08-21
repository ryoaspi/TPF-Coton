using System;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyShoot : MonoBehaviour
    {
        #region Unity Api

        private void Awake()
        {
            _enemyStat = GetComponentInParent<EnemyStat>();
        }
        
        #endregion
        
        #region Utils

        public void Shooting()
        {
            Debug.Log("Je tire");
            _bullet = AmmoPool.Instance.GetFromPool();
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            var enemyAmmo = _bullet.GetComponent<EnemyAmmo>();
            if (enemyAmmo != null)
            {
                enemyAmmo.m_damage = _enemyStat.m_damage;
            }       
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private Transform _firePoint;
        
        private EnemyStat _enemyStat;
        private GameObject _bullet;

        #endregion
    }
}
