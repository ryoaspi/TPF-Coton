using Damage.Runtime;
using TheFundation.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{    
    public class PlayerController : FBehaviour
    {
        #region Public
		
        public Inventory m_inventory = new ();
	        
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
            
            _renderer = GetComponentInChildren<Renderer>();
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
            
                if (_moveInput.sqrMagnitude < 0.01f) return;

                
                CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
                if (brain == null || brain.ActiveVirtualCamera == null) return;

                Transform camTransform = brain.transform;

                
                Vector3 camForward = camTransform.forward;
                Vector3 camRight = camTransform.right;

                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                
                Vector3 moveDirection = (camRight * _moveInput.x + camForward * _moveInput.y).normalized;

                
                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
                }

                
                Vector3 move = moveDirection * (_moveSpeed * Time.deltaTime);
                _rb.MovePosition(transform.position + move);
                
                // If Hits
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
            
            if (other.gameObject.layer == LayerMask.NameToLayer("BulletEnemy"))
            {
                Hit();
                var damage = other.gameObject.GetComponent<EnemyAmmo>().m_damage;
                if (_renderer.material.color != Color.red)
                    _currentHealth -= damage;
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
            _weapon.GetComponent<WeaponDamage>().IsAttacking();
        }

		public void OnInventory(InputAction.CallbackContext context)
		{
			//Implémentation de la logique pour l'inventaire
			_isInventoryOpen = !_isInventoryOpen;
			_inventoryPanel.SetActive(_isInventoryOpen);
		}

        public void OnBlocking(InputAction.CallbackContext context)
        {
            if (context.performed) _shield.enabled = true;
            
            else _shield.enabled = false;
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

        [ContextMenu("Hits")]
        private void Hit()
        {
            _renderer.material.color = Color.red;
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
        [SerializeField] private GameObject _weapon;
        [SerializeField] private float _hits = 1f;
        [SerializeField] private Collider _shield;
        
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
        [SerializeField] private Renderer _renderer;
        private Inventory _inventory = new ();
        
        #endregion
    }
}
