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
            _text.text = text;
            
            if (_icon is not null)
            {
                _icon.sprite = icon;
                _icon.enabled = icon is not null;
                _icon.gameObject.SetActive(icon is not null);
            }
            _text.gameObject.SetActive(true);

        }

        public void HidePrompt()
        {
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
