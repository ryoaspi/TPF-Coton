using Damage.Runtime;
using UnityEngine;

namespace Enemy.Runtime
{
    public class EnemyShoot : MonoBehaviour
    {
        #region Unity Api

        private void Update()
        {
            _nextFire += Time.deltaTime;
            Shooting();
        }

        #endregion
        
        #region Main Method

        private void Shooting()
        {
            if (_nextFire >= _fireRate)
            {
                GameObject bullet = AmmoPool.Instance.GetFromPool();
                bullet.transform.position = _firePoint.position;
                bullet.transform.rotation = _firePoint.rotation;
                
                _nextFire = 0;
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private float _fireRate = 2;
        [SerializeField] private Transform _firePoint;
        private float _nextFire;
        
        #endregion
    }
}
