using UnityEngine;

namespace Enemy.Runtime
{
    public class ResetEnemy : MonoBehaviour
    {
        #region Unity Api

        [ContextMenu("Reset Enemies")]
        public void ResetEnemies()
        {
            foreach (GameObject enemies in _enemies)
            {
                if (enemies.activeSelf == false) enemies.SetActive(true);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject[] _enemies;
        
        #endregion
    }
}
