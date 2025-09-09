using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public void ShowPrompt(string text, Sprite icon)
        {
            Debug.Log($"[UIManager] ShowPrompt received: {text}, Icon: {(icon ? icon.name : "null")}");
            
            _text.text = text;
            _text.gameObject.SetActive(true);
            
            if (_icon is not null)
            {
                _icon.sprite = icon;
                bool iconIsValid = icon is not null;
                _icon.enabled = iconIsValid;
                _icon.gameObject.SetActive(iconIsValid);
                
            }

        }

        public void HidePrompt()
        {
            Debug.Log("[UIManager] HidePrompt received");
            _text.gameObject.SetActive(false);
            if (_icon is not null) _icon.gameObject.SetActive(false);
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;

        #endregion
    }
}
