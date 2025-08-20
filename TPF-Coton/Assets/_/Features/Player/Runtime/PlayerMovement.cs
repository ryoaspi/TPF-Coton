using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class PlayerMovement : MonoBehaviour
    {
        #region UnityAPI

        private void Awake()
        {
            _playerInput=GetComponent<PlayerInput>();
            _moveAction=_playerInput.actions["Move"];
        }

        private void Start()
        {
            _rb=GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            
            
        }

        private void LateUpdate()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            GroundCheck();
        }
        
        void Update()
        {
            
            if (_isGrounded &&_moveDirection.sqrMagnitude > 0.01f && _mainCamera != null)
            {
                
                Vector3 camForward = _mainCamera.transform.forward;
                Vector3 camRight = _mainCamera.transform.right;
                float stickMagnitude = Mathf.Clamp01(_moveDirection.magnitude);
                
                camForward.y = 0;
                camRight.y = 0;
                camForward.Normalize();
                camRight.Normalize();
                
                _moveDir = camForward * _moveDirection.y + camRight * _moveDirection.x;
                
                Vector3 slopeNormal = _slopeHit.normal;
                Vector3 movement = Vector3.ProjectOnPlane(_moveDir, slopeNormal);
                
                movement.Normalize();
                _lastDirection =  _moveDir.normalized;
                
                _rb.AddForce(movement * (speed * stickMagnitude), ForceMode.VelocityChange);
                
                _rb.linearDamping = groundedDrag;
               
            }  
            else if(!_isGrounded)
            {
                _rb.linearDamping = airDrag;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(_lastDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
        }

        #endregion
        
        
        #region Utils
        
        
        private void GroundCheck()
        {
            
            Vector3 origin = transform.position + Vector3.up * 0.1f; 
            
            if (Physics.SphereCast(origin, groundCheckRadius, Vector3.down, out RaycastHit slopeHit, groundCheckDistance))
            {
                _slopeHit = slopeHit;
                float slopeAngle = Vector3.Angle(_slopeHit.normal, Vector3.up);
                
                 if (slopeAngle <= maxSlopeAngle)
                 {
                        _isGrounded = true;
                        return;
                 } 
            }
            _isGrounded = false;
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
        
        
        [SerializeField] float speed = 5;
        [SerializeField] private float rotationSpeed = 10f;
        
        [Header("Ground Check Settings")]
        [SerializeField] private float groundCheckDistance = 1.5f;
        [SerializeField] private float groundCheckRadius = 0.4f;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private float groundedDrag = 5f;
        [SerializeField] private float airDrag = 0.1f;
        private RaycastHit _slopeHit;
        private Vector3 _slopeGravity;
        private Vector3 _slopeRight;
        
        private Vector3 _lastDirection;
        private Vector3 _moveDir;

        #endregion
    }
}

