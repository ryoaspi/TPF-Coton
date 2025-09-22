using Interface.Runtime;
using Object.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Runtime
{
    public class EnemyBig : MonoBehaviour
    {
        #region publics

        public GameObject m_colliderDommage;
        [HideInInspector] public int m_currentCoton;


        #endregion
        
        
        #region Unity Api

        private void Start()
        {
            _enemyStat = GetComponent<EnemyStat>();
            _enemyAI = GetComponent<EnemyAI>();
            _enemyStat.OnCotonLost += (int amount) => LoseCoton(amount, true);
            _animator =  GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _agentSpeed = _agent.velocity;
            
            _baseDamage = _enemyStat.m_damage;
            _baseBlock = _enemyStat.m_block;
            _baseHealth = _enemyStat.m_currentHealth;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                Coton coton = other.GetComponent<Coton>();
                if (coton is not null && !coton.CanBeCollected()) return;
                
                int cotonCollected = collectable.Collect();
                _colliderDisable = other.gameObject;
                _enemyAI.ResetCotonCollection();
                _enemyAI.m_isEating = true;
                _agent.isStopped = true;
                _animator.SetTrigger("Eat");
                                
                // === Soin si PV perdus ===
                int missingHealth = _enemyStat.m_currentHealth < _baseHealth ? _baseHealth - _enemyStat.m_currentHealth : 0;
                
                int healAmount = Mathf.Min(cotonCollected, missingHealth);

                if (healAmount > 0)
                {
                    _enemyStat.Heal(healAmount);
                }
                
                // === le surplus est utilisé pour le buff ===
                int cotonForBuff = cotonCollected - healAmount;
                if (cotonForBuff > 0)
                {
                    m_currentCoton += cotonForBuff;
                    UpdateStats();
                    
                }
            }
        }
        
        #endregion
        
        
        #region Utils

        public void LoseCoton(int amout, bool fromStat = false)
        {
            if (m_currentCoton <= 0) return;
            int actualLoss = Mathf.Min(amout, m_currentCoton);
            m_currentCoton -= actualLoss;
            if (!fromStat)_enemyStat.DropCotonDamage(actualLoss);
            UpdateStats();
            
        }

        public void Fight()
        {
            if(_isAttacking) return;

            var damageReset = m_colliderDommage.GetComponent<IWeaponDamageResettable>();
            if (damageReset != null)
            {
                damageReset.ResetHit();
            }
            
            _isAttacking = true;
            _attackTimer = 0f;
            m_colliderDommage.SetActive(true);
        }

        public void EndAttack()
        {
            m_colliderDommage.SetActive(false);
            _isAttacking = false;
            _attackTimer = 0;
        }

        public void Eat()
        {
            if (_colliderDisable is not null)
            {
                _colliderDisable.SetActive(false);
                _colliderDisable = null;
            }
        }
        
        public void EndEat()
        {
            _agent.velocity = _agentSpeed;
        }

        
        #endregion


        #region Main Methods

        private void UpdateStats()
        {
            int buffLevel = m_currentCoton / _counter;
            
            int newDamage = _baseDamage + buffLevel;
            int newBlock = _baseBlock + buffLevel;
            int newHealth = _baseHealth + buffLevel;
            
            GameObject parent = transform.parent.gameObject;
            float upScale = buffLevel * 0.1f;
            parent.transform.localScale = new Vector3(1 + upScale, 1 + upScale, 1 + upScale);
            _enemyStat.SetStat(newDamage,newBlock,newHealth);
            
        }

        
        #endregion
        
        
        
        #region Private And Protected

        [SerializeField] private int _counter = 5;
        private int _coton;
        private EnemyStat _enemyStat;
        private EnemyAI _enemyAI;

        private int _baseDamage;
        private int _baseBlock;
        private int _baseHealth;
        
        [SerializeField] private LayerMask _layerCoton;
        
        private float _attackTimer;
        private float _attackDuration =1f;
        private bool _isAttacking;
        private Animator _animator;
        private NavMeshAgent _agent;
        private Vector3 _agentSpeed;
        
        private GameObject _colliderDisable;
        
        #endregion
    }
}
