using Interface.Runtime;
using Player.Runtime;
using Sound.Runtime;
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

                if (_particleCraft is not null && _interactionPoint.Length > 0)
                { 
                    Vector3 position = (_interactionPoint[0].transform.position + _interactionPoint[1].transform.position) / 2;
                    _particleCraft.transform.position = position;
                    Debug.Log("Lancement VFX de craft à " + _particleCraft.transform.position);
                    _particleCraft.Play();
                }
                
                _soundEvent.PlaySoundEvent("Craft");
                
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
                if (_particleDecraft is not null)
                {
                    Vector3 position = (_interactionPoint[0].transform.position + _interactionPoint[1].transform.position) / 2;
                    _particleDecraft.transform.position = position;
                    Debug.Log("Lancement VFX de craft à " + _particleCraft.transform.position);
                    _particleDecraft.Play();
                }
                
                _soundEvent.PlaySoundEvent("Decraft");
                
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

        private Sprite _iconPC;
        private Sprite _iconXbox;
        private Sprite _iconPS;
        private IInteractable _interactableImplementation;
        
        [Header("VFX")]
        [SerializeField] private InteractionPoint[] _interactionPoint;
        [SerializeField] private ParticleSystem _particleCraft;
        [SerializeField] private ParticleSystem _particleDecraft;
        
        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SoundEvent _soundEvent;

        #endregion
    }
    
}
