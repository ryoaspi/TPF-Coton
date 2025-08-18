using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Damage.Runtime
{
    public class WeaponEnemyDamage : MonoBehaviour
    {
        #region Public
        
        
        public int m_damage;
        
        #endregion
        
        
        #region Unity Api

        private void Update()
        {
            if (_isAttacking)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _target.position) <= 0.1f)
                    _isAttacking = false;
                

            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _origin.position, _speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _origin.position) <= 0.1f)
                    _isAttacking = true;
            }
        }

        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _origin;
        
        private bool _isAttacking;
        
        #endregion
    }
}
