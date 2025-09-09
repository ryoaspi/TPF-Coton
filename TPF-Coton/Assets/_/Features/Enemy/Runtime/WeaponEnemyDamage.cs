using UnityEngine;

namespace Enemy.Runtime
{
    public class WeaponEnemyDamage : MonoBehaviour
    {
        #region Public
        
        [HideInInspector] public bool m_isAttacking;
        
        #endregion

        
        #region Utils

        public void Attack()
        {
            if (!_target || !_origin) return;
            
            if (m_isAttacking)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _target.position) <= 0.1f)
                    m_isAttacking = false;
            }
            
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _origin.position, _speed * Time.deltaTime);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _origin;

        #endregion
    }
}