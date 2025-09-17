using UnityEngine;

namespace Enemy.Runtime
{
    public class AnimationEvenRelayRange : MonoBehaviour
    {
        public EnemyShoot m_enemyShoot;
        
        public void TriggerShootingFromAnimation()
        {
            if (m_enemyShoot is not null)
            {
                m_enemyShoot.Shooting();
            }
        }
    }
}
