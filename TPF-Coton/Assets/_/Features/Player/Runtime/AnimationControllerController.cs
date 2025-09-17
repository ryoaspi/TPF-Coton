using UnityEngine;

namespace Player.Runtime
{
    public class AnimationControllerController : MonoBehaviour
    {
        private void Awake()
        {
            // On récupère toutes les refs au démarrage
            _playerMovement = GetComponent<PlayerMovement>();
            _shield = GetComponent<Shield>();
            _playerDamage = GetComponent<PlayerDamage>();
        }

        private void Update()
        {
            // Walk
            _animator.SetBool("IsWalking", _playerMovement.m_isWalking);

            // Shield
            _animator.SetBool("IsShielding", _shield.m_isShielding);

            // Attack
            _animator.SetBool("IsAttacking", _playerDamage.m_isAttacking);
        }

        #region private

        [SerializeField] private Animator _animator;

        private PlayerMovement _playerMovement;
        private PlayerDamage _playerDamage;
        private Shield _shield;

        #endregion
    }
}