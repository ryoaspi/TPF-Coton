using Enemy.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CheatCode.Runtime
{
    public class CheatCode : MonoBehaviour
    {
        
        #region Api Unity

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            
        }

        private void OnEnable()
        {
            var actions = _playerInput.actions;
            actions["RespawnPlayer"].started += Respawn;
            actions["ResetEnemy"].started += ResetEnemies;
        }

        private void OnDisable()
        {
            var actions = _playerInput.actions;
            actions["RespawnPlayer"].started -= Respawn;
            actions["ResetEnemy"].started -= ResetEnemies;
        }
        
        

        #endregion
        
       
        #region Utils
        
        private void Respawn(InputAction.CallbackContext context)
        {
            _playerRespawn.Respawn();
        }
        
        private void ResetEnemies(InputAction.CallbackContext context)
        {
            _resetEnemy.ResetEnemies();
        }
        
        #endregion
        
        
        #region Private And Protected
        
        private PlayerInput _playerInput;
        [SerializeField] private PlayerRespawn _playerRespawn;
        [SerializeField] private ResetEnemy _resetEnemy;
        
        #endregion
    }
}
