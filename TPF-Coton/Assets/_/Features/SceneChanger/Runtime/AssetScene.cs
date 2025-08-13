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
            _playerInput.SwitchCurrentActionMap("Player");
            SceneManager.LoadScene("Lucie");
        }

        public void LoadenemyScene()
        {
            _playerInput.SwitchCurrentActionMap("Player");
            SceneManager.LoadScene("Thomas");
        }
    }
}
