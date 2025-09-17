using Interface.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.Runtime;
using DeviceType = Core.Runtime.DeviceType;

namespace Craft.Runtime
{
    public class InteractionSystem : MonoBehaviour
    {
        #region Public
        
        
        
        #endregion
     
        
        #region Api Unity

        private void Awake()
        {
            _camera = Camera.main;
            _uiManager = FindFirstObjectByType<UIManager.Runtime.UIManager>();
            if (_camera == null) _camera = Camera.main;
            _countObject = FindObjectOfType<CraftObject>();
            DetectControllerType(Mouse.current);
        }

        private void OnEnable()
        {
            _playerInput.actions["Interact"].started += OnInteract;
            _playerInput.onActionTriggered += OnActionTriggered;
            _playerInput.actions["ClosePrompt"].started += OnClosePrompt;
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void Update()
        {
            if (_isPromptLocked) return;
            
            float offsetX = 0f;
            float offsetY = 100f;
            
            Vector3 screenPoint = new Vector3(Screen.width / 2 + offsetX, Screen.height / 2 + offsetY, 0f);
            
            Ray ray = new Ray(_playerTransform.position, _playerTransform.forward.normalized);
            Debug.DrawRay(ray.origin, ray.direction * _interactionDetected, Color.red);
            
            if (Physics.Raycast(ray, out RaycastHit hit, _interactionDetected, _layerMask, QueryTriggerInteraction.Collide))
            {
                
                float distanceToHit = Vector3.Distance(_playerTransform.position, hit.point);

                if (Physics.Raycast(_playerTransform.position, (hit.point - _playerTransform.position).normalized,
                        out RaycastHit obstacleHit, distanceToHit, _obstacleMask))
                {
                    ResetInteraction();
                    return;
                }
                
                // D'abord les vrais interactable
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable is not null && distanceToHit <= _maxInteractionDistance)
                {
                    _lastCollider = hit.collider;
                    _currentInteractable = interactable;

                    Sprite icon = interactable.GetIconForDevice(_currentDeviceType);
                    string text = interactable.InteractionLabel[0];
                    int cost = interactable.InteractionCost;
                    
                    _uiManager.ShowPrompt($"{text} pour : {cost}",icon, true, "", null);
					
                    
                    return;
                }
                
                // Check les objets inspectables
                IInspectable inspectable = hit.collider.GetComponent<IInspectable>();
                if (inspectable is not null && distanceToHit <= _maxInteractionDistance)
                {
                    string text = inspectable.InspectionLabel;
                    Sprite icon = inspectable.GetIconForDevice(_currentDeviceType);
                    _uiManager.ShowPrompt($"{text}", icon , IsPersistentPrompt(hit.collider.gameObject),
                        IsPersistentPrompt(hit.collider.gameObject) ? GetClosePromptText() : "",
                        IsPersistentPrompt(hit.collider.gameObject) ? GetClosePromptSprite() : null);
                    
                    if (IsPersistentPrompt(hit.collider.gameObject)) _isPromptLocked = true;
                }
                
            }
            
            else if (!_isPromptLocked)
            {
                ResetInteraction();
            }
        }



        private void OnDisable()
        {
            _playerInput.actions["Interact"].started -= OnInteract;
            _playerInput.onActionTriggered -= OnActionTriggered;
            _playerInput.actions["ClosePrompt"].started -= OnClosePrompt;
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

        private void OnClosePrompt(InputAction.CallbackContext context)
        {
            if (_isPromptLocked)
            {
                ResetInteraction();
                _isPromptLocked = false;
            }
        }

        #endregion
        
        
        #region Main Method

        private void ShowPrompt(string text, int count)
        {
            string composedText = $" {text} pour : {count}";
            
            if (composedText == _lastPromptText) return;
            
            _lastPromptText = composedText;
            _uiManager.ShowPrompt(composedText,null);
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

        private DeviceType _currentDeviceType = DeviceType.PC;

        private void DetectControllerType(InputDevice device)
        {
            string name = device.name.ToLower();
            string displayName = device.displayName.ToLower();

            if (device is Gamepad)
            {
                if (name.Contains("xbox") || name.Contains("xinput") || displayName.Contains("xbox"))
                {
                    _currentDeviceType = DeviceType.Xbox;
                    
                    return;
                }

                if (name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense") || displayName.Contains("playstation"))
                {
                    _currentDeviceType = DeviceType.PlayStation;
                    
                    return;
                }

                // Default for gamepads
                _currentDeviceType = DeviceType.Xbox;
                
                return;
            }

            // Clavier/souris
            _currentDeviceType = DeviceType.PC;
            
        
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected)
            {
            }

            if (change == InputDeviceChange.Removed)
            {
                DetectControllerType(device);
            }
        }

        private bool IsPersistentPrompt(GameObject obj)
        {
            return (_persistentPromptMask.value & (1 << obj.layer)) != 0;
        }

        private Sprite GetClosePromptSprite()
        {
            switch (_currentDeviceType)
            {
                case DeviceType.PC:
                    return _spriteKeyboard;
                case DeviceType.Xbox:
                    return _spriteXbox;
                case DeviceType.PlayStation:
                    return _spritePlayStation;
                default:
                    return null;
            }
        }
        
        private string GetClosePromptText()
        {
            return _closePromptText;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [Header("Params")]
        [SerializeField] private float _interactionDetected = 30f;
        [SerializeField] private float _maxInteractionDistance = 2f;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _persistentPromptMask;
        [SerializeField] private Sprite _spriteKeyboard;
        [SerializeField] private Sprite _spriteXbox;
        [SerializeField] private Sprite _spritePlayStation;
        [SerializeField] private string _closePromptText = "Pour Fermer";
        
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
        
        private InputDevice _lastUsedDevice;
        private bool _isPromptLocked;
		

        #endregion
    }
}
