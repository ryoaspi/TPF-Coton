using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Player.Runtime
{
    public class Shield : MonoBehaviour
    {
        
        #region public

        [HideInInspector]public bool m_isShielding;
        
        #endregion
        
        #region UnityAPI
        void Awake()
        {
            _playerInput=GetComponent<PlayerInput>();
            _playerMovement =  GetComponent<PlayerMovement>();
            _playerDamage= GetComponent<PlayerDamage>();
            _fronde = GetComponent<Fronde>();
        }

        private void Start()
        {
            _shield.SetActive(false);
            m_isShielding = false;

        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("BulletEnemy"))
            {
                other.gameObject.SetActive(false);
            }
            
        }
        #endregion
        
        
        
        #region New Input System
        
        private void OnEnable()
        {
            var actions = _playerInput.actions;
            actions["Block"].started += OnBlock;
            actions["Block"].canceled += OnBlockCancelled;
            
        }
        
        private void OnDisable()
        {
            var actions = _playerInput.actions;
            actions["Block"].started -= OnBlock;
            actions["Block"].started -= OnBlockCancelled;
        }

        private void OnBlock(InputAction.CallbackContext context)
        {
            if (!_playerDamage.m_isAttacking && !_fronde.m_coolDownCharge)
            {
                m_isShielding = true;
                _shield.SetActive(true);
                _playerMovement.m_speed = 0;
            }
                
            
        }

        private void OnBlockCancelled(InputAction.CallbackContext context)
        {
            
                _playerMovement.m_speed = _playerMovement.m_speedSave;
                m_isShielding = false;
                _shield.SetActive(false);
                
        }
        #endregion
        
        #region Private
        
        private PlayerInput _playerInput;
        [SerializeField] private GameObject _shield;
        private PlayerMovement _playerMovement;
        private PlayerDamage _playerDamage;
        private Fronde _fronde;
        #endregion
    }
}
