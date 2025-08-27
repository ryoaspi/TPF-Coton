using Enemy.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Damage.Runtime
{
    public class EnemyWeaponDamage : MonoBehaviour
    {
        #region Unity Api

        private void OnEnable()
        {
            _enemyStat = GetComponentInParent<EnemyStat>();
            _enemyAI = GetComponentInParent<EnemyAI>();
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerStats playerStat = other.GetComponentInParent<PlayerStats>();
                if (playerStat != null && _enemyStat != null)
                {
                    _damage = _enemyStat.m_damage;
                    playerStat.DoDamage(_damage);
                }

                return;
            }

            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Puffed && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (_enemyStat != null)
                {
                    _damage = _enemyStat.m_currentHealth;
                    _enemyStat.DoDamage(_damage,null);
                }
                
            }
        }

        #endregion
        
        
        #region Private And Protected
        
        private EnemyStat _enemyStat;
        private int _damage;
        private EnemyAI _enemyAI;

        #endregion
    }
}
