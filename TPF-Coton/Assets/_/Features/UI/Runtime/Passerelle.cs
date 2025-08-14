using Player.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIManager.Runtime
{
    [DefaultExecutionOrder(-150)]
    public class Passerelle : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject m_assetLoadButton;
        public GameObject m_enemyLoadButton;
        public GameObject m_assetButtonSelector;
        public GameObject m_enemyButtonSelector1;
        
        void Awake()
        {
            canvas = GetComponent<Canvas>();
            m_assetLoadButton= transform.GetChild(0).gameObject;
            m_enemyLoadButton=transform.GetChild(1).gameObject;
            m_assetButtonSelector=transform.GetChild(0).GetChild(1).gameObject;
            m_enemyButtonSelector1=transform.GetChild(1).GetChild(1).gameObject;

            PlayerController Player = FindFirstObjectByType<PlayerController>();
            Player.GetRefCanvas(canvas,m_assetLoadButton,m_enemyLoadButton,m_assetButtonSelector,m_enemyButtonSelector1);

        }
        void Start()
        {
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        void OnEnable()
        {
            // S'abonne à l'événement
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            // Se désabonne pour éviter les fuites de mémoire
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Nouvelle scène chargée : " + scene.name);
            canvas = GetComponent<Canvas>();
            m_assetLoadButton= transform.GetChild(0).gameObject;
            m_enemyLoadButton=transform.GetChild(1).gameObject;
            m_assetButtonSelector=transform.GetChild(0).GetChild(1).gameObject;
            m_enemyButtonSelector1=transform.GetChild(1).GetChild(1).gameObject;

            PlayerController Player = FindFirstObjectByType<PlayerController>();
            Player.GetRefCanvas(canvas,m_assetLoadButton,m_enemyLoadButton,m_assetButtonSelector,m_enemyButtonSelector1);
        }
        
       
    }
}
