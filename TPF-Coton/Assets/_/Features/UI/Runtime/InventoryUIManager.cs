using System.Collections.Generic;
using Player.Runtime;
using TheFundation.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager.Runtime
{
    public class InventoryUIManager : FBehaviour
    {

        
        
        #region Unity Api

        private void OnEnable()
        {
            RefreshInventoryUI();
        }
        
        #endregion
        
        
        #region Utils

        public void RefreshInventoryUI()
        {
            // Supprimer les anciens boutons
            foreach (var button in _activeButtons)
            {
                Destroy(button);
            }   
            _activeButtons.Clear();
            
            // Afficher les nouveaux objets
            foreach (var items in _playerController.m_inventory.m_items)
            {
                GameObject newButton = Instantiate(_itemButtonPrefab, _contentPanel);
                newButton.GetComponentInChildren<TMP_Text>().text = items.m_name;
                
                // Pour une interaction futur (ex : équiper)
                Button btn = newButton.GetComponent<Button>();
                btn.onClick.AddListener(() => OnItemClicked(items));
                
                _activeButtons.Add(newButton);
            }
        }
        
        #endregion


        #region Main Method

        public void OnItemClicked(ItemData item)
        {
            // Ouverture menu "Utiliser/équiper/jeter"
        }

        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private GameObject _itemButtonPrefab;
        [SerializeField] private Transform _contentPanel;
        
        private List<GameObject> _activeButtons = new();

        #endregion
    }
}
