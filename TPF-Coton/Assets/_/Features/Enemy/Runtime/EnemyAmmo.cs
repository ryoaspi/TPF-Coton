using System;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyAmmo : MonoBehaviour
    {
        #region Public
        
        [HideInInspector] public int m_damage;
        
        #endregion
        
        
        #region Unity Api
        
        private void OnEnable()
        {
            _timer = 0;
            _timeTouch = 0;
            _isTouch = false;
            m_damage = _damage;


        }

        private void Update()
        {
            Move();
            LifeTime();
            AppDamage();
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            _isTouch = true;
        }

        #endregion


		#region Utils

        public void InitDirection(Vector3 dir)
        {
            _direction = dir.normalized;
        }

		#endregion
        
        
        #region Main Method

        private void Move()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
        }

        private void LifeTime()
        { 
            _timer += Time.deltaTime;
            
            if (_timer >= _duration)
            {
                AmmoPool.Instance.ReturnToPool(gameObject);
            }
        }

        private void AppDamage()
        {
            if (_isTouch)
            {
                _timeTouch += Time.deltaTime;
                if (_timeTouch >= 1)
                {
                    _isTouch = false;
                    _timeTouch = 0;
                    AmmoPool.Instance.ReturnToPool(gameObject);
                }
            }
            
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _duration = 1;
        [SerializeField] private int _damage;
        
        private Vector3 _direction;
        private bool _isTouch;
        private float _timer;
        private float _timeTouch;
        
        #endregion
    }
}
