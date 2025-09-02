using UnityEngine;

namespace Player.Runtime
{
    public class FrondeBullet : MonoBehaviour
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
            AppDamage();
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            _isTouch = true;
            gameObject.SetActive(false);
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
                FrondePool.Instance.ReturnToPool(gameObject);
            }
        }

        private void AppDamage()
        {
            if (_isTouch)
            {
                _timeTouch += Time.deltaTime;
                if (_timeTouch >= 1)
                {
                    _timeTouch = 0;
                    _isTouch = false;
                    FrondePool.Instance.ReturnToPool(gameObject);
                }
            }
            
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _duration = 1;
        
        private bool _isTouch;
        private float _timer;
        private float _timeTouch;

        #endregion
    }
}
