using Interface.Runtime;
using UnityEngine;

namespace Object.Runtime
{
    public class Coton : MonoBehaviour, ICollectable
    {
        #region Unity Api

        private void OnEnable()
        {
            _currentTime = 0;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            
            if (_currentTime >= _lifeTime || _collected)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion
        
        #region Utils
        
        public int Collect()
        {
            _collected = true;
            return 1;
        }
        
        #endregion
        
        
        #region Main Methods

        
        
        #endregion
        
        
        #region private and protected
        
        private bool _collected;
        [SerializeField] private float _lifeTime;
        private float _currentTime;
        private bool _isDone;

        #endregion
    }
}
