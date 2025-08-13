using UnityEngine;
using UnityEngine.SceneManagement;
namespace SceneChanger.Runtime
{
    public class AssetScene : MonoBehaviour
    {
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
