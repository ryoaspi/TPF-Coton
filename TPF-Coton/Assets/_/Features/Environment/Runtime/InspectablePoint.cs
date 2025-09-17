using Interface.Runtime;
using UnityEngine;

namespace Environment.Runtime
{
    public class InspectablePoint : MonoBehaviour, IInspectable
    {

        public string InspectionLabel => _label;
        public Sprite InspectIconPC => _iconPC;
        public Sprite InspectIconXbox => _iconXbox;
        public Sprite InspectIconPS => _iconPS;

        public Sprite GetIconForDevice(Core.Runtime.DeviceType device)
        {
            return device switch
            {
                Core.Runtime.DeviceType.PC => _iconPC,
                Core.Runtime.DeviceType.Xbox => _iconXbox,
                Core.Runtime.DeviceType.PlayStation => _iconPS,
                _ => _iconPC
            };
        }
        
        [SerializeField] private string _label = "Point d'inspection";
        [SerializeField] private Sprite _iconPC;
        [SerializeField] private Sprite _iconXbox;
        [SerializeField] private Sprite _iconPS;
        
    }
}

