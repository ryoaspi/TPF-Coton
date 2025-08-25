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
            _bullet = AmmoPool.Instance.GetFromPool();
            _bullet.transform.position = _firePoint.position;
            _bullet.transform.rotation = _firePoint.rotation;

            
            if (_enemyStat is not null && _bullet.TryGetComponent<EnemyAmmo>(out var enemyAmmo))
            {
                enemyAmmo.m_damage = _enemyStat.m_damage;
            }       
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private Transform _firePoint;
        
        private EnemyStat _enemyStat;
        private GameObject _bullet;
        private EnemyAmmo _enemyAmmo;

        #endregion
    }
}
