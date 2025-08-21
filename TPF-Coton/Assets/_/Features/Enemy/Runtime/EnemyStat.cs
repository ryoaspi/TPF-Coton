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
        public event Action OnDeath;
        public event Action<int> OnCotonLost;
        #endregion
        
        
        #region Unity Api

        private void Start()
        {
            m_damage = _Damage;
        }

        private void OnEnable()
        {
            _currentHealth = _health;
            _renderer = GetComponent<Renderer>();
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

        public void DoDamage(int damage)
        {
            int damageToApply = damage - _block;
            _currentHealth -= damageToApply;
            
            Hit();
            DropCotonDamage(damageToApply);
            
            if (_currentHealth <= 0)
            {
                Death();
            }
            
        }

        public void SetStat(int damage, int block, int health)
        {
            m_damage = damage;
            m_block = block;
            _currentHealth = health;
            m_currentHealth = _currentHealth;
            
            //Si on est en pleine form (buff avant dégâts), augmente les PV max
            if (_currentHealth >= _health)
            {
                _health = _currentHealth;
            }
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
            // if (m_isDeath) return;
            
            m_isDeath = true;
            
            OnDeath?.Invoke();
            
            gameObject.SetActive(false);
        }
        
        [ContextMenu("Damage")]
        private void DebugDamage() => DoDamage(1);
        
        private void DropCotonDamage(int damageToApply)
        {
            if (damageToApply <= 0) return;
            
            OnCotonLost?.Invoke(damageToApply);
            
            for (int i = 0; i < damageToApply; i++)
            {
                GameObject newCoton = Instantiate(_coton, transform.position, Quaternion.identity);

                Vector2 offset = Random.insideUnitCircle;
                offset.y = Mathf.Abs(offset.y);
                
                Vector3 targetPos = transform.position + new Vector3(offset.x,0,offset.y) * _distance;
                
                var contonComp = newCoton.GetComponent<Coton>();
                var lerpComp = newCoton.GetComponent<ParabolLerp>();
                
                // Désactive la physique pendant le lerp
                contonComp.SetPhysicsActive(false);
                
                // Lance le lerp
                lerpComp.Lerp(transform.position, targetPos, _arcHeight, _arcDuration);;
                
                // Quand le Lerp est terminé, réactive la physique pour la chute naturell.
                lerpComp.OnLerpComplete += () => contonComp.SetPhysicsActive(true);
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

        [Header("Loot Comportement")] 
        [SerializeField] private float _distance = 1.5f;
        [SerializeField] private float _arcHeight = 2f;
        [SerializeField] private float _arcDuration = 1f;
        



        #endregion
    }
}
