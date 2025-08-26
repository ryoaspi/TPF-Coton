using Interface.Runtime;
using Object.Runtime;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyBig : MonoBehaviour
    {
        #region publics

        

        #endregion
        
        
        #region Unity Api

        private void Start()
        {
            _enemyStat = GetComponent<EnemyStat>();
            _enemyAI = GetComponent<EnemyAI>();
            _enemyStat.OnCotonLost += LoseCoton;
            
            _baseDamage = _enemyStat.m_damage;
            _baseBlock = _enemyStat.m_block;
            _baseHealth = _enemyStat.m_currentHealth;
        }

        private void Update()
        {
            
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
                    _currentCoton += cotonForBuff;
                    UpdateStats();
                    
                }
                
            }
        }

        #endregion
        
        
        #region Utils

        public void LoseCoton(int amout)
        {
            _currentCoton -= amout;
            if (_currentCoton < 0) _currentCoton = 0;
            UpdateStats();
        }
        
        #endregion


        #region Main Methods

        private void UpdateStats()
        {
            int buffLevel = _currentCoton / _counter;
            
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
        private int _currentCoton;
        private EnemyStat _enemyStat;
        private EnemyAI _enemyAI;

        private int _baseDamage;
        private int _baseBlock;
        private int _baseHealth;
        
        [SerializeField] private LayerMask _layerCoton;
        
        #endregion
    }
}
