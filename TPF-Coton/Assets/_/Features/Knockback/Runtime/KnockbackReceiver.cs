using UnityEngine;

namespace Knockback.Runtime
{
    public interface IKnockbackReceiver 
    {
        void ReceiveKnockback(UnityEngine.Vector3 direction, float force);
    }
}
