using UnityEngine;

namespace Player.Runtime
{
    public class FrondeBullet : MonoBehaviour
    {
        #region Public
        
        
        
        #endregion
        
        
        #region Unity Api
        
        private void OnEnable()
        {
            _timer = 0;
            _playerDropCoton=GetComponent<PlayerDropCoton>();
            _fronde = FindFirstObjectByType<Fronde>();

        }

        private void Update()
        {
            Move();
            LifeTime();
            AppDamage();
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (LayerMask.LayerToName(other.gameObject.layer) == "WeaponEnemy") return;
            if (LayerMask.LayerToName(other.gameObject.layer) == "NoContactBulletPlayer") return;
            if (LayerMask.LayerToName(other.gameObject.layer) == "Boundarie") return;
            if (LayerMask.LayerToName(other.gameObject.layer) == "Crafted") return;
            if (LayerMask.LayerToName(other.gameObject.layer) == "Ignore Raycast") return;
            if (LayerMask.LayerToName(other.gameObject.layer) == "Coton") return;
            if(LayerMask.LayerToName(other.gameObject.layer) == "BulletEnemy") return;
               
            _isTouch = true;
            _playerDropCoton.DropCotonDamage(_fronde.m_hpLoss);
            FrondePool.Instance.ReturnToPool(gameObject); 
            
            
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
                _playerDropCoton.DropCotonDamage(_fronde.m_hpLoss);
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
                    _playerDropCoton.DropCotonDamage(_fronde.m_hpLoss);
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
        private Fronde _fronde;
        private PlayerDropCoton _playerDropCoton;
        #endregion
    }
}
