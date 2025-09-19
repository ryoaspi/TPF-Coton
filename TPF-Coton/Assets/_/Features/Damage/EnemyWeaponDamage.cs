using Enemy.Runtime;
using Interface.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Damage.Runtime
{
    public class EnemyWeaponDamage : MonoBehaviour, IWeaponDamageResettable
    {
        #region Unity Api

        private void OnEnable()
        {
            _enemyStat = GetComponentInParent<EnemyStat>();
            _enemyAI = GetComponentInParent<EnemyAI>();
            _weaponEnemyDamage = GetComponent<WeaponEnemyDamage>();
            
            //Détection manuelle des objets dédjà dans le collider au moment de l'activation
            DetectInitialOverlaps();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Melee)
            {
                if (_weaponEnemyDamage == null || !_weaponEnemyDamage.m_isAttacking) return;
            }
            
            HandleCollision(other);
        }

        #endregion
        
        
        #region Utils

        public void ResetHit()
        {
            _hasExploded = false;
            _hasDamagedPlayer = false;
        }
        
        #endregion
        
        
        #region Main Methods
        
        private void HandleCollision(Collider other)
        {
            if (other == null || _enemyStat == null) return;

            int otherLayer = other.gameObject.layer;
            
            // Gestion des dégâts sur un autre ennemi (pour le type Puffed)
            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Puffed &&
                otherLayer == LayerMask.NameToLayer("Enemy") &&
                !_hasDamagedPlayer)
            {
                
                if (other.transform.IsChildOf(transform)) return;

                EnemyStat otherEnemy = other.GetComponentInParent<EnemyStat>();

                if (otherEnemy == null || otherEnemy == _enemyStat)
                {
                    
                    return;
                }

                _damage = _enemyStat.m_currentHealth;
                otherEnemy.DoDamage(_damage, transform);
                _hasDamagedPlayer = true;
            }

            // Gestion des dégâts sur le joueur
            if (otherLayer == LayerMask.NameToLayer("Player"))
            {
                PlayerStats playerStat = other.GetComponentInParent<PlayerStats>();
                if (playerStat != null)
                {
                    _damage = _enemyStat.m_damage;

                    playerStat.DoDamage(_damage);
                }
            }
        }
        
        private void DetectInitialOverlaps()
        {
            Collider[] overlaps = Physics.OverlapBox(
                transform.position,
                GetComponent<Collider>().bounds.extents,
                transform.rotation,
                LayerMask.GetMask("Player", "Enemy")
            );

            foreach (var collider in overlaps)
            {
                HandleCollision(collider);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        private EnemyStat _enemyStat;
        private int _damage;
        private EnemyAI _enemyAI;
        private bool _hasExploded;
        private bool _hasDamagedPlayer;
        private WeaponEnemyDamage _weaponEnemyDamage;

        #endregion
    }
}
