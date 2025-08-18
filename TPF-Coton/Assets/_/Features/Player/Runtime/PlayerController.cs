using System;
using Damage.Runtime;
using TheFundation.Runtime;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
            Cursor.visible = false;
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

        private void FixedUpdate()
        {

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

            if (other.gameObject.layer == LayerMask.NameToLayer("WeaponEnemy"))
            {
                if (_isBlocking == false)
                {
                    Hit();
                    var damage = other.gameObject.GetComponent<WeaponEnemyDamage>().m_damage;
                    if (_renderer.material.color != Color.red)
                        _currentHealth -= damage;
                }
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
            // if (context.started)
            // {
            //     _isAttackingCharged = true;
            //     _timeCharged += Time.deltaTime;
            //     if (_timeCharged >= 1f)
            //     {
            //         _rendererSword.material.color = Color.yellow;
            //         if ( context.canceled)
            //         {
            //             var charge = _weapon.GetComponent<WeaponDamage>().m_damage;
            //             charge = (int) (charge * 2f);
            //             _isAttackingCharged = false;
            //             _timeCharged = 0f;
            //             _rendererSword.material.color = Color.gray;
            //         }
            //
            //     }
            //     
            //     else
            //     {
            //         _isAttackingCharged = false;
            //         _timeCharged = 0f;
            //         _rendererSword.material.color = Color.gray;
            //     }
            // }
            
        }

		public void OnInventory(InputAction.CallbackContext context)
		{
			//Implémentation de la logique pour l'inventaire
			_isInventoryOpen = !_isInventoryOpen;
			_inventoryPanel.SetActive(_isInventoryOpen);
		}

        public void OnBlocking(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _shield.enabled = true;
                _isBlocking = true;
            }

            else
            {
                _shield.enabled = false;
                _isBlocking = false;
            }
        }

        // public void OnMenu(InputAction.CallbackContext context)
        // {
        //     EventSystem.current.SetSelectedGameObject(null);
        //     EventSystem.current.SetSelectedGameObject(_assetLoadButton);
        //     _enemyLoadSelect.gameObject.SetActive(false);
        //     _isLoadScene = true;
        //     _playerInput.SwitchCurrentActionMap("UI");
        //     _loadSceneCanvas.gameObject.SetActive(_isLoadScene);
        // }
        //
        // public void OnCloseMenu(InputAction.CallbackContext context)
        // {
        //     _isLoadScene = false;
        //     _playerInput.SwitchCurrentActionMap("Player");
        //     _loadSceneCanvas.gameObject.SetActive(_isLoadScene);
        // }
        //
        // public void OnNavigate(InputAction.CallbackContext context)
        // {
        //     _buttonSelected=EventSystem.current.currentSelectedGameObject;
        //     if (_buttonSelected == _assetLoadButton)
        //     {
        //         _assetLoadSelect.gameObject.SetActive(true);
        //         _enemyLoadSelect.gameObject.SetActive(false);
        //     }
        //     else if (_buttonSelected == _enemyLoad)
        //     {
        //         _assetLoadSelect.gameObject.SetActive(false);
        //         _enemyLoadSelect.gameObject.SetActive(true);
        //     }
        // }

        
        

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

        // public void GetRefCanvas(Canvas canvas,GameObject button1, GameObject button2,GameObject image1, GameObject image2 )
        // {
        //     _loadSceneCanvas = canvas;
        //     _assetLoadButton = button1;
        //     _enemyLoad=button2;
        //     _assetLoadSelect=image1;
        //     _enemyLoadSelect=image2;
        //     
        // }
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
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        private Vector2 _moveInput;
        private Rigidbody _rb;
        
        //Interaction
        [Header("Interaction")]
        [SerializeField] private float _interactionRange =2f;
        
        //Combat
        [Header("Fight")]
        [SerializeField] private int _attackPower = 5;
        [SerializeField] private GameObject _weapon;
        [SerializeField] private float _hits = 1f;
        [SerializeField] private Collider _shield;
        private bool _isBlocking = false;
        // private bool _isAttackingCharged = false;
        // private float _timeCharged = 0f;
        // [SerializeField] private Renderer _rendererSword;
        
        //Inventory
        [Header("Inventory")]
		[SerializeField] private GameObject _inventoryPanel;
        private bool _isInventoryOpen = false;

        //UI
        // [SerializeField] private Canvas  _loadSceneCanvas;
        // private bool _isLoadScene =true;
        // [SerializeField] private GameObject _assetLoadButton;
        // [SerializeField] private GameObject _enemyLoad;
        // private GameObject _buttonSelected;
        // [SerializeField] private GameObject _assetLoadSelect;
        // [SerializeField] private GameObject _enemyLoadSelect;
        
        //Stat
        [Header("Stat")]
        [SerializeField] private string _playerName = "Flonflon";
        [SerializeField] private int _level = 1;
        [SerializeField] private int _experience = 0;
        [SerializeField] private int _gold = 0;
        [SerializeField] private int _MaxHealth = 100;
        
        [Header("Other")]
        private int _currentHealth;
        [SerializeField] private Renderer _renderer;
        private Inventory _inventory = new ();
        
        #endregion
    }
}
