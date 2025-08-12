using System;
using Player.Runtime;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyStat : MonoBehaviour
    {
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
                var rapierController = other.gameObject.GetComponentInChildren<RapierControlle>();
                if (rapierController != null)
                {
                    _currentHealth -= rapierController.m_damage - _block;
                    Hit();
                    if (_currentHealth <= 0)
                        Destroy(gameObject);
                }
                else Debug.LogWarning("No RapierController on Player");
            }
        }

        #endregion
        
        
        #region Main Method

        private void Hit()
        {
            _renderer.material.color = Color.red;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private int _health = 5;
        [SerializeField] private int _block = 0;
        
        private Renderer _renderer;
        private int _blessing;
        private int _currentHealth;
        private float _hits = 1f;


        #endregion
    }
}
