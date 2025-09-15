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

        public void ShowPrompt(string text, Sprite icon, bool showCloseHint = false, string closeHintText = "", Sprite closeHintIcon = null)
        {
            _text.text = text;
            _text.gameObject.SetActive(true);
            
            if (_icon is not null)
            {
                _icon.sprite = icon;
                bool iconIsValid = icon is not null;
                _icon.enabled = iconIsValid;
                _icon.gameObject.SetActive(iconIsValid);
                
            }

            if (_pressToCloseText is not null)
            {
                if (showCloseHint && !string.IsNullOrEmpty(closeHintText))
                {
                    _pressToCloseText.text = closeHintText;
                    _pressToCloseText.gameObject.SetActive(true);
                }
                else
                {
                    _pressToCloseText.gameObject.SetActive(false);
                }
            }

            if (_pressToCloseButton is not null)
            {
                if (showCloseHint && closeHintIcon is not null)
                {
                    _pressToCloseButton.sprite = closeHintIcon;
                    _pressToCloseButton.gameObject.SetActive(true);
                }
                else
                {
                    _pressToCloseButton.gameObject.SetActive(false);
                }
            }

        }

        public void HidePrompt()
        {
            _text.gameObject.SetActive(false);
            if (_icon is not null) _icon.gameObject.SetActive(false);
            if (_pressToCloseText is not null) _pressToCloseText.gameObject.SetActive(false);
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _pressToCloseText;
        [SerializeField] private Image _pressToCloseButton;

        #endregion
    }
}
