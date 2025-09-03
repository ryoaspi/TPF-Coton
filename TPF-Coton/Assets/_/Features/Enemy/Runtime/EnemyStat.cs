using System;
using Object.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Runtime
{
    public class EnemyStat : MonoBehaviour
    {
        #region Public
        
        [HideInInspector] public bool m_isDeath;
        [HideInInspector] public int m_damage;
        [HideInInspector] public int m_currentHealth;
        [HideInInspector] public int m_block;
        [HideInInspector] public int m_maxHealth;
        public event Action OnDeath;
        public event Action<int> OnCotonLost;
        #endregion
        
        
        #region Unity Api

        private void Awake()
        {
            _origin = transform.position;
            _enemyAI = GetComponent<EnemyAI>();
            _enemyBig = GetComponent<EnemyBig>();
        }

        private void OnEnable()
        {
            _currentHealth = _health;
            m_currentHealth = _currentHealth;
            _renderer = GetComponent<Renderer>();
            m_damage = _Damage;
            m_block = _block;
            m_isDeath = false;
            transform.position = _origin;
            m_maxHealth = _health;

        }
        
        private void Update()
        {
            if (_renderer.material.color == Color.red)
            {
                _hits -= Time.deltaTime;
                if (_hits <= 0)
                {
                    _renderer.material.color = Color.gray;
                    _hits = 1f;
                }
            }
            
        }
        
        #endregion
        
        
        #region Utils

        public void DoDamage(int damage, Transform playerTransform)
        {
            int damageToApply = damage - _block;
            if (damageToApply <= 0) damageToApply = 0;
            
            damageToApply = Mathf.Min(damageToApply, _currentHealth);

            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Puffed)
            {
                _enemyBig.LoseCoton(damageToApply);
            }
            _currentHealth -= damageToApply;
            m_currentHealth = _currentHealth;
            
            Hit();
            DropCotonDamage(damageToApply);
            
            if (_enemyAI != null && playerTransform != null)
            {
                _enemyAI.OnHitByPlayer(playerTransform.position);
            }
            
            
            if (m_currentHealth <= 0)
            {
                Death();
            }
        }

        public void SetStat(int damage, int block, int newMaxHealth)
        {
			bool wasAtFullHealth = m_currentHealth == _health;
            
            m_damage = damage;
            m_block = block;
            m_maxHealth = newMaxHealth;

            if (wasAtFullHealth)
            {
                _currentHealth = m_maxHealth;
            }
            
            m_currentHealth = _currentHealth;
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;
            if (_currentHealth > _health)
            {
                _currentHealth = _health;
            }
            
            m_currentHealth = _currentHealth;
        }

        #endregion
        
        
        #region Main Method

        private void Hit()
        {
            _renderer.material.color = Color.red;
        }
        
        [ContextMenu("Death")]
        private void Death()
        {
           
            m_isDeath = true;
            
            OnDeath?.Invoke();
            
            gameObject.SetActive(false);
        }
        
        [ContextMenu("Damage")]
        private void DebugDamage() => DoDamage(1,null);
        
        public void DropCotonDamage(int damageToApply)
        {
            
            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Ranged || damageToApply <= 0) return;
            
            OnCotonLost?.Invoke(damageToApply);
            
            GameObject newCoton = Instantiate(_coton, transform.position, Quaternion.identity);
            
            Vector2 offset = Random.insideUnitCircle;
            offset.y = Mathf.Abs(offset.y);
                
            Vector3 targetPos = transform.position + new Vector3(offset.x,0,offset.y) * _distance;
                
            var contonComp = newCoton.GetComponent<Coton>();
            var lerpComp = newCoton.GetComponent<ParabolLerp>();

            contonComp.GetCotonValue(damageToApply);
                
            // Désactive la physique pendant le lerp
            contonComp.SetPhysicsActive(false);
                
            // Lance le lerp
            lerpComp.Lerp(transform.position, targetPos, _arcHeight, _arcDuration);;
                
            // Quand le Lerp est terminé, réactive la physique pour la chute naturel.
            lerpComp.OnLerpComplete += () => contonComp.SetPhysicsActive(true);
            
            // for (int i = 0; i < damageToApply; i++)
            // {
            //     GameObject newCoton = Instantiate(_coton, transform.position, Quaternion.identity);
            //
            //     Vector2 offset = Random.insideUnitCircle;
            //     offset.y = Mathf.Abs(offset.y);
            //     
            //     Vector3 targetPos = transform.position + new Vector3(offset.x,0,offset.y) * _distance;
            //     
            //     var contonComp = newCoton.GetComponent<Coton>();
            //     var lerpComp = newCoton.GetComponent<ParabolLerp>();
            //     
            //     // Désactive la physique pendant le lerp
            //     contonComp.SetPhysicsActive(false);
            //     
            //     // Lance le lerp
            //     lerpComp.Lerp(transform.position, targetPos, _arcHeight, _arcDuration);;
            //     
            //     // Quand le Lerp est terminé, réactive la physique pour la chute naturel.
            //     lerpComp.OnLerpComplete += () => contonComp.SetPhysicsActive(true);
            // }

            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Puffed)
            {
                _enemyBig.LoseCoton(damageToApply,true);
            }
        }
        
        #endregion
        
        
        #region Private And Protected
        
        [Header("Stats")]
        [SerializeField] private int _health = 5;
        [SerializeField] private int _block = 0;
        [SerializeField] private int _Damage = 1;
        [SerializeField] private float _hits = 1f;
        
        private Renderer _renderer;
        private int _blessing;
        private int _currentHealth;
        
        [Header("Loot")]
        [SerializeField] private GameObject _coton;

        [Header("Loot Comportment")] 
        [SerializeField] private float _distance = 1.5f;
        [SerializeField] private float _arcHeight = 2f;
        [SerializeField] private float _arcDuration = 1f;
        
        private Vector3 _origin;
        private EnemyAI _enemyAI;
        private EnemyBig _enemyBig;


        #endregion
    }
}
