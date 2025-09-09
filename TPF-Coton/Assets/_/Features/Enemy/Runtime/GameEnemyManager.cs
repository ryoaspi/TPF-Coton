using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Runtime
{
    public class GameEnemyManager : MonoBehaviour
    {
        #region Api Unity
       
        void Start()
        {
            _deadCount = 0;
            
            foreach (var enemy in _enemies)
            {
                if (enemy == null) continue;

                EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
                if (enemyStat != null) 
                {
                    enemyStat.OnDeath += OnEnemyDeath;
                }
            }
            
        }
        #endregion
        
        
        #region Main Methods

        private void OnEnemyDeath()
        {
            _deadCount++;

            if (_deadCount == _enemies.Count && _enemies.Count > 0)
            {
                // Action à faire quand tous les ennemis sont morts.
                if (_addGameObjects)
                {
                    _door.SetActive(true);
                }
                else _door.SetActive(false);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private List<GameObject> _enemies;
        [SerializeField] private bool _addGameObjects;
        private int _deadCount;

        [Header("Open")] [SerializeField] private GameObject _door;

        #endregion
    }
}
