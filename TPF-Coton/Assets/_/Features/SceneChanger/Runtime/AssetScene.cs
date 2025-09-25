using UnityEngine;
using UnityEngine.SceneManagement;
namespace SceneChanger.Runtime
{
    public class AssetScene : MonoBehaviour
    {
      
        public void LoadGameScene()
        {
            
            SceneManager.LoadScene("DEMO");
        }

        public void LoadCreditScene()
        {
            
            SceneManager.LoadScene("Credit");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
