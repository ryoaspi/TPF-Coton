using Interface.Runtime;
using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

namespace Environment.Runtime
{
    public class NarratifPoint : MonoBehaviour, IInspectable
    {
        public string InspectionLabel => _LabelNarratif;
        public Sprite InspectIconPC => _spritePortrait;
        public Sprite InspectIconXbox => _spritePortrait;
        public Sprite InspectIconPS => _spritePortrait;
        public Sprite GetIconForDevice(DeviceType device)
        {
            return device switch
            {
                _ => _spritePortrait
            };
        }

        [SerializeField] private string _LabelNarratif;
        [SerializeField] private Sprite _spritePortrait;
    }
}
