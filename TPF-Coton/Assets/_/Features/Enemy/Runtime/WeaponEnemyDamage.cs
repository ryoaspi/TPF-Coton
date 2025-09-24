using UnityEngine;

namespace Enemy.Runtime
{
    public class WeaponEnemyDamage : MonoBehaviour
    {
        #region Public
        
        [HideInInspector] public bool m_isAttacking;
        
        #endregion
        
        
        #region Api Unity

        private void Awake()
        {
            _enemyAI = GetComponentInParent<EnemyAI>();
            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Melee)
            {
                _collider = GetComponent<Collider>();
                _collider.enabled = false;
                _collider.isTrigger = true;
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_isAttacking) return;
        }

        #endregion

        
        #region Utils

        public void ActivateDamage()
        {
            m_isAttacking = true;
            _collider.enabled = true;
        }

        public void DeactivateDamage()
        {
            m_isAttacking = false;
            _collider.enabled = false;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private Collider _collider;
        private EnemyAI _enemyAI;

        #endregion
    }
}