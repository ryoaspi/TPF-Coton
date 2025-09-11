using Interface.Runtime;
using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

namespace Environment.Runtime
{
    public class NarratifPoint : MonoBehaviour, IInspectable
    {
        public string InspectionLabel => _LabelNarratif;
        public Sprite InspectIconPC { get; }
        public Sprite InspectIconXbox { get; }
        public Sprite InspectIconPS { get; }
        public Sprite GetIconForDevice(DeviceType device)
        {
            return null;
        }

        [SerializeField] private string _LabelNarratif;
    }
}
