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

        }

        private void Update()
        {
            Move();
            LifeTime();
        }

        private void OnCollisionEnter(Collision other)
        {
            AmmoPool.Instance.ReturnToPool(gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            AmmoPool.Instance.ReturnToPool(gameObject);
        }

        #endregion
        
        
        #region Main Method

        private void Move()
        {
            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
        }

        private void LifeTime()
        { 
            _timer += Time.deltaTime;
            
            if (_timer >= _duration)
            {
                AmmoPool.Instance.ReturnToPool(gameObject);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _duration = 1;
        
        private float _timer;

        #endregion
    }
}
