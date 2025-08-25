using Interface.Runtime;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyBig : MonoBehaviour
    {
        #region publics

        

        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            _enemyStat = GetComponent<EnemyStat>();
            _enemyAI = GetComponent<EnemyAI>();
            _enemyStat.OnCotonLost += LoseCoton;
            
            _baseDamage = _enemyStat.m_damage;
            _baseBlock = _enemyStat.m_block;
            _baseHealth = _enemyStat.m_currentHealth;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                _coton = collectable.Collect();
                _currentCoton += _coton;
                other.gameObject.SetActive(false);
                
                UpdateStats();
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
        
        #endregion
    }
}
