using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

namespace Interface.Runtime
{
    public interface IInspectable
    {
        string InspectionLabel { get; }
        Sprite InspectIconPC { get; }
        Sprite InspectIconXbox { get; }
        Sprite InspectIconPS { get; }
        Sprite GetIconForDevice(DeviceType device);
    }
}
