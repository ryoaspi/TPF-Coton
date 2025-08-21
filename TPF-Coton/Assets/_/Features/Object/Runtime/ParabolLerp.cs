using UnityEngine;

namespace Object.Runtime
{
    public class ParabolLerp : MonoBehaviour
    {
        #region Public
        
        public event System.Action OnLerpComplete;
        
        #endregion
        
        
        #region Unity Api

        private void Update()
        {
            if (!_isLerping) return;
        
            if (_time < _duration)
            {
                _time += Time.deltaTime;
                float t = _time / _duration;
            
                Vector3 basePos = Vector3.Lerp(_startPos, _endPos, t);
                float arc = _height * t * (1 - t);
            
                basePos.y += arc;
            
                transform.position = basePos;
            }
            else
            {
                _isLerping = false;
                OnLerpComplete?.Invoke();
            }
        }

        #endregion
    
    
        #region Utils

        public void Lerp(Vector3 startPos, Vector3 endPos,float height, float duration)
        {
            _startPos = startPos;
            _endPos = endPos;
            _height = height;
            _duration = duration;
            _time = 0;
            _isLerping = true;
        }
    
        #endregion
    
        #region Private And Protected
    
        private Vector3 _startPos;
        private Vector3 _endPos;
        private float _time;
        private float _height;
        private float _duration;
    
        private bool _isLerping;

        #endregion
    }
}
