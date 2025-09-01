using System.Collections.Generic;
using Craft.Runtime;
using Enemy.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Damage.Runtime
{
    public class PlayerWeaponDamage : MonoBehaviour
    {
        #region Unity Api

        private void Awake()
        {
            _playerStats = GetComponentInParent<PlayerStats>();
        }
        
        private void OnEnable()
        {
            _enemiesHitThisSwing.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Glouton"))
            {
                EnemyStat enemyStat = other.GetComponentInParent<EnemyStat>();
                if (enemyStat != null && _playerStats != null && !_enemiesHitThisSwing.Contains(enemyStat))
                {
                    _damage = _playerStats.m_publicDamage;
                    enemyStat.DoDamage(_damage, _playerStats.transform);
                    _enemiesHitThisSwing.Add(enemyStat);
                }
                return;
            }

        }

        #endregion
        
        
        #region Utils

        public void ResetHitEnemies()
        {
            _enemiesHitThisSwing.Clear();
        }
        
        #endregion
        
        
        #region Private And Protected
        
        private PlayerStats _playerStats;
        private int _damage;
        
        private HashSet<EnemyStat> _enemiesHitThisSwing = new HashSet<EnemyStat>();
        
        #endregion
    }
}
