using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

namespace Interface.Runtime
{
    public interface IInteractable
    {
        void Interact();
        string[] InteractionLabel { get; }
        int InteractionCost { get; }

        Sprite GetIconForDevice(DeviceType device);
    }
    
}
