using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyShoot : MonoBehaviour
    {
        #region Unity Api
        
        

        #endregion
        
        #region Main Method

        private void Shooting()
        {
            if (_nextFire <= _fireRate)
                Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        }
        
        #endregion
        
        
        #region Private And Protected

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _fireRate = 2;
        private float _nextFire;
        
        #endregion
    }
}
