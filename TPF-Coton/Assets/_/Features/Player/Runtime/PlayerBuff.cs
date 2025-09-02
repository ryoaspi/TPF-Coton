using Interface.Runtime;
using Object.Runtime;
using TMPro;
using UnityEngine;

namespace Player.Runtime
{
    public class PlayerBuff : MonoBehaviour
    {
        #region Publics

        [HideInInspector] public int m_buff;
        [HideInInspector] public int m_coton;

        #endregion


        #region Unity Api

        private void Start()
        {
            _playerStats=GetComponent<PlayerStats>();
            _playerMovement=GetComponent<PlayerMovement>();
            m_coton = _playerStats.m_currentHealth;
            _textCoton.text = $"nombre de coton : {_playerStats.m_currentHealth}" ;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.TryGetComponent(out ICollectable collectable))
            {
                Coton coton = other.GetComponent<Coton>();
                if (coton is not null && !coton.CanBeCollected()) return;
                
                _playerStats.m_currentHealth++;
                m_coton = _playerStats.m_currentHealth;
                _textCoton.text = $"nombre de coton : {_playerStats.m_currentHealth} " ;
                collectable.Collect();
            }
        }

        #endregion
        
        
        #region Utils

        public void LoseCoton(int amout)
        {
            _playerStats.m_currentHealth -= amout;
            m_coton = _playerStats.m_currentHealth;
            _textCoton.text = $"nombre de coton : {_playerStats.m_currentHealth}" ;
        }
        
        #endregion
        
        
        #region Main Methods

        // private void BuffStat()
        // {
        //     m_buff = m_coton / _numberCotonForBuff;
        //     _playerStats.m_publicDamage=_playerStats.m_privateDamage+m_buff;
        //     _playerStats.m_publicHP=_playerStats.m_privateHP+m_buff;
        //     if (_playerMovement.m_speed > _minimalSpeed)
        //     {
        //         _playerMovement.m_speed = _playerMovement.m_speedSave - m_buff;
        //     }
        // }
        
        #endregion
        
        
        #region Private And Protected

        [SerializeField] private int _numberCotonForBuff = 5;
        [SerializeField] private TMP_Text _textCoton;
        private PlayerStats _playerStats;
        private PlayerMovement _playerMovement;
        [SerializeField] private float _minimalSpeed=5;

        #endregion
    }
}
