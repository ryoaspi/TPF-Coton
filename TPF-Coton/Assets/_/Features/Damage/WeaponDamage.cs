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
            if (m_isAttacking)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _target.position) <= 0.1f)
                    m_isAttacking = false;
            }

            else
                transform.position = Vector3.MoveTowards(transform.position, _origin.position, _speed * Time.deltaTime);
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
        
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _origin;
        
        #endregion
    }
}
