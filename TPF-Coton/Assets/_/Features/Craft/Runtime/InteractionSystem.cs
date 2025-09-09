using Interface.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Craft.Runtime
{
    public class InteractionSystem : MonoBehaviour
    {
        #region Api Unity

        private void Awake()
        {
            _camera = Camera.main;
            _uiManager = FindFirstObjectByType<UIManager.Runtime.UIManager>();
            if (_camera == null) _camera = Camera.main;
            _countObject = FindObjectOfType<CraftObject>();
            
        }

        private void OnEnable()
        {
            _playerInput.actions["Interact"].started += OnInteract;
            _playerInput.onActionTriggered += OnActionTriggered;
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void Update()
        {
            float offsetX = 0f;
            float offsetY = 100f;
            
            Vector3 screenPoint = new Vector3(Screen.width / 2 + offsetX, Screen.height / 2 + offsetY, 0f);
            
            Ray ray = new Ray(_playerTransform.position, _playerTransform.forward.normalized);
            Debug.DrawRay(ray.origin, ray.direction * _interactionDetected, Color.red);
            
            if (Physics.Raycast(ray, out RaycastHit hit, _interactionDetected, _layerMask, QueryTriggerInteraction.Collide))
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                float distanceToHit = Vector3.Distance(_playerTransform.position, hit.point);

                if (Physics.Raycast(_playerTransform.position, (hit.point - _playerTransform.position).normalized,
                        out RaycastHit obstacleHit, distanceToHit, _obstacleMask))
                {
                    ResetInteraction();
                    return;
                }
                
                if (interactable is not null && distanceToHit <= _maxInteractionDistance)
                {
                    _lastCollider = hit.collider;
                    _currentInteractable = interactable;
                    ShowPrompt(interactable.InteractionLabel[0], _countObject.m_craftLife);
                    
                    return;
                }
                
            }
            
            ResetInteraction();
        }

        private void OnDisable()
        {
            _playerInput.actions["Interact"].started -= OnInteract;
            _playerInput.onActionTriggered -= OnActionTriggered;
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnActionTriggered(InputAction.CallbackContext context)
        {
            if (context.control == null) return;
            
            var device = context.control.device;

            if (device != _lastUsedDevice)
            {
                _lastUsedDevice = device;
                
                DetectControllerType(device);
            }
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (_currentInteractable is not null && _lastCollider is not null)
            {
                float distance = Vector3.Distance(_playerTransform.position, _lastCollider.transform.position);
                
                if (distance <= _maxInteractionDistance)
                {
                    _currentInteractable.Interact();
                }
            }
            
        }

        #endregion
        
        
        #region Main Method

        private void ShowPrompt(string text, int count)
        {
            string composedText = $" {text} pour : {count}";
            
            if (composedText == _lastPromptText) return;
            
            _lastPromptText = composedText;
            _uiManager.ShowPrompt(composedText, _currentSprite);
        }
        
        private void HidePrompt()
        {
            _uiManager.HidePrompt();
        }
        
        private void ResetInteraction()
        {
            _lastCollider = null;
            _currentInteractable = null;
            _lastPromptText = string.Empty;
            HidePrompt();
        }

        private void DetectControllerType(InputDevice device)
        {
            string name = device.name.ToLower();
            string displayName = device.displayName.ToLower();

            if (device is Gamepad)
            {
                if (name.Contains("xbox") || name.Contains("xinput") || displayName.Contains("xbox"))
                {
                    _currentSprite = _spriteX;
                    Debug.Log("🎮 Xbox controller active.");
                    return;
                }

                if (name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense") || displayName.Contains("playstation"))
                {
                    _currentSprite = _spriteP;
                    Debug.Log("🎮 PlayStation controller active.");
                    return;
                }

                // Default for gamepads
                _currentSprite = _spriteX;
                return;
            }

            // Clavier/souris
            _currentSprite = _spritePC;
            Debug.Log("⌨️ PC (Keyboard/Mouse) active.");
        
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
            {
                Debug.Log($"[Controller Detection] Device {device.name} connected.");
            }

            if (change == InputDeviceChange.Removed)
            {
                Debug.Log($"[Controller Detection] Device {device.name} disconnected.");
                DetectControllerType(device);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [Header("Params")]
        [SerializeField] private float _interactionDetected = 30f;
        [SerializeField] private float _maxInteractionDistance = 2f;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private LayerMask _obstacleMask;
        
        [Header("References")]
        private Camera _camera;
        [SerializeField] private PlayerInput _playerInput;
        
        private IInteractable _currentInteractable;
        private Collider _lastCollider;
        private UIManager.Runtime.UIManager _uiManager;
        private CraftObject _countObject;
        
        [SerializeField] private Transform _playerTransform;
        private CraftObject _craftObject;
        private string _lastPromptText;
        
        [Header("Sprites PC")]
        [SerializeField] private Sprite _spritePC;
        [Header("Sprites Xbox")]
		[SerializeField] private Sprite _spriteX;
        [Header("Sprite Playstation")]
        [SerializeField] private Sprite _spriteP;
        private Sprite _currentSprite;
        private InputDevice _lastUsedDevice;

        #endregion
    }
}
