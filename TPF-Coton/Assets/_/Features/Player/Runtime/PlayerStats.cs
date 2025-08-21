using System;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerStats : MonoBehaviour
    {
        
        #region public

        [HideInInspector]public int m_publicDamage; 
        
        #endregion
        
        
        #region UnityApi
        void Awake()
        {

            _currentHealth = _MaxHealth;
            _isChronoOn = false;
            _shield=GetComponent<Shield>();
            
        }

        void Start()
        {
            
            DamageUpdate();
            
        }
        
        
        private void Update()
        {

            if (_isChronoOn)
            {
                
                _chrono+=Time.deltaTime;
                if (_chrono >= _colorHitTime)
                {
                    _renderer.material.color = Color.gray;
                    _chrono = 0;
                    _isChronoOn = false;
                }
                
            }
            
        }

        #endregion
        
        
        #region Utils

        [ContextMenu("Hits")]
        private void Hit()
        {
            _renderer.material.color = Color.red;
            _isChronoOn = true;
        }
        
        public void DoDamage(int damage)
        {
            if (_shield.m_isShielding == false)
            {
                _currentHealth -= damage;
                Hit();
                IsDead();
            }
            
            
            
        }

        public void IsDead()
        {
            if (_currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        private void DamageUpdate()
        {

            m_publicDamage = _statDamage;
        }
        
        #endregion
        #region private
        
        [Header("Stat")]
        [SerializeField] private string _playerName = "Flonflon";
        [SerializeField] private int _level = 1;
        [SerializeField] private int _experience = 0;
        [SerializeField] private int _gold = 0;
        [SerializeField] private int _MaxHealth = 100;
        [SerializeField] private int _statDamage =1;
        private float _currentHealth;
        
        [Header("Hit")]
        [SerializeField] private Renderer _renderer;
        [SerializeField] private float _colorHitTime = 1f;
        [SerializeField]private float _chrono;
        private bool _isChronoOn;
        private Shield _shield;
        
        #endregion
    }
}
