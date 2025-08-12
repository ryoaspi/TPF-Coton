using UnityEngine;

namespace Player.Runtime
{
    public class RapierControlle : MonoBehaviour
    {
        #region Public
        
        public int m_damage = 3;
        [HideInInspector] public bool _isAttacking;
        
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
                transform.position = Vector3.MoveTowards(transform.position, _origin.position, _speed * Time.deltaTime);
        }

        #endregion
        
        
        #region Utils

        public bool IsAttacking()
        {
            _isAttacking = true;
            return _isAttacking;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _origin;
        
        
        
        #endregion
    }
}
