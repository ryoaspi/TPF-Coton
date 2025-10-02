using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UIManager.Runtime
{
    public class EndGameUiManager : MonoBehaviour
    {
        [Header("UI References")] 
        public GameObject m_deathUI;
        public GameObject m_victoryUI;
        
        [Header("First Selectable Buttons")]
        public GameObject m_firstDeathButton;
        public GameObject m_firstVictoryButton;
        
        private PlayerInput _playerInput;
        private bool _menuActive;

        private void Awake()
        {
            // Cache the PlayerInput component
            _playerInput = FindObjectOfType<PlayerInput>();
        }

        public void ShowDeathUI()
        {
            if (_menuActive) return;
            
            _menuActive = true;
            
            m_victoryUI.SetActive(false);
            m_deathUI.SetActive(true);

            _playerInput.SwitchCurrentActionMap("UI");
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_firstDeathButton);
        }

        public void ShowVictoryUI()
        {
            if (_menuActive) return;
            
            _menuActive = true;
            
            m_deathUI.SetActive(false);
            m_victoryUI.SetActive(true);

            _playerInput.SwitchCurrentActionMap("UI");
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_firstVictoryButton);
        }
        
    }
}
