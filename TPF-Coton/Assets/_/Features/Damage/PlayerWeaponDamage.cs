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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyStat enemyStat = other.GetComponentInParent<EnemyStat>();
                if (enemyStat != null && _playerStats != null)
                {
                    _damage = _playerStats.m_publicDamage;
                    enemyStat.DoDamage(_damage, _playerStats.transform);
                }
            }
        }

        #endregion
        
        
        #region Private And Protected
        
        private PlayerStats _playerStats;
        private int _damage;
        
        #endregion
    }
}
