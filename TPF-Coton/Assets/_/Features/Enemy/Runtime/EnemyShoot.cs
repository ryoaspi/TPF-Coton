using System;
using Damage.Runtime;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyShoot : MonoBehaviour
    {
        #region Unity Api

        private void Start()
        {
            _enemyAI = GetComponentInParent<EnemyAI>();
        }

        private void Update()
        {
            _nextFire += Time.deltaTime;
            _playerDetected = _enemyAI.m_playerDetected;
            Shooting();
        }

        #endregion
        
        #region Main Method

        private void Shooting()
        {
            if (!_playerDetected) return;
            
            if (_nextFire >= _fireRate)
            {
                GameObject bullet = AmmoPool.Instance.GetFromPool();
                bullet.transform.position = _firePoint.position;
                bullet.transform.rotation = _firePoint.rotation;
                
                _nextFire = 0;
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _fireRate = 2;
        [SerializeField] private Transform _firePoint;
        private float _nextFire;
        private bool _playerDetected;
        private EnemyAI _enemyAI;


        #endregion
    }
}
