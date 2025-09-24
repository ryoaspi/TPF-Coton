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
                TriggerAction();
            }
        }

        private void TriggerAction()
        {
            switch (_actionToPerform)
            {
                case ActionType.None:
                    break;
                
                case ActionType.ActivateGameObject:
                    if (_door != null) _door.SetActive(true);
                    break;
                
                case ActionType.DeactivateGameObject:
                    if (_door != null) _door.SetActive(false);
                    break;
                
                case ActionType.PlayVFX:
                    if (_vfx != null) _vfx.Play();
                    break;
                case ActionType.Both:
                    if(_door != null) _door.SetActive(true);
                    if(_vfx != null) _vfx.Play();
                    break;
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private List<GameObject> _enemies;
        private int _deadCount;

        [Header("Action Settings")]
        [SerializeField] private ActionType _actionToPerform =  ActionType.None;
        
        [Header("Object to Trigger")] 
        [SerializeField] private GameObject _door;
        [SerializeField] private ParticleSystem _vfx;

        private enum ActionType
        {
            None,
            ActivateGameObject,
            DeactivateGameObject,
            PlayVFX,
            Both
        }

        #endregion
    }
}
