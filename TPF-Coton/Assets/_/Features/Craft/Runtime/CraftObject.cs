using Interface.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Craft.Runtime
{
    public class CraftObject : MonoBehaviour, IInteractable
    {
        #region Public
        
        [HideInInspector] public int m_craftCost;
        
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            _coton = FindFirstObjectByType<PlayerBuff>();
            if (_craftPrefab is not null) _craftPrefab.SetActive(false);
            m_craftCost = _craftCost;
        }

        #endregion
        
        
        #region Utils

        public void Interact()
        {
            Craft();
        }

        public string InteractionLabel => _interactionLabel;
        
        #endregion
        
        
        #region Main Method

        private bool Craft()
        {
            if (_coton.m_coton >= _craftCost + 1)
            {
                _coton.LoseCoton(_craftCost);
                _craftPrefab.SetActive(true);
                return true;
            }
            
            return false;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private int _craftCost = 20;
        private PlayerBuff _coton;
        [SerializeField] private GameObject _craftPrefab;
        [SerializeField] private string _interactionLabel;

        #endregion
    }
}
