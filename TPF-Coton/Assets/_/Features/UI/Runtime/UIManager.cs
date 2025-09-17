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
            
            if (_pressToCloseTextObject is not null) _pressToCloseTextObject.SetActive(showCloseHint);
            
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
            }

            if (_pressToCloseButton is not null)
            {
                _pressToCloseButton.sprite = closeHintIcon;
                _pressToCloseButton.enabled = closeHintIcon is not null;
                _pressToCloseButton.gameObject.SetActive(closeHintIcon is not null);
            }

        }

        public void HidePrompt()
        {
            _text.gameObject.SetActive(false);
            
            if (_pressToCloseTextObject is not null) _pressToCloseTextObject.SetActive(false);

            if (_icon is not null)
            {
                _icon.sprite = null;
                _icon.gameObject.SetActive(false);
            }

            if (_pressToCloseText is not null)
            {
                _pressToCloseText.text = string.Empty;
                _pressToCloseText.gameObject.SetActive(false);
            }

            if (_pressToCloseButton is not null)
            {
                _pressToCloseButton.sprite = null;
                _pressToCloseButton.enabled = false;
                _pressToCloseButton.gameObject.SetActive(false);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _pressToCloseText;
        [SerializeField] private Image _pressToCloseButton;
        [SerializeField] private GameObject _pressToCloseTextObject;

        #endregion
    }
}
