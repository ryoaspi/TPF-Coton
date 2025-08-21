using Enemy.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Damage.Runtime
{
    public class WeaponDamage : MonoBehaviour
    {
        #region Unity Api

        private void Awake()
        {
            _enemyStat = GetComponent<EnemyStat>();
            // _playerStat = GetComponent<PlayerStat>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // _damage = _playerStat.m_publicDamage;
                _enemyStat.DoDamage(_damage);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _damage = _enemyStat.m_damage;
                // _playerStat.DoDamage(_damage);
            }
        }
        
        #endregion
        

        #region Private And Protected

        private EnemyStat _enemyStat;
        // private PlayerStat _playerStat;
        private int _damage = 1;

        #endregion
    }
}
