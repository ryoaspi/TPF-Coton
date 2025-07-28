using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TheFundation.Runtime;

namespace Player.Runtime
{    
    public class PlayerController : FBehaviour
    {
        #region Public

        
        
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rb = GetComponent<Rigidbody>();
            _currentHealth = _MaxHealth;
            
            LoadPlayerFacts();

            if (_currentHealth <= 0)
            {
                _currentHealth = _MaxHealth/2;
            }
        }

        private void OnEnable()
        {
            var actions = _playerInput.actions;

            actions["Move"].performed += OnMove;
            actions["Move"].canceled += OnMove;

            actions["Interact"].performed += OnInteract;
            actions["Attack"].performed += OnAttack;
        }

        private void OnDisable()
        {
            var actions = _playerInput.actions;

            actions["Move"].performed -= OnMove;
            actions["Move"].canceled -= OnMove;

            actions["Interact"].performed -= OnInteract;
            actions["Attack"].performed -= OnAttack;
        }

        private void Update()
        {
            Vector3 move = new Vector3(_moveInput.x,0,_moveInput.y) * (_moveSpeed * Time.deltaTime);
            _rb.MovePosition(transform.position + move);
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();    
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            //Implémentation de la logique d'intéraction.
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            //Implémentation de la logique de combat.
        }

        #endregion
        
        
        #region Utils

        public void SavePlayerFacts()
        {
            var factPersistence = FactDictionary.FactPersistence.Persistent;
            
            SetFact("Name",_playerName, factPersistence);
            SetFact("Level",_level, factPersistence);
            SetFact("EXP",_experience, factPersistence);
            SetFact("MaxHealth",_MaxHealth, factPersistence);
            SetFact("CurrentHealth",_currentHealth, factPersistence);
            SetFact("Gold",_gold, factPersistence);
        }

        public void LoadPlayerFacts()
        {
            var factPersistence = FactDictionary.FactPersistence.Persistent;
            
            TryGetFact("Name", out _playerName);
            TryGetFact("Level", out _level);
            TryGetFact("EXP", out _experience);
            TryGetFact("MaxHealth", out _MaxHealth);
            TryGetFact("CurrentHealth", out _currentHealth);
            TryGetFact("Gold", out _gold);
        }
        
        #endregion
        
        
        #region Private And Protected
        
        // Input
        private PlayerInput _playerInput;
        
        //Movement
        [SerializeField] private float _moveSpeed = 5f;
        private Vector2 _moveInput;
        private Rigidbody _rb;
        
        //Interaction
        [SerializeField] private float _interactionRange =2f;
        
        //Combat
        [SerializeField] private int _attackPower = 5;
        
        //Stat
        [Header("Stat")]
        [SerializeField] private string _playerName = "Flonflon";
        [SerializeField] private int _level = 1;
        [SerializeField] private int _experience = 0;
        [SerializeField] private int _gold = 0;
        
        [SerializeField] private int _MaxHealth = 100;
        private int _currentHealth;
        
        #endregion
    }
}
