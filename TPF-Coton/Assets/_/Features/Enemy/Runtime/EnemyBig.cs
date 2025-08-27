using System;
using Interface.Runtime;
using Object.Runtime;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyBig : MonoBehaviour
    {
        #region publics

        public Collider m_colliderDommage;
        [HideInInspector] public int m_currentCoton;


        #endregion
        
        
        #region Unity Api

        private void Start()
        {
            _enemyStat = GetComponent<EnemyStat>();
            _enemyAI = GetComponent<EnemyAI>();
            _enemyStat.OnCotonLost += (int amount) => LoseCoton(amount, true);
            
            _baseDamage = _enemyStat.m_damage;
            _baseBlock = _enemyStat.m_block;
            _baseHealth = _enemyStat.m_currentHealth;
        }

        private void Update()
        {
            if (_isAttacking)
            {
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= _attackDuration)
                {
                    m_colliderDommage.enabled = false;
                    _isAttacking = false;
                    _attackTimer = 0;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                Coton coton = other.GetComponent<Coton>();
                if (coton is not null && !coton.CanBeCollected()) return;
                
                int cotonCollected = collectable.Collect();
                other.gameObject.SetActive(false);

                _enemyAI.ResetCotonCollection();
                
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
            m_currentCoton -= amout;
            if (m_currentCoton < 0) m_currentCoton = 0;
            if (!fromStat)_enemyStat.DropCotonDamage(amout);
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
            m_colliderDommage.enabled = true;
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
        
        #endregion
    }
}
