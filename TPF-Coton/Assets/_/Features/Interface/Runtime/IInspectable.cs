using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

namespace Interface.Runtime
{
    public interface IInspectable
    {
        bool HasBeenInspected { get; }
        void MarkInspected();
        string InspectionLabel { get; }
        Sprite InspectIconPC { get; }
        Sprite InspectIconXbox { get; }
        Sprite InspectIconPS { get; }
        Sprite InspectIconSwitch { get; }
        Sprite GetIconForDevice(DeviceType device);
    }
}
