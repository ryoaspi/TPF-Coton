using Interface.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

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
        }

        private void Update()
        {
            float offsetX = 0f;
            float offsetY = 100f;
            
            Vector3 screenPoint = new Vector3(Screen.width / 2 + offsetX, Screen.height / 2 + offsetY, 0f);
            
            Ray ray = new Ray(_playerTransform.position, _camera.ScreenPointToRay(screenPoint).direction);
            Debug.DrawRay(ray.origin, ray.direction * _interactionRange, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hit, _interactionRange, _layerMask))
            {
                
                if (_lastCollider != hit.collider)
                {
                    _lastCollider = hit.collider;
                    _currentInteractable = hit.collider.GetComponentInParent<IInteractable>();
                }
                if (_currentInteractable is  not null) ShowPrompt(_currentInteractable.InteractionLabel[0], _countObject.m_craftLife );
            }
            else
            {
                if (_lastCollider is not null)
                {
                    _lastCollider = null;
                    _currentInteractable = null;
                    _lastPromptText = string.Empty;
                    HidePrompt();
                }
            }
        }

        private void OnDisable()
        {
            _playerInput.actions["Interact"].started -= OnInteract;
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            _currentInteractable?.Interact();
        }

        #endregion
        
        
        #region Main Method

        private void ShowPrompt(string text, int count)
        {
            string composedText = $"[E] {text} pour : {count}";
            
            if (composedText == _lastPromptText) return;
            
            _lastPromptText = composedText;
            _uiManager.ShowPrompt(composedText);
        }
        
        private void HidePrompt()
        {
            _uiManager.HidePrompt();
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [Header("Params")]
        [SerializeField] private float _interactionRange = 2f;
        [SerializeField] private LayerMask _layerMask;
        
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

        #endregion
    }
}
