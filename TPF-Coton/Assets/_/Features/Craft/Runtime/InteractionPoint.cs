using Interface.Runtime;
using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

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
        public int InteractionCost => _craftObject.InteractionCost;
        public Sprite GetIconForDevice(Core.Runtime.DeviceType device)
        {
            return device switch
            {
                DeviceType.PC => _iconPC,
                DeviceType.Xbox => _iconXbox,
                DeviceType.PlayStation => _iconPlayStation,
                _ => _iconPC
            };
        }

        public Sprite CotonIcon => null;

        [SerializeField] private Sprite _iconPC;
        [SerializeField] private Sprite _iconXbox;
        [SerializeField] private Sprite _iconPlayStation;
    }
}
