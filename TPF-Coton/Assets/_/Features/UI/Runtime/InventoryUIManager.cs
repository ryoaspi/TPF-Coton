using System.Collections.Generic;
using Player.Runtime;
using TheFundation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UIManager.Runtime
{
    public class InventoryUIManager : FBehaviour
    {
        #region Unity Api

        private void OnEnable()
        {
            RefreshInventoryUI();
            UpdateSelectionVisual();
        }

        private void Update()
        {
            HangleNavigation();
        }

        #endregion
        
        
        #region Utils

        public void RefreshInventoryUI()
        {
            foreach (var button in _activeButton)
            {
                Destroy(button);
            }
            
            _activeButton.Clear();

            // Toujours créer 20 slots, même vides
            int slotsCount = _playerController.m_inventory.m_Maxslot;

            for (int i = 0; i < slotsCount; i++)
            {
                GameObject newButton = Instantiate(_itemButtonPrefab, _contentPanel);
                _activeButton.Add(newButton);
                
                // Récupère item à l'index i, ou null si vide
                ItemData item = null;
                if (i < _playerController.m_inventory.m_items.Count) 
                    item = _playerController.m_inventory.m_items[i];
                
                // Mets à jour le slot avec item ou vide
                newButton.GetComponent<InventorySlotUI>().SetItem(item);
                
                //Met à jour le text (nom ou vide)
                var text = newButton.GetComponentInChildren<TMP_Text>();
                text.text = item != null ? item.m_name : "";
                
                // Ajoute l'écouter, même si slot vide (à adapter si besoin)
                int index = i; // capture index local pour le lambda
                Button btn = newButton.GetComponent<Button>();
                btn.onClick.AddListener(() => OnItemClikedAtIndex(index));
            }
            
            _selectedIndex = Mathf.Clamp(_selectedIndex, 0, _activeButton.Count -1);
            UpdateSelectionVisual();
        }

        public void OnItemClikedAtIndex(int index)
        {
            ItemData item = null;
            if (index < _playerController.m_inventory.m_items.Count)
                item = _playerController.m_inventory.m_items[index];
            
            if (item != null) Debug.Log($"Item clicked: {item.m_name}");
            else Debug.Log("Slot vide cliqué");
            
            // Ici tu peux ouvrir menu utiliser / équiper / jeter selon item ou vide
        }
        
        #endregion
        
        
        #region Main Methods

        private void UpdateSelectionVisual()
        {
            for (int i = 0; i < _activeButton.Count; i++)
            {
                // Trouve le cadre de sélection (un enfant nommé "SelectionFrame")
                Transform frame = _activeButton[i].transform.Find("SelectionFrame");
                if (frame != null)
                {
                    frame.gameObject.SetActive(i == _selectedIndex);
                }
            }
        }

        private void HangleNavigation()
        {
            var gamepad = Gamepad.current;
            if (gamepad == null) return;
            
            Vector2 stick = gamepad.dpad.ReadValue();

            if (stick.x > 0.5f)
            {
                _selectedIndex = Mathf.Min(_selectedIndex + 1, _activeButton.Count);
                UpdateSelectionVisual();
            }
            else if (stick.x < -0.5f)
            {
                _selectedIndex = Mathf.Max(_selectedIndex - 1, 0);
                UpdateSelectionVisual();
            }

            if (stick.y > 0.5f)
            {
                // navigation vers le haut 
            }

            if (stick.y < -0.5f)
            {
                // navigation vers le bas
            }
        }
        
        #endregion
        
        
        #region Private And Protected

        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GameObject _itemButtonPrefab;
        [SerializeField] private Transform _contentPanel;
       
        private List<GameObject> _activeButton = new ();
        private int _selectedIndex = 0;

        #endregion
    }
}
