using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Runtime
{
    public class PlayerStats : MonoBehaviour
    {

        #region public

        [Header("Damage")] [HideInInspector] public int m_publicDamage;
        [HideInInspector] public int m_privateDamage;

        [Header("HP")] [HideInInspector] public int m_publicHP;
        [HideInInspector] public int m_privateHP;
        [HideInInspector]public int m_currentHealth;
        #endregion


        #region UnityApi

        void Awake()
        {


            _isChronoOn = false;
            _shield = GetComponent<Shield>();
            _playerBuff = GetComponent<PlayerBuff>();
            _playerDropCoton=GetComponent<PlayerDropCoton>();

        }

        void Start()
        {
            m_currentHealth = _maxHealth;
            m_publicHP = _maxHealth;
            m_publicDamage = _statDamage;
           
            UpdateTextHealth();
            DamageUpdate();
            UpdateMaxHealth();
        }


        private void Update()
        {

            if (_isChronoOn)
            {

                _chrono += Time.deltaTime;
                if (_chrono >= _invinsibilityTime)
                {
                    _renderer.material.color = Color.gray;
                    _chrono = 0;
                    _isChronoOn = false;
                }

            }

        }

        #endregion


        #region Utils

        [ContextMenu("DoDamage")]
        private void DoDamage() => DoDamage(1);
        [ContextMenu("Hits")]
        private void Hit()
        {
            _renderer.material.color = Color.red;
            _isChronoOn = true;
        }

        public void DoDamage(int damage)
        {
            if (_shield.m_isShielding == false && _isChronoOn==false)
            {
                _playerDropCoton.DropCotonDamage(damage);
                m_currentHealth -= damage;
                _playerBuff.LoseCoton(damage);
                UpdateTextHealth();
                Hit();
                IsDead();
            }
        }

        public void IsDead()
        {
            if (m_currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        private void DamageUpdate()
        {
            m_privateDamage = _statDamage;
        }

        private void UpdateMaxHealth()
        {
            m_privateHP = _maxHealth;
        }

        public void UpdateTextHealth()
        {
            _textCurrentHealth.text = $"{m_currentHealth}/{m_publicHP}";
        }
        #endregion
        
        
        #region private
        
        [Header("Stat")]
        [SerializeField] private string _playerName = "Flonflon";
        [SerializeField] private int _statDamage =1;
        [SerializeField] private int _maxHealth=100;
        
        [Header ("UI")]
        [SerializeField] private TextMeshProUGUI _textCurrentHealth;
        
        
        
        [Header("Hit")]
        [SerializeField] private Renderer _renderer;
        [FormerlySerializedAs("_colorHitTime")] [SerializeField] private float _invinsibilityTime = 1f;
        private float _chrono;
        private bool _isChronoOn;
        private Shield _shield;
        private PlayerBuff _playerBuff;
        private PlayerDropCoton _playerDropCoton;
        #endregion
    }
}
