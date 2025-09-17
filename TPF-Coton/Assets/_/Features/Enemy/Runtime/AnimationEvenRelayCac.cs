using UnityEngine;

namespace Enemy.Runtime
{
    public class AnimationEvenRelayCac : MonoBehaviour
    {
        public WeaponEnemyDamage m_weaponEnemyDamage;

        public void DeactivateWeaponDamage()
        {
            if (m_weaponEnemyDamage is not null)
            {
                m_weaponEnemyDamage.DeactivateDamage();
            }
            else
            {
                Debug.LogWarning("WeaponEnemyDamage is not assigned");
            }
        }
    }
}
