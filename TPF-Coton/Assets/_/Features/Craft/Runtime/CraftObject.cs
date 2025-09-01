using Interface.Runtime;
using Player.Runtime;
using UnityEngine;

namespace Craft.Runtime
{
    public class CraftObject : MonoBehaviour, IInteractable
    {
        #region Public
        
        [HideInInspector] public int m_craftLife;
        
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            _coton = FindFirstObjectByType<PlayerBuff>();

            if (_craftPrefab is null)
            {
                return;           
            }
            m_craftLife = _craftCost;
        }

        private void OnEnable()
        {
            if (_craftPrefab is not null) _craftPrefab.gameObject.SetActive(false);
        }

        #endregion
        
        
        #region Utils

        public void Interact()
        {
            if (!_isCrafted) Craft();
            else Decraft();
        }

        public string[] InteractionLabel => new [] {_isCrafted ? _interactionLabel[1] : _interactionLabel[0]} ;
        
        #endregion
        
        
        #region Main Method

        private bool Craft()
        {
            if (_craftPrefab.activeSelf) return false;
            
            if (_coton.m_coton >= _craftCost + 1)
            {
                _coton.LoseCoton(_craftCost);
                _craftPrefab.gameObject.SetActive(true);
                _isCrafted = true;
                return true;
            }
            return false;
        }

        private bool Decraft()
        {
            if (_craftPrefab.activeSelf)
            {
                _coton.LoseCoton(-_craftCost);
                _craftPrefab.gameObject.SetActive(false);
                _isCrafted = false;
                return true;
            }
            return false;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private int _craftCost = 5;
        private PlayerBuff _coton;
        [SerializeField] private GameObject _craftPrefab;
        [SerializeField] private string[] _interactionLabel;
        private bool _isCrafted;

        #endregion
    }
    
}
