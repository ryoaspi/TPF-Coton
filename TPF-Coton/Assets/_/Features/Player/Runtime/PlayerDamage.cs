using System;
using UnityEngine;
using UnityEngine.InputSystem;



namespace Player.Runtime
{
    public class PlayerDamage : MonoBehaviour
    {
        
        #region UnityAPi

        private void Awake()
        {
            _playerInput=GetComponent<PlayerInput>();
            _shield=GetComponent<Shield>();
        }

        void Start()
        {
            
            _sword =transform.GetChild(2).GetChild(0).gameObject;
            _swordAnchor = transform.GetChild(2).gameObject;
            
            _baseSwordRotationConverted = _baseSwordPositionObject.transform.localRotation;
            _targetPositionConverted = _targetPositionObject.transform.localRotation;
            
            
            _sword.SetActive(false);
            _isOnCooldown = false;
            _isAttacking = false;
            
        }
        
        void Update ()
        {
            if (_isOnCooldown)
            {
                _cooldown += Time.deltaTime;
                if (_cooldown >= _hitCooldown)
                {
                   _cooldown = 0;
                   _isOnCooldown = false;
                }
                
            }

            if (_isAttacking)
            {
                
              
                _currentSwordRotationConverted=Quaternion.RotateTowards(_swordAnchor.transform.localRotation, _targetPositionConverted, _anglepPerSecond*Time.deltaTime);
                _swordAnchor.transform.localRotation=_currentSwordRotationConverted;
                Debug.Log(_baseSwordRotationConverted+" base");
                Debug.Log(_targetPositionConverted+" target");
                if (Quaternion.Angle(_currentSwordRotationConverted, _targetPositionConverted) < 0.1f)
                {
                    _isAttacking = false;
                    _sword.SetActive(false);
                    
                    _swordAnchor.transform.localRotation = _baseSwordRotationConverted;
                    
                }
            }
            
        }
        
        #endregion
        
        #region Input System
        private void OnEnable()
        {
            var actions = _playerInput.actions;
            actions["Attack"].performed += OnAttack;
            
        }
        
        private void OnDisable()
        {
            var actions = _playerInput.actions;
            actions["Attack"].performed -= OnAttack;
        }
        
        private void OnAttack(InputAction.CallbackContext context)
        {
            if (!_isOnCooldown && _shield.m_isShielding==false)
            {
               
                _sword.SetActive(true);
                _isOnCooldown = true;
                _isAttacking = true;
            }
        }

        #endregion
        
        #region Private

        private PlayerInput _playerInput;
        
        [SerializeField]private GameObject _sword;
        [SerializeField]private GameObject _swordAnchor;
        [SerializeField]private GameObject _baseSwordPositionObject;
        [SerializeField]private GameObject _targetPositionObject;
        private bool _isOnCooldown;
        private float _cooldown;
        private Vector3 _baseSwordPosition;
        private Vector3 _targetPosition;
        private Quaternion _baseSwordRotationConverted;
        private Quaternion _currentSwordRotationConverted;
        private Quaternion _targetPositionConverted;
        [SerializeField] private float _hitCooldown =1f;
        [SerializeField] private float _anglepPerSecond;
        private bool _isAttacking;
        private Shield _shield;

        #endregion
    }
}
