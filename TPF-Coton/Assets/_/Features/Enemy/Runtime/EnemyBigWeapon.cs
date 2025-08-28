using System;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyBigWeapon : MonoBehaviour
    {
        #region Unity Api
        
        private void OnEnable()
        {
            _origin = transform.localPosition;
            _target = _origin + Vector3.down * 2f;
            _isMoving = true;
        }

        private void Update()
        {
            if (!_isMoving) return;
            
            transform.localPosition = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
            
            if (transform.localPosition == _target) _isMoving = false;
        }

        private void OnDisable()
        {
            transform.localPosition = _origin;
            _isMoving = false;
        }

        #endregion
        
        
        #region Private And Protected

        private Vector3 _origin;
        private Vector3 _target;
        private bool _isMoving;
        [SerializeField] private float _speed = 10f;

        #endregion
    }
}
