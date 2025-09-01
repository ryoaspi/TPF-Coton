using UnityEngine;
using UnityEngine.Analytics;

namespace Craft.Runtime
{
    public class ObjectStateManager : MonoBehaviour
    {
        #region Publics
        
        public bool m_isActive { get; private set; }
        public string[] m_insteractionLabels = new[] { "Activer", "Désactiver" };

        #endregion
        
        
        #region Utils

        public bool ToggleState()
        {
            if (m_isActive)
            {
                Deactivate();
                return false;
            }
            else
            {
                Activate();
                return true;
            }
        }
        
        #endregion
        
        
        #region Main Method

        private bool Activate()
        {
            m_isActive = true;
            if (_visualRepresentation is not null) _visualRepresentation.SetActive(true);
            return true;
        }
        
        private bool Deactivate()
        {
            m_isActive = false;
            if (_visualRepresentation is not null) _visualRepresentation.SetActive(false);
            return true;       
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject _visualRepresentation;
        
        #endregion
    }
}
