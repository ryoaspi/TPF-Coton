using Enemy.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Damage.Runtime
{
    public class EnemyWeaponDamage : MonoBehaviour
    {
        #region Unity Api

        private void Awake()
        {
            _enemyStat = GetComponentInParent<EnemyStat>();
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
            }
        }

        #endregion
        
        
        #region Private And Protected
        
        private EnemyStat _enemyStat;
        private int _damage;
        
        #endregion
    }
}
