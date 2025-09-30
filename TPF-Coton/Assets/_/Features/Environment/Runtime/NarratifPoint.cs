using Interface.Runtime;
using UnityEngine;
using DeviceType = Core.Runtime.DeviceType;

namespace Environment.Runtime
{
    public class NarratifPoint : MonoBehaviour, IInspectable
    {
        public bool HasBeenInspected => _hasBeenInspected;
        public void MarkInspected()
        {
            _hasBeenInspected  = true;
        }

        public string InspectionLabel => _LabelNarratif;
        public Sprite InspectIconPC => _spritePortrait;
        public Sprite InspectIconXbox => _spritePortrait;
        public Sprite InspectIconPS => _spritePortrait;
        public Sprite InspectIconSwitch => _spritePortrait;
        public Sprite GetIconForDevice(DeviceType device)
        {
            return _spritePortrait;
        }

        [SerializeField] private string _LabelNarratif;
        [SerializeField] private Sprite _spritePortrait;
        private bool _hasBeenInspected;
    }
}
