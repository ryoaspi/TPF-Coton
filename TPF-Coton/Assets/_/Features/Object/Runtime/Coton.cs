using Interface.Runtime;
using UnityEngine;

namespace Object.Runtime
{
    public class Coton : MonoBehaviour, ICollectable
    {
        #region Unity Api

        private void OnEnable()
        {
            _spawnTime = Time.time;
            _currentTime = 0;
            _col = GetComponent<Collider>();
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            _col.isTrigger = true;
        }

        private void Update()
        {
            if (_liveCoton) _currentTime += Time.deltaTime;
            
            if (_currentTime >= _lifeTime || _collected)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                _rb.isKinematic = true;
                _col.isTrigger = true;
            }
        }

        #endregion
        
        #region Utils
        
        public int Collect()
        {
            _collected = true;
            return 1;
        }

        public void SetPhysicsActive(bool active)
        {
            _rb.isKinematic = !active;
            _col.isTrigger = !active;
        }

        public bool CanBeCollected()
        {
            return Time.time >= _spawnTime + _collectibleDelay;
        }
        
        #endregion
        
        
        #region Main Methods

        
        
        #endregion
        
        
        #region private and protected
        
        private bool _collected;
        [SerializeField] private float _lifeTime;
        private float _currentTime;
        private bool _isDone;
        private Collider _col;
        private Rigidbody _rb;
        [SerializeField] private bool _liveCoton;
        
        private float _spawnTime;
        [SerializeField] private float _collectibleDelay = 0.5f;

        #endregion
    }
}
