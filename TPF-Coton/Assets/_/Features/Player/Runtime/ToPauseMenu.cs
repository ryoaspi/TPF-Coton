using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Player.Runtime
{
    public class ToPauseMenu : MonoBehaviour
    {
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.onActionTriggered += OnActionTrigger;
            var actions = _playerInput.actions;
            actions["ToPause"].performed += ToPause;
        }

        private void OnDisable()
        {
            _playerInput.onActionTriggered -= OnActionTrigger;
            var actions = _playerInput.actions;
            actions["ToPause"].performed -= ToPause;
        }

        private void OnActionTrigger(InputAction.CallbackContext context)
        {
            if (context.action.name != "ToPause" || !context.performed) return;
            
            ToPause(context);
        }

        public void ToPause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
        public void PauseGame()
        {
            toPauseMenu.SetActive(true);
            Time.timeScale = 0;
            _isPaused = true;
            
            _playerInput.SwitchCurrentActionMap("UI");
            EventSystem.current.SetSelectedGameObject(restartButton);
        }

        public void ResumeGame()
        {
            _playerInput.SwitchCurrentActionMap("Player");
            toPauseMenu.SetActive(false);
            Time.timeScale = 1;
            _isPaused = false;
        }
        
        [SerializeField] private GameObject toPauseMenu;
        [SerializeField] private GameObject restartButton;
        private bool _isPaused = false;
        private PlayerInput _playerInput;
        
    }
}
