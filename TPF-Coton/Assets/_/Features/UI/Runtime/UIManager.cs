using TMPro;
using UnityEngine;

namespace UIManager.Runtime
{
    public class UIManager : MonoBehaviour
    {
        #region Public
        
        public static UIManager m_Instance;
        
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            m_Instance = this;
            HidePrompt();       
        }

        #endregion
        
        
        #region Utils

        public void ShowPrompt(string text)
        {
            _text.text = text;
            _text.gameObject.SetActive(true);
        }

        public void HidePrompt()
        {
            _text.gameObject.SetActive(false);       
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private TMP_Text _text;
        
        #endregion
    }
}
