using UnityEngine;

namespace CheatCode.Runtime
{
    public class PlayerRespawn : MonoBehaviour
    {
       
        #region Utils

        
        private void Awake()
        {
            _spawnPoint = new GameObject("SpawnPoint").transform;
            _spawnPoint.position = _player.transform.position;
        }
        
        [ContextMenu("Respawn point")]
        public void Respawn()
        {
            _player.transform.position = _spawnPoint.position;
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [SerializeField] private GameObject _player;
        private Transform _spawnPoint;
        
        #endregion
    }
}
