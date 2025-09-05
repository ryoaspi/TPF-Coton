using UnityEngine;
using Knockback.Runtime;
using UnityEngine.AI;
namespace Enemy.Runtime
{
    public class KnockBackReceive : MonoBehaviour, IKnockbackReceiver
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float knockbackDuration = 0.3f;
        private Vector3 knockbackVelocity;
        private float knockbackTimer;

        private void Awake()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();
        }

        public void ReceiveKnockback(Vector3 direction, float force)
        {
            if (agent == null) return;

            agent.isStopped = true;
            knockbackVelocity = direction.normalized * force;
            knockbackTimer = knockbackDuration;
        }

        private void Update()
        {
            if (knockbackTimer > 0f)
            {
                // ✅ Utilisation de agent.Move() pour respecter les collisions
                agent.Move(knockbackVelocity * Time.deltaTime);

                knockbackTimer -= Time.deltaTime;

                if (knockbackTimer <= 0f)
                {
                    agent.isStopped = false;
                }
            }
        }
    }
}