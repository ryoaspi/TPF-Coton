using System;
using UnityEngine;
using Knockback.Runtime;

namespace Player.Runtime
{
    public class KnockBackApply : MonoBehaviour
    {
        [SerializeField] private float knockbackForce = 10f;
        [SerializeField] private Transform player; // ref au joueur

        private void Start()
        {
            
            player=transform.root;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IKnockbackReceiver  knockbackable))
            {
                Vector3 dir = (other.transform.position - player.position).normalized;
                knockbackable.ReceiveKnockback(dir, knockbackForce);
            }
        }
    }
}
