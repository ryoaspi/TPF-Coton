using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerDamage : MonoBehaviour
    {
        #region Public

        [HideInInspector] public bool m_isAttacking;

        // Event pour notifier le début de l'attaque
        public static event System.Action OnAttackStart;

        #endregion 

        #region UnityAPI

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _shield = GetComponent<Shield>();
            _fronde = GetComponent<Fronde>();
            _animator = GetComponent<Animator>();
            _animatorController = _animator.runtimeAnimatorController;
        }

        void Start()
        {
            _baseSwordRotationConverted = _baseSwordPositionObject.transform.localRotation;
            _targetPositionConverted = _targetPositionObject.transform.localRotation;

            m_isAttacking = false;
            _isOnCooldown = false;
            _swordCollider.enabled = false;
            // Récupère la durée du clip "NeedleAttack"
            foreach (var clip in _animatorController.animationClips)
            {
                if (clip.name == "NeedleAttack") // le nom exact du clip
                {
                    _timeofAnim = clip.length/2;
                }
            }
        }

        void Update()
        {
            // Gestion de l'attaque en cours
            if (m_isAttacking)
            {
                _attackTimer += Time.deltaTime;
                if (_attackTimer >= _timeofAnim)
                {
                    EndAttack();
                }
            }

            // Gestion du cooldown
            if (_isOnCooldown)
            {
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer >= _hitCooldown)
                {
                    _isOnCooldown = false;
                    _cooldownTimer = 0f;
                }
            }
        }

        #endregion

        #region Input System

        private void OnEnable()
        {
            var actions = _playerInput.actions;
            actions["Attack"].started += OnAttack;
        }

        private void OnDisable()
        {
            var actions = _playerInput.actions;
            actions["Attack"].started -= OnAttack;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (!_isOnCooldown && !_shield.m_isShielding && !_fronde.m_coolDownCharge)
            {
                m_isAttacking = true;
                _attackTimer = 0f;
                _swordCollider.enabled = true;
                _isOnCooldown = true;
                _cooldownTimer = 0f;

                // Notifie Damage pour reset la liste d'ennemis
                OnAttackStart?.Invoke();
            }
        }

        #endregion

        #region Public Methods

        // Appelé automatiquement quand la durée du clip est écoulée
        public void EndAttack()
        {
            m_isAttacking = false;
            _swordCollider.enabled = false;
        }

        #endregion

        #region Private

        private PlayerInput _playerInput;

        [SerializeField] private GameObject _sword;
        [SerializeField] private GameObject _swordAnchor;
        [SerializeField] private GameObject _baseSwordPositionObject;
        [SerializeField] private GameObject _targetPositionObject;

        private bool _isOnCooldown;

        private float _attackTimer;     // Timer pour la durée de l'attaque
        private float _cooldownTimer;   // Timer pour le cooldown global

        private Vector3 _baseSwordPosition;
        private Vector3 _targetPosition;
        private Quaternion _baseSwordRotationConverted;
        private Quaternion _currentSwordRotationConverted;
        private Quaternion _targetPositionConverted;

        [SerializeField] private float _hitCooldown = 1f; // délai entre attaques
        [SerializeField] private float _anglepPerSecond;

        private Shield _shield;
        private Fronde _fronde;
        private Animator _animator;
        private RuntimeAnimatorController _animatorController;
        private float _timeofAnim;
        [SerializeField]private Collider _swordCollider;

        #endregion
    }
}
