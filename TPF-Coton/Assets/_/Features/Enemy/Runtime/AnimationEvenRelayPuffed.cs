using UnityEngine;

namespace Enemy.Runtime
{
    public class AnimationEvenRelayPuffed : MonoBehaviour
    {
        public EnemyAI m_enemyAi;

        public void EndEat()
        {
            m_enemyAi.EndEating();
        }
    }
}
