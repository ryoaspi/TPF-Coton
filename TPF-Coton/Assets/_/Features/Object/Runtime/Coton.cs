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

            if (!_wasCollectable && CanBeCollected())
            {
                _wasCollectable = true;
                
                _col.enabled = false;
                _col.enabled = true;
                
                Debug.Log("Coton collectable");
            }
        }
        


        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                _rb.isKinematic = true;
                _col.isTrigger = true;
            }
        }

        private void OntriggerEnter(Collider other)
        {
            if (!CanBeCollected())return;
            Debug.Log($"Coton touché par : {other.gameObject.name}");
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
            bool canCollect = Time.time >= _spawnTime + _collectibleDelay;
            Debug.Log($"Can be collected : {canCollect}");
            return canCollect;
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

        private bool _wasCollectable;

        #endregion
    }
}
