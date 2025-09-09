using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Player.Runtime
{
    public class PlayerStats : MonoBehaviour
    {

        #region public

        [Header("Damage")]
        [HideInInspector] public int m_publicDamage;
        [HideInInspector] public int m_privateDamage;
        [HideInInspector] public int m_frondeDamage;
        
        
        [Header("HP")] [HideInInspector] public int m_publicHP;
        [HideInInspector] public int m_privateHP;
        [HideInInspector]public int m_currentHealth;
        [FormerlySerializedAs("_currentState")] [HideInInspector]public int m_currentState;
        
        #endregion


        #region UnityApi

        void Awake()
        {


            _isChronoOn = false;
            _shield = GetComponent<Shield>();
            _playerBuff = GetComponent<PlayerBuff>();
            _playerDropCoton=GetComponent<PlayerDropCoton>();
            _fronde = GetComponent<Fronde>();
            _playerMovement=GetComponent<PlayerMovement>();
            _mediumSpeed = _playerMovement.m_speed;
             m_currentHealth = _maxHealth/2;
             m_publicHP = _maxHealth;
             m_publicDamage = _mediumStatDamage;
        }

        void Start()
        {
            
            FrondeDamageUpdate();
            UpdateTextHealth();
            DamageUpdate();
            UpdateMaxHealth();
            healthSlider.maxValue = m_privateHP;
            healthSlider.value = m_currentHealth;
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
                _playerBuff.LoseCoton(damage);
                _playerBuff.CheckSize();
                Hit();
                IsDead();
            }
        }
        public void FrondeSelfDamage (int damage)
        {
                _playerBuff.LoseCoton(damage);
                _playerBuff.CheckSize();
                IsDead();
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
            m_privateDamage = _mediumStatDamage;
        }
        private void FrondeDamageUpdate()
        {
            m_frondeDamage = _mediumfrondeDamage;
        }
        private void UpdateMaxHealth()
        {
            m_privateHP = _maxHealth;
        }

        public void UpdateTextHealth()
        {
           healthSlider.value = m_currentHealth;
           _numberOfCoton.SetText(m_currentHealth.ToString());
        }
        
        
        
        public void LittleState()
        {
            m_frondeDamage = _littleFrondeDamage;
            m_publicDamage = _littleStatDamage;
            _playerMovement.m_speedSave=_littleSpeed;
            _playerMovement.m_speed = _littleSpeed;
            _playerMovement.m_speedSave = _littleSpeed;
            transform.localScale= new Vector3(0.5f-_littleScale,0.5f-_littleScale,0.5f-_littleScale);
            m_currentState = 1;
            _playerMovement.groundCheckDistance = 0.27f;
            _playerMovement.groundCheckRadius = 0.15f;
        }
        
        public void MediumState()
        {
            m_frondeDamage = _mediumfrondeDamage;
            m_publicDamage = _mediumStatDamage;
            _playerMovement.m_speedSave=_mediumSpeed;
            _playerMovement.m_speed = _mediumSpeed;
            _playerMovement.m_speedSave = _mediumSpeed;
            transform.localScale= new Vector3(0.5f,0.5f,0.5f);
            m_currentState = 2;
            _playerMovement.groundCheckDistance = 0.38f;
            _playerMovement.groundCheckRadius = 0.2f;
        }
        public void BigState()
        {
            m_frondeDamage = _bigFrondeDamage;
            m_publicDamage = _bigStatDamage;
            _playerMovement.m_speedSave=_bigSpeed;
            _playerMovement.m_speed = _bigSpeed;
            _playerMovement.m_speedSave = _bigSpeed;
            transform.localScale= new Vector3(0.5f+_bigScale,0.5f+_bigScale,0.5f+_bigScale);  
            m_currentState = 3;
            _playerMovement.groundCheckDistance = 0.8f;
            _playerMovement.groundCheckRadius = 0.4f;
        }
        #endregion
        
        
        #region private
        
        [Header("StatDamage")]
        [SerializeField]private int _littleStatDamage;
        [FormerlySerializedAs("_statDamage")] [SerializeField] private int _mediumStatDamage =1;
        [SerializeField]private int _bigStatDamage;
        [SerializeField]private int _littleFrondeDamage;
        [FormerlySerializedAs("_frondeDamage")][SerializeField]private int _mediumfrondeDamage=1;
        [SerializeField]private int _bigFrondeDamage;
        [SerializeField] private float _littleSpeed; 
        private float _mediumSpeed;
        [SerializeField] private float _bigSpeed;
        [SerializeField] private float _littleScale;
        [SerializeField]private float _bigScale;
        
        
        [SerializeField] private int _maxHealth=100;
        
        [Header ("UI")]
        [SerializeField]public Slider healthSlider;
        [SerializeField]private TMP_Text _numberOfCoton;
        
        
        [Header("Hit")]
        [SerializeField] private Renderer _renderer;
        [FormerlySerializedAs("_colorHitTime")] [SerializeField] private float _invinsibilityTime = 1f;
        private float _chrono;
        private bool _isChronoOn;
        private Shield _shield;
        private PlayerBuff _playerBuff;
        private PlayerDropCoton _playerDropCoton;
        private PlayerMovement _playerMovement;
        private Fronde _fronde;
        
        #endregion
    }
}
