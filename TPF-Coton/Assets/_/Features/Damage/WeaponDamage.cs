using UnityEngine;

namespace Damage.Runtime
{
    public class WeaponDamage : MonoBehaviour
    {
        #region Public
        
        public int m_damage;
        [HideInInspector] public bool m_isAttacking;
        
        #endregion
        
        
        #region Unity Api

        private void Update()
        {
            
        }
        
        #endregion
        
        
        #region Utils

        public bool IsAttacking()
        {
            m_isAttacking = true;
            return m_isAttacking;
        }

        public void AddDamage(int damage)
        {
            m_damage += damage;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        
        
        #endregion
    }
}
