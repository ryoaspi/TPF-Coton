using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyAmmo : MonoBehaviour
    {
        #region Public
        
        [HideInInspector]
        public int m_damage;
        
        #endregion
        
        
        #region Unity Api

        private void OnEnable()
        {
            m_damage = _damage;
            _currentduration = _duration;
        }
        
        private void Update()
        {
            Move();
            if (_currentduration <= 0)
                gameObject.SetActive(false);
            else _currentduration -= Time.deltaTime;
        }
        
        #endregion


        #region Main Method

        private void Move() => transform.position = Vector3.forward * (_speed * Time.deltaTime);

        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _speed = 3;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _duration = 2;

        private float _currentduration;

        #endregion
    }
}
