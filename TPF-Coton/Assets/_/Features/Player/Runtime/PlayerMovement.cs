using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player.Runtime
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Public
        
        [FormerlySerializedAs("speed")] [SerializeField] public  float m_speed = 5;
        [HideInInspector] public float m_speedSave;
        
        #endregion
        
        #region UnityAPI

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            m_speedSave = m_speed;
            _playerDamage = GetComponent<PlayerDamage>();
            _baseGravity=Physics.gravity;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            // Update _lastDirection based on current input
            if (_moveDirection.sqrMagnitude > 0.01f && _playerDamage.m_isAttacking == false)
            {
                Vector3 camForward = _mainCamera.transform.forward;
                Vector3 camRight = _mainCamera.transform.right;
                camForward.y = 0;
                camRight.y = 0;
                camForward.Normalize();
                camRight.Normalize();
                _lastDirection = (camForward * _moveDirection.y + camRight * _moveDirection.x).normalized;
            }

            if (_isGrounded && _playerDamage.m_isAttacking == false)
            {
                HandleMovement();
            }
            else if (!_isGrounded)
            {
                TeleportOnGround();
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(_lastDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        private void Update()
        {
            GroundCheck();
            if (_isGrounded)
            {
                _rb.linearDamping = groundedDrag;
                Physics.gravity =_baseGravity ;
            }
            else
            {
                _rb.linearDamping = airDrag;
                Physics.gravity= _baseGravity*3;
            }
            
        }

        #endregion
        
        #region Utils

        private void HandleMovement()
        {
            // ... (le reste du code est inchangé)
            
            Vector3 camForward = _mainCamera.transform.forward;
            Vector3 camRight = _mainCamera.transform.right;
            float stickMagnitude = Mathf.Clamp01(_moveDirection.magnitude);
            
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();
            
            _moveDir = camForward * _moveDirection.y + camRight * _moveDirection.x;
            
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDir.normalized, _slopeHit.normal);
            Vector3 desiredVelocity = projectedVelocity * (m_speed * stickMagnitude);
            
            // Ajoute la vélocité désirée directement.
            _rb.linearVelocity = desiredVelocity;
            
            // Applique une force vers le haut sur les pentes escaladables.
            if (_onClimbableSlope)
            {
                _rb.AddForce(Vector3.up * (_climbUpwardForce * Time.fixedDeltaTime), ForceMode.Force);
            }
            
            // Applique une force vers le bas sur les pentes pour éviter de flotter.
            if (_onSlope && _rb.linearVelocity.y > 0)
            { 
                _rb.AddForce(Vector3.down * (_downwardSlopeForce * Time.fixedDeltaTime), ForceMode.Force);
            }
            
            
            }
        

        private void GroundCheck()
        {
            Vector3 origin = transform.position + Vector3.up * 0.1f; 
            _isGrounded = Physics.SphereCast(origin, groundCheckRadius, Vector3.down, out _slopeHit, groundCheckDistance);
            
            if (_isGrounded)
            {
                float slopeAngle = Vector3.Angle(_slopeHit.normal, Vector3.up);
                _onClimbableSlope = slopeAngle > maxSlopeAngle && slopeAngle <= _maxClimbAngle;
                _onSlope = slopeAngle > 0 && slopeAngle <= _maxClimbAngle;
            } 
            else
            {
                _onSlope = false;
                _onClimbableSlope = false;
            }
         }
        
        private void TeleportOnGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                float capsuleHeight = _rb.gameObject.GetComponent<CapsuleCollider>().height;
                transform.position = hit.point + new Vector3(0, capsuleHeight / 2, 0);
            }
        }
        
        #endregion
        
        #region NewInputSystem
        
        private void OnEnable()
        {
            _moveAction.Enable();
            _moveAction.performed += OnMove; 
            _moveAction.canceled += OnMoveCanceled;
        }
        
        private void OnDisable()
        {
            _moveAction.performed -= OnMove;
            _moveAction.canceled -= OnMoveCanceled;
            _moveAction.Disable(); 
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
        
        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _moveDirection = Vector2.zero;
        }
        
        #endregion
        
        #region Private

        private Rigidbody _rb;
        private Vector2 _moveDirection;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private Camera _mainCamera;
        private bool _isGrounded;
        
        [SerializeField] private float rotationSpeed = 10f;
        [Header("Ground Check Settings")]
        [SerializeField] private float groundCheckDistance = 1.5f;
        [SerializeField] private float groundCheckRadius = 0.4f;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private float groundedDrag = 5f;
        [SerializeField] private float airDrag = 0.1f;
        [FormerlySerializedAs("m_downwardSlopeForce")] [SerializeField] private float _downwardSlopeForce = 80f;

        [Header("Climbing Settings")]
        [Tooltip("The max angle (in degrees) that the player can climb.")]
        [HideInInspector] private float _maxClimbAngle = 60f;
        [Tooltip("Upward force to apply when climbing steeper slopes.")]
        [HideInInspector] private float _climbUpwardForce = 50f;
        
        private RaycastHit _slopeHit;
        private bool _onSlope;
        private bool _onClimbableSlope;
        private Vector3 _lastDirection;
        private Vector3 _moveDir;
        private PlayerDamage _playerDamage;
        private Vector3 _baseGravity;

        #endregion
    }
}