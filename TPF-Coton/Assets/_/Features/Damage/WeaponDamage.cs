using System;
using Enemy.Runtime;
using UnityEngine;

namespace Damage.Runtime
{
    public class WeaponDamage : MonoBehaviour
    {
        #region Unity Api

        private void Awake()
        {
            _enemyStat = GetComponent<EnemyStat>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _enemyStat.DoDamage(_damage);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _damage = _enemyStat.m_damage;
            }
        }
        
        #endregion
        

        #region Private And Protected

        private EnemyStat _enemyStat;
        private int _damage = 1;

        #endregion
    }
}
