using UnityEngine;

namespace CheatCode.Runtime
{
    public class PlayerRespawn : MonoBehaviour
    {
       
        #region Utils
        
        [ContextMenu("Respawn point")]
        public void Respawn()
        {
            _player.transform.position = _spawnPoint.position;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject _player;
        [SerializeField] private Transform _spawnPoint;
        
        #endregion
    }
}
