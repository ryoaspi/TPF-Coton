using System;
using UnityEngine;

namespace Player.Runtime
{
    public class ShieldController : MonoBehaviour
    {
        #region Public
        
        public int m_shield = 2;
        
        #endregion
        
        
        #region Unity Api

        private void Update()
        {
            if (_isBlocking)
            {
                transform.position = Vector3.MoveTowards(transform.position, _shieldTarget.position, _shieldSpeed * Time.deltaTime);

                if (!_isRotating)
                {
                    float rotationStep = 90 * Time.deltaTime;
                    transform.Rotate(Vector3.forward, rotationStep);
                    _rotationAmount += rotationStep;
                    if (_rotationAmount >= 90)
                    {
                        _isRotating = true;
                        _rotationAmount = 0f;
                    }
                        
                }

                if (Vector3.Distance(transform.position, _shieldTarget.position) <= 0.1f)
                {
                    _isBlocking = false;
                    _isRotating = false;
                }
                
            }
            
            else transform.position = Vector3.MoveTowards(transform.position, _shieldOrigin.position, _shieldSpeed * Time.deltaTime);
        }

        #endregion
        
        
        #region Utils

        public bool IsBlocking()
        {
            _isBlocking = true;
            return _isBlocking;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private Transform _shieldTarget;
        [SerializeField] private Transform _shieldOrigin;
        [SerializeField] private float _shieldSpeed = 10f;
        
        private bool _isBlocking;
        private float _rotationAmount = 0f;
        private bool _isRotating;

        #endregion
    }
}
