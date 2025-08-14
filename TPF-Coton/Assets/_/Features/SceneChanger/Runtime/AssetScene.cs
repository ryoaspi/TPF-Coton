using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
namespace SceneChanger.Runtime
{
    public class AssetScene : MonoBehaviour
    {
        private PlayerInput _playerInput;

        public void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }
        public void LoadAssetScene()
        {
            
            SceneManager.LoadScene("Lucie");
        }

        public void LoadenemyScene()
        {
            
            SceneManager.LoadScene("Thomas");
        }
    }
}
