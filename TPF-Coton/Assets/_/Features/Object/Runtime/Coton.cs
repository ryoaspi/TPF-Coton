using System;
using Interface.Runtime;
using UnityEngine;

namespace Object.Runtime
{
    public class Coton : MonoBehaviour, ICollectable
    {
        
        #region Unity Api

        private void Awake()
        {
            
        }

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
            if (!_wasCollectable && CanBeCollected())
            {
                _wasCollectable = true;
                
                _col.enabled = false;
                _col.enabled = true;
                
            }
        }

        private void OnDisable()
        {
            Destroy(gameObject);
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
        }

        #endregion
        
        #region Utils
        
        public int Collect()
        {
            _collected = true;
            return _valueCoton;
        }

        public void SetPhysicsActive(bool active)
        {
            _rb.isKinematic = !active;
            _col.isTrigger = !active;
        }

        public bool CanBeCollected()
        {
            bool canCollect = Time.time >= _spawnTime + _collectibleDelay;
            return canCollect;
        }

        public int GetCotonValue(int damage)
        {
            _valueCoton = damage;
            
            if (_valueCoton >= 2)
            {
                float upSize = _valueCoton * _multiplicateurScale;
                transform.localScale = new Vector3(transform.localScale.x + upSize, transform.localScale.y + upSize,transform.localScale.z + upSize);    
            }
            Debug.Log(_valueCoton);
            return _valueCoton;
        }
        
        #endregion
        
        
        #region Main Methods

        
        
        #endregion
        
        
        #region private and protected
        
        private bool _collected;
        private float _currentTime;
        private bool _isDone;
        private Collider _col;
        private Rigidbody _rb;
        
        private float _spawnTime;
        [SerializeField] private float _collectibleDelay = 0.5f;
        [SerializeField] private float _multiplicateurScale = 0.05f;

        private bool _wasCollectable;
        private int _valueCoton = 1;


        #endregion
    }
}
