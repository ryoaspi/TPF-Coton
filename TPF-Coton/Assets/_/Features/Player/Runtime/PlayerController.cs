using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TheFundation.Runtime;

namespace Player.Runtime
{    
    public class PlayerController : FBehaviour
    {
        #region Public
		
        public Inventory m_inventory = new Inventory();
	        
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rb = GetComponent<Rigidbody>();
            _currentHealth = _MaxHealth;
			//_currentAttackTime = _AttackTime;
            
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
			if (_attackState = true) 
			{ 
				_currentAttackTime -= Time.deltaTime;
			}
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
			Attack();
        }

		public void OnInventory(InputAction.CallbackContext context)
		{
			//Implémentation de la logique pour l'inventaire
			_isInventoryOpen = !_isInventoryOpen;
			_inventoryPanel.SetActive(_isInventoryOpen);
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
            
            string inventoryJson = JsonUtility.ToJson(_inventory);
            SetFact("Inventory", inventoryJson, factPersistence);
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

            if (TryGetFact("Inventory", out string inventoryJson))
            {
                _inventory = JsonUtility.FromJson<Inventory>(inventoryJson);
            }
        }
        
        #endregion

		
		#region Main Methods

		private void Attack()
		{
			_attackState = true;
			_attackCollider.gameObject.SetActive(true);

			if (_currentAttackTime <= 0) 
			{
				_attackState = false;
				_attackCollider.gameObject.SetActive(false);
				_currentAttackTime = _attackTime;
			}
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
		[SerializeField] private Collider _attackCollider;
		[SerializeField] private float _attackTime = 1f;
		private float _currentAttackTime =0;
		private bool _attackState = false;
        
        //Inventory
		[SerializeField] private GameObject _inventoryPanel;
        private bool _isInventoryOpen = false;

        
        //Stat
        [Header("Stat")]
        [SerializeField] private string _playerName = "Flonflon";
        [SerializeField] private int _level = 1;
        [SerializeField] private int _experience = 0;
        [SerializeField] private int _gold = 0;
        
        [SerializeField] private int _MaxHealth = 100;
        private int _currentHealth;
        
        private Inventory _inventory = new ();
        
        #endregion
    }
}
