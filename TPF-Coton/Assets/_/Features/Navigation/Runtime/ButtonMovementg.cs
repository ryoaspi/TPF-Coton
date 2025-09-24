using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Navigation.Runtime
{
    public class ButtonMovementg : MonoBehaviour
    {
        [SerializeField] private Image[] _Selected;

        void Update()
        {
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

            for (int i = 0; i < _Selected.Length; i++)
            {
                _Selected[i].gameObject.SetActive(false);
            }

            if (selectedObj != null)
            {
                switch (selectedObj.name)
                {
                    case "Start":
                        _Selected[0].gameObject.SetActive(true);
                        break;
                    case "Credit":
                        _Selected[1].gameObject.SetActive(true);
                        break;
                    case "QuitButton":
                        _Selected[2].gameObject.SetActive(true);
                        break;
                }
            }
        }
    }
}