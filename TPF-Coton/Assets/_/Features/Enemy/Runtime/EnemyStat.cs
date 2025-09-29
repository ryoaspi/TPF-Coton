using System;
using System.Collections.Generic;
using Object.Runtime;
using Sound.Runtime;
using UnityEngine;
using UnityEngine.AI;
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
            _agent = GetComponent<NavMeshAgent>();
            
            _renderers =  GetComponentsInChildren<Renderer>();
            _originalColors.Clear();

            foreach (var renderer in _renderers)
            {
                var colors = new Color[renderer.materials.Length];
                for (var i = 0; i < renderer.materials.Length; i++)
                {
                    Material mat = renderer.materials[i];
                    if (mat.HasProperty("_Color")) colors[i] = renderer.materials[i].color;
                }
                _originalColors.Add(colors);
            }
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
            if (_isFlashing)
            {
                _flashTimer -= Time.deltaTime;
                if (_flashTimer <= 0)
                {
                    ResetColot();
                    _isFlashing = false;
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

            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Puffed && _enemyBig is not null)
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
                _agent.speed = 0;
                ParticulSystem();
                //Stop all behaviors
                DisableBehaviourOnDeath();
                _soundEvent.PlaySoundEventScript("Death", (() => {Death();}));
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

        public void Kill()
        {
            if (m_isDeath) return;
            Death();
        }

        #endregion
        
        
        #region Main Method

        private void Hit()
        {
            SetColot(Color.red);
            _isFlashing = true;
            _flashTimer = _hits;
            
            _soundEvent.PlaySoundEvent("hit");

        }
        
        [ContextMenu("Death")]
        private void Death()
        {
           
            m_isDeath = true;

            if (_agent != null && _agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                _agent.speed = 0;
                _agent.isStopped = true;
            }
            
            OnDeath?.Invoke();
            
            gameObject.SetActive(false);
        }

        private void ParticulSystem()
        {
            if (_CloudDeath is not null)
            {
                ParticleSystem psInstance = Instantiate(_CloudDeath, transform.position, Quaternion.identity);
                psInstance.Play();
                
                //détruit automatiquement l'instance après sa durée + lifetime max
                Destroy(psInstance.gameObject, psInstance.main.duration + psInstance.main.startLifetime.constantMax);
            }
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

            if (_enemyAI.m_enemyType == EnemyAI.EnemyType.Puffed)
            {
                _enemyBig.LoseCoton(damageToApply,true);
            }
        }

        private void SetColot(Color color)
        {
            foreach (var renderer in _renderers)
            {
                foreach (var material in renderer.materials)
                {
                    material.color = color;
                }
            }
        }

        private void ResetColot()
        {
            for (int r = 0; r < _renderers.Length; r++)
            {
                Renderer renderer = _renderers[r];
                Color[] savedColors = _originalColors[r];

                for (int c = 0; c < renderer.materials.Length; c++)
                {
                    if (c < savedColors.Length)
                    {
                        renderer.materials[c].color = savedColors[c];
                    }
                }
            }
        }

        private void DisableBehaviourOnDeath()
        {
            MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();
            foreach (var behaviour in behaviours)
            {
                
                if (behaviour != _soundEvent && behaviour != _audio)
                {
                    behaviour.enabled = false;
                }
            }

            foreach (Transform child in transform)
            {
                foreach (var childcom in child.GetComponentsInChildren<MonoBehaviour>())
                {
                    childcom.enabled = false;
                }
            }
            
            Animator[] animators = GetComponents<Animator>();
            foreach (var animator in animators)
            {
                animator.enabled = false;
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
        
        private Renderer[] _renderers;
        private List<Color[]> _originalColors = new ();
        private bool _isFlashing;
        private float _flashTimer;
        private NavMeshAgent _agent;
        
        [Header("Audio")]
        [SerializeField] private SoundEvent _soundEvent;
        [SerializeField] private AudioSource _audio;
        
        [Header("VFX")]
        [SerializeField] private ParticleSystem _CloudDeath;

        #endregion
    }
}
