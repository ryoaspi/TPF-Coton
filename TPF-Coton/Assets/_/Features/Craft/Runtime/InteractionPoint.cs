using Interface.Runtime;
using UnityEngine;

namespace Craft.Runtime
{
    public class InteractionPoint : MonoBehaviour, IInteractable
    {
        [SerializeField] private CraftObject _craftObject;

        public void Interact()
        {
            _craftObject.Interact();
        }

        public string[] InteractionLabel => _craftObject.InteractionLabel;

    }
}
