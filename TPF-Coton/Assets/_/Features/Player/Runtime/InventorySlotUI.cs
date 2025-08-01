using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Player.Runtime
{
    public class InventorySlotUI : MonoBehaviour
    {
        #region Public
        
        public Image m_icon;
        public TextMeshProUGUI m_quantityText;
        
        #endregion
        
        
        #region Utils

        public void SetItem([CanBeNull] ItemData item)
        {
            if (item == null)
            {
                m_icon.enabled = false;
                m_quantityText.text = "";
            }
            else
            {
                m_icon.enabled = true;
                m_icon.sprite = item.m_icon;
                m_quantityText.text = item.m_quantity > 1 ? item.m_quantity.ToString() : "";
            }
        }
        
        #endregion
    }
}
