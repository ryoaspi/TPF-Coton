using UnityEngine;

namespace Object.Runtime
{
    public class InspectableObject : MonoBehaviour
    {
        [HideInInspector] public string InspectLabel => _label;
        [HideInInspector] public Sprite InspectIcon => _icon;
        
        
        [SerializeField] private string _label;
        [SerializeField] private Sprite _icon;
    }
}
