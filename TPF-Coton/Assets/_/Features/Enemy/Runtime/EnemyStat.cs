using System;
using Damage.Runtime;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyStat : MonoBehaviour
    {
        #region Public
        
        [HideInInspector] public bool m_isDeath;
        public event Action OnDeath;
        #endregion
        
        
        #region Unity Api

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

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var rapierController = other.gameObject.GetComponentInChildren<WeaponDamage>();
                if (rapierController != null && rapierController.m_isAttacking)
                {
                    _currentHealth -= rapierController.m_damage - _block;
                    Hit();
                    if (_currentHealth <= 0)
                    {
                        gameObject.SetActive(false);
                        m_isDeath = true;
                    }
                        
                }
                else Debug.LogWarning("No WeaponDamage on Player");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var rapierController = other.gameObject.GetComponentInChildren<WeaponDamage>();
                if (rapierController != null && rapierController.m_isAttacking)
                {
                    _currentHealth -= rapierController.m_damage - _block;
                    Hit();
                    if (_currentHealth <= 0)
                    {
                        Death();
                    }
                        
                }
                else Debug.LogWarning("No WeaponDamage on Player");
            }
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
            
            gameObject.SetActive(false);

            OnDeath?.Invoke();
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [Header("Stats")]
        [SerializeField] private int _health = 5;
        [SerializeField] private int _block = 0;
        [SerializeField] private float _hits = 1f;
        [SerializeField] private float _speed = 10f;
        
        private Renderer _renderer;
        private int _blessing;
        private int _currentHealth;
        
        #endregion
    }
}
