using Interface.Runtime;
using Object.Runtime;
using Sound.Runtime;
using TMPro;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerBuff : MonoBehaviour
    {
        #region Publics
        
        [HideInInspector] public int m_coton;

        #endregion


        #region Unity Api

        private void Start()
        {
            _shield=GetComponent<Shield>();
            _playerStats=GetComponent<PlayerStats>();
            _playerMovement=GetComponent<PlayerMovement>();
            m_coton = _playerStats.m_currentHealth;
            CheckSize();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                if (_playerStats.m_currentHealth < _playerStats.m_privateHP && !_shield.m_isShielding)
                {
                    Coton coton = other.GetComponent<Coton>();
                    if (coton is not null && !coton.CanBeCollected()) return;
                    _soundEvent.PlaySoundEventScript("GetCoton");
                    _playerStats.m_currentHealth += collectable.Collect();
                    m_coton = _playerStats.m_currentHealth; 
                    CheckSize();
                    _playerStats.UpdateTextHealth(); 
                    other.gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {

            
        }
        #endregion
        
        
        #region Utils

        public void LoseCoton(int amout)
        {
            _playerStats.m_currentHealth -= amout;
            m_coton = _playerStats.m_currentHealth;
            _playerStats.UpdateTextHealth();
        }

        public void CheckSize()
        {
            if (m_coton <= _cotonForLittleState)
            {
                if (_playerStats.m_currentState == 2)
                {
                    _soundEvent.PlaySoundEventScript("LevelDown");
                }

                _playerStats.LittleState();
            }
            else if (m_coton >= _cotonForBigState)
            {
                if (_playerStats.m_currentState == 2)
                {
                    _soundEvent.PlaySoundEventScript("LevelUp");
                }
                
                _playerStats.BigState();
            }
            else
            {
                if (_playerStats.m_currentState==3)
                {
                    _soundEvent.PlaySoundEventScript("LevelDown");
                }

                if (_playerStats.m_currentState == 1)
                {
                    _soundEvent.PlaySoundEventScript("LevelUp");
                }
                _playerStats.MediumState();
            }
        }
        #endregion
        
        
        #region Main Methods

       
        
        #endregion
        
        
        #region Private And Protected

        [Header("State")] 
        [SerializeField] private int _cotonForLittleState;
        [SerializeField] private int _cotonForBigState;
        [SerializeField] private TMP_Text _textCoton;
        private int _cotonState;
        private PlayerStats _playerStats;
        private PlayerMovement _playerMovement; 
        private Shield _shield;
        
        [Header("Speed")]
        [SerializeField] private float _minimalSpeed=5;
        [SerializeField] private SoundEvent _soundEvent;

        #endregion
    }
}
