using Interface.Runtime;
using Player.Runtime;
using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

namespace Craft.Runtime
{
    public class CraftObject : MonoBehaviour, IInteractable
    {
        #region Public
        
        [HideInInspector] public int m_craftLife;
        public int InteractionCost => _craftCost;
        
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

        public Sprite GetIconForDevice(DeviceType device)
        {
            return device switch
            {
                DeviceType.PC => _iconPC,
                DeviceType.Xbox => _iconXbox,
                DeviceType.PlayStation => _iconPS,
                _ => _iconPC
            };
        }

        #endregion
        
        
        #region Main Method

        private bool Craft()
        {
            if (_craftPrefab.activeSelf) return false;
            
            if (_coton.m_coton >= _craftCost + 1)
            {
                _coton.LoseCoton(_craftCost);
                foreach (var collider in _colliderSecurity) collider.enabled= false;
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
                foreach (var collider in _colliderSecurity) collider.enabled = true;
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
        [SerializeField] private Collider[] _colliderSecurity;
        private bool _isCrafted;

        [SerializeField] private Sprite _iconPC;
        [SerializeField] private Sprite _iconXbox;
        [SerializeField] private Sprite _iconPS;
        private IInteractable _interactableImplementation;

        #endregion
    }
    
}
