using System;
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
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                Coton coton = other.GetComponent<Coton>();
                if (coton is not null && !coton.CanBeCollected()) return;
                
                
                if (_playerStats.m_currentHealth == _playerStats.m_publicHP)
                {
                    int cotonCollected = collectable.Collect();
                    m_coton += cotonCollected;
                    other.gameObject.SetActive(false);
                    BuffStat();  
                }
                else
                {
                    _playerStats.m_currentHealth++;
                    _playerStats.UpdateTextHealth();
                    other.gameObject.SetActive(false);
                }
                
            }
        }

        #endregion
        
        
        #region Utils

        public void LoseCoton(int amout)
        {
            m_coton -= amout;
            if (m_coton < 0) m_coton = 0;
            BuffStat();
        }
        
        #endregion
        
        
        #region Main Methods

        private void BuffStat()
        {
            m_buff = m_coton / _numberCotonForBuff;
            _playerStats.m_publicDamage=_playerStats.m_privateDamage+m_buff;
            _playerStats.m_publicHP=_playerStats.m_privateHP+m_buff;
            if (_playerStats.m_currentHealth == _playerStats.m_publicHP-m_buff)
            {
                _playerStats.m_currentHealth=_playerStats.m_publicHP;
            }
            if (_playerMovement.m_speed > _minimalSpeed)
            {
                _playerMovement.m_speed = _playerMovement.m_speedSave - m_buff;
            }
            _playerStats.UpdateTextHealth();
            Debug.Log(m_buff);
        }
        
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
