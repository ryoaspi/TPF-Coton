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

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                Coton coton = other.GetComponent<Coton>();
                if (coton is not null && !coton.CanBeCollected()) return;
                
                int cotonCollected = collectable.Collect();
                other.gameObject.SetActive(false);
                m_coton += cotonCollected;
                BuffStat();
                _textCoton.text = $"nombre de coton : {m_coton} \n Level Buff : {m_buff} " ;
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
            Debug.Log(m_buff);
        }
        
        #endregion
        
        
        #region Private And Protected

        [SerializeField] private int _numberCotonForBuff = 5;
        [SerializeField] private TMP_Text _textCoton;

        #endregion
    }
}
