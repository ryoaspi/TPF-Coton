using System.Collections.Generic;
using Craft.Runtime;
using Enemy.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Damage.Runtime
{
    public class PlayerWeaponDamage : MonoBehaviour
    {
        #region Unity API

        private void Awake()
        {
            _playerStats = FindFirstObjectByType<PlayerStats>();
            _playerDamage = FindFirstObjectByType<PlayerDamage>();
        }

        private void OnEnable()
        {
            _enemiesHitThisSwing.Clear();

            // --- AJOUT --- s'abonne à l'event pour reset chaque attaque
            PlayerDamage.OnAttackStart += ResetHitEnemies;
        }

        private void OnDisable()
        {
            // --- AJOUT --- désabonnement
            PlayerDamage.OnAttackStart -= ResetHitEnemies;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Glouton"))
            {
                EnemyStat enemyStat = other.GetComponentInParent<EnemyStat>();
                if (enemyStat != null && _playerStats != null && !_enemiesHitThisSwing.Contains(enemyStat))
                {
                    if (_playerDamage.m_isAttacking)
                    {
                        _damage = _playerStats.m_publicDamage;
                        enemyStat.DoDamage(_damage, _playerStats.transform);
                        _enemiesHitThisSwing.Add(enemyStat);
                    }
                    else
                    {
                        _damage = _playerStats.m_frondeDamage;
                        enemyStat.DoDamage(_damage, _playerStats.transform);
                        _enemiesHitThisSwing.Add(enemyStat);
                    }
                }
            }
        }

        #endregion

        #region Utils

        public void ResetHitEnemies()
        {
            _enemiesHitThisSwing.Clear();
        }

        #endregion

        #region Private

        private PlayerStats _playerStats;
        private int _damage;
        private PlayerDamage _playerDamage;
        private HashSet<EnemyStat> _enemiesHitThisSwing = new HashSet<EnemyStat>();

        #endregion
    }
}
