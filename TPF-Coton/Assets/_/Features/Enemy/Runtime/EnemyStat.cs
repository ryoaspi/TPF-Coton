using System;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyStat : MonoBehaviour
    {
        #region Public
        
        [HideInInspector] public bool m_isDeath;
        [HideInInspector] public int m_damage;
        public event Action OnDeath;
        #endregion
        
        
        #region Unity Api

        private void Start()
        {
            m_damage = _Damage;
        }

        private void OnEnable()
        {
            _currentHealth = _health;
            _renderer = GetComponent<Renderer>();
        }
        
        private void Update()
        {
            if (_renderer.material.color == Color.red)
            {
                _hits -= Time.deltaTime;
                if (_hits <= 0)
                {
                    _renderer.material.color = Color.gray;
                    _hits = 1f;
                }
            }
            
        }
        
        #endregion
        
        
        #region Utils

        public int DoDamage(int damage)
        {
            _currentHealth -= damage;
            Hit();
            if (_currentHealth <= 0)
            {
                Death();
            }
            
            return _currentHealth;
            
        }
        
        #endregion
        
        
        #region Main Method

        private void Hit()
        {
            _renderer.material.color = Color.red;
        }
        
        [ContextMenu("Death")]
        private void Death()
        {
            // if (m_isDeath) return;
            
            m_isDeath = true;
            
            OnDeath?.Invoke();
            
            gameObject.SetActive(false);
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [Header("Stats")]
        [SerializeField] private int _health = 5;
        [SerializeField] private int _block = 0;
        [SerializeField] private int _Damage = 1;
        [SerializeField] private float _hits = 1f;
        
        private Renderer _renderer;
        private int _blessing;
        private int _currentHealth;
        
        #endregion
    }
}
