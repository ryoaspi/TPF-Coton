using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Runtime
{
    public class EnemyAI : MonoBehaviour
    {
        #region Public

        [HideInInspector] public bool m_playerDetected;
        [HideInInspector] public bool m_isCotonDetected;
        [Header("Enemy Type")] 
        public EnemyType m_enemyType;

        public enum EnemyType
        {
            Melee,
            Ranged,
            Puffed
        }
        

        #endregion


        #region Unity Api

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            if (_agent == null) Debug.LogError("Naw Mesh Agent is null");
            _agent.updateRotation = true;
            _enemySword = GetComponentInChildren<WeaponEnemyDamage>();
            _enemyShoot = GetComponentInChildren<EnemyShoot>();
            _enemyBig = GetComponent<EnemyBig>();
            
            _canInitOnEnable = true;
        }

        private void OnEnable()
        {
            if (_canInitOnEnable) _agent.SetDestination(_target[1].position);
        }

        private void FixedUpdate()
        {
            IsPlayerDetected();
            
            
            if (m_enemyType == EnemyType.Puffed)
            {
                
                if (_isCollectingCoton)
                {
                    //Si on attend entre deux cotons, on ne bouge pas
                    if (_isWaitingToCollectNext) return;
                    
                    //Si on n'a plus rien à collecter, on arrête
                    if (_cotonQueue.Count == 0)
                    {
                        _isCollectingCoton = false;
                    }

                    // Aller au prochain coton
                    else
                    {
                        Vector3 nextCotonPosition = _cotonQueue.Dequeue();
                        _hit = nextCotonPosition;
                        _agent.SetDestination(_hit);
                        _isWaitingToCollectNext = true;
                    
                        //Attendre un petit temps avant de lancer la suite
                        Invoke(nameof(ContinueCotonCollect), _timeBetweenCotonCollects);
                        return;
                    }

                }
                
                // Si pas déjà en collecte, tenter d'en détecter
				IsCotonDetected(LayerMask.NameToLayer("Coton"));
                if (_cotonQueue.Count > 0)
                {
                    _isCollectingCoton = true;
                    return;
                }
                
                //Si pas de coton, on va vers les ennemies
                IsEnemyDetected();
                if (_enemyIsDetected)
                {
                    HandleCombat();
                    return;
                }
            }

            if (m_playerDetected)
            {
                HandleCombat();

                transform.LookAt(_hit);

                float distanceToPlayer = Vector3.Distance(transform.position, _hit);
                
                if (m_enemyType == EnemyType.Melee && _enemySword is not null)
                {
                    //Melee Logic
                    if (!_enemySword.m_isAttacking && distanceToPlayer <= _minAttackDistance &&
                        Time.time >= _lastAttackTime + _attackCooldown)
                    {
                        _enemySword.m_isAttacking = true;
                        _lastAttackTime = Time.time;
                    }
                
                    _enemySword.Attack();
                }

                return;
            }

            if (_isSearching)
            {
                SearchAtLastKnownPosition();
                return;
            }

            Patrol();

        }

        #endregion
        
        
        #region Utils

        public void ResetCotonCollection()
        {
            _cotonQueue.Clear();
            _isCollectingCoton = false;
            _isWaitingToCollectNext = false;
        }

        public void OnHitByPlayer(Vector3 playerPosition)
        {
            m_playerDetected = true;
            _hit = playerPosition;
            _lastKnowPlayerPosition = _hit;
            
            //Reset Timer to prevent search mode
            _isSearching = false;
            _lostPlayerTimer = 0;
            _hasSeenPlayer = true;
            _searchTimer = 0;
            _globalSearchTimer = 0;
            
            //Tourne immédiatement vers le joueur
            Vector3 direction = (_hit - transform.position).normalized;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
            
            //va vers le joueur si nécessaire
            float distanceToPlayer = Vector3.Distance(transform.position, _hit);
            if (distanceToPlayer > _minAttackDistance)
            {
                _agent.SetDestination(_hit);
            }
            else
            {
                _agent.ResetPath();
            }
        }

        public void OnHitByCoton(Vector3 cotonPosition)
        {
            _lastCotonPosition = cotonPosition;
            _lastCotonHitTime = Time.time;
            m_isCotonDetected = true;
            
            // Priorité directe vers coton
            _hit = cotonPosition;
            _agent.SetDestination(_hit);
            
            //Reset recherche Joueur
            _isSearching = false;
            _lostPlayerTimer = 0;
        }
        
        public void PlayerDetected() => IsPlayerDetected();

        public void IsCotonDetected(int cotonLayer)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionDistance, LayerMask.GetMask("Coton"));
            
            _cotonQueue.Clear();
            m_isCotonDetected = false;
            
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Coton") && collider.gameObject.activeInHierarchy)
                {
                    Vector3 direction = (collider.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(collider.transform.position, transform.position);
                    float angle = Vector3.Angle(transform.forward, direction);

                    if (angle <= _detectionAngle / 2f)
                    {
                        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
                        if (!Physics.Raycast(raycastOrigin, direction, distance, LayerMask.GetMask("Default")))
                        {
                            _cotonQueue.Enqueue(collider.transform.position);
                            m_isCotonDetected = true;
                            
                        }
                    }
                }
            }
        }

        public void IsEnemyDetected()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionDistance, LayerMask.GetMask("Enemy"));

            _enemyIsDetected = false;
            
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy") && collider.gameObject.activeInHierarchy)
                {
                    Vector3 direction = (collider.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(collider.transform.position, transform.position);
                    float angle = Vector3.Angle(transform.forward, direction);

                    if (angle <= _detectionAngle / 2f)
                    {
                        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
                        if (!Physics.Raycast(raycastOrigin, direction, distance, LayerMask.GetMask("Default")))
                        {
                            _hit = collider.transform.position;
                            _enemyIsDetected = true;
                            
                            // Ne se rapproche pas si trop proche
                            if (distance > _minAttackDistance)
                            {
                                _agent.SetDestination(_hit);
                            }
                            else
                            {
                                _agent.ResetPath();
                            }
                        }
                    }
                }
            }
        }
        
        #endregion


        #region Main Method

        private void ContinueCotonCollect()
        {
            _isWaitingToCollectNext = false;
        }

        private void IsPlayerDetected()
        {

            if (_isSearching)
            {
                m_playerDetected = false;
                // return;
            }
            //Détection de tous les objets dans le rayon
            Collider[] colliders =
                Physics.OverlapSphere(transform.position, _detectionDistance, LayerMask.GetMask("Player"));
            
            m_playerDetected = false;

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Vector3 direction = (collider.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(collider.transform.position, transform.position);
                    float angle = Vector3.Angle(transform.forward, direction);

                    if (angle <= _detectionAngle / 2f)
                    {
                        Vector3 raycastOrigin = transform.position + Vector3.up * 0.5f;
                        if (!Physics.Raycast(raycastOrigin, direction, distance, LayerMask.GetMask("Default")))
                        {
                            m_playerDetected = true;
                            _hit = collider.transform.position;
                            _lastKnowPlayerPosition = _hit;
                            
                            //reset importants
                            _isSearching = false;
                            _lostPlayerTimer = 0;
                            _hasSeenPlayer = true;
                            _searchTimer = 0f;
                            _globalSearchTimer = 0f;

                            // Ne se rapproche pas si trop proche
                            if (distance > _minAttackDistance)
                            {
                                _agent.SetDestination(_hit);
                            }
                            else
                            {
                                _agent.ResetPath();
                            }


                            return;
                        }
                    }
                }
            }
            
            // Plus de vision directe : gérer la perte
            _lostPlayerTimer += Time.deltaTime;

            if (_hasSeenPlayer)
            {
                if (!_isSearching) _agent.SetDestination(_lastKnowPlayerPosition);
            }

            if (_lostPlayerTimer >= _lostPlayerDelay)
            {
                m_playerDetected = false;

                if (_hasSeenPlayer)
                {
                    _isSearching = true;
                    _searchTimer = 0;
                    _globalSearchTimer = 0;
                    _agent.ResetPath();
                    _lostPlayerTimer = 0;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionDistance);

            Vector3 left = Quaternion.Euler(0, _detectionAngle / 2f, 0) * transform.forward;
            Vector3 right = Quaternion.Euler(0, -_detectionAngle / 2f, 0) * transform.forward;

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, left * _detectionDistance);
            Gizmos.DrawRay(transform.position, right * _detectionDistance);
        }

        private void SearchAtLastKnownPosition()
        {
            
            _globalSearchTimer += Time.deltaTime;
            
            //FailSafe : ne jamais chercher au-delà de _maxSearchTime
            if (_globalSearchTimer >= _maxsearchTimer)
            {
                EndSearch();
            }
            
            // si pas de chemin en cours, essaye d'y aller
            if (!_agent.hasPath && !_agent.pathPending)
            {
                _agent.SetDestination(_lastKnowPlayerPosition);
                return;
            }
            
            //Si chemin invalide/partiel
            bool pathbad = _agent.pathStatus == NavMeshPathStatus.PathInvalid || _agent.pathStatus == NavMeshPathStatus.PathPartial;
            
            bool arrived = !_agent.pathPending && (_agent.remainingDistance <= Mathf.Max(_agent.stoppingDistance, _arrivalEpsilon));

            if (pathbad || arrived)
            {
                _agent.ResetPath();
                
                transform.Rotate(Vector3.up * (45f * Time.deltaTime));
                
                _searchTimer += Time.deltaTime;
                if (_searchTimer >= _searchDuration)
                {
                    EndSearch();
                }
            }

        }

        private void EndSearch()
        {
            _searchTimer = 0;
            _globalSearchTimer = 0;
            _isSearching = false;
            _hasSeenPlayer = false;
            _lostPlayerTimer = 0;
            _agent.ResetPath();
        }
        private void HandleCombat()
        {
            // Rotation vers le joueur
            Vector3 direction = (_hit - transform.position).normalized;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationSpeed * Time.deltaTime);

            float distanceToPlayer = Vector3.Distance(transform.position, _hit);

            switch (m_enemyType)
            {
                case EnemyType.Melee:
                    if (distanceToPlayer > _minAttackDistance) _agent.SetDestination(_hit);

                    else
                    {
                        _agent.ResetPath();

                        if (_enemySword is null)
                        {
                            break;
                        }
                        if (!_enemySword.m_isAttacking && Time.time >= _lastAttackTime + _attackCooldown)
                        {
                            _enemySword.m_isAttacking = true;
                            _lastAttackTime = Time.time;
                            _enemySword.Attack();
                        }
                    }
                    break;
                
                case EnemyType.Ranged:
                    if (distanceToPlayer > _minAttackDistance)
                    {
                        _agent.SetDestination(_hit);
                    
                    }
                    else
                    {
                        _agent.ResetPath();
                        if (_enemyShoot is not null && Time.time >= _lastAttackTime + _attackCooldown)
                        {
                            _lastAttackTime = Time.time;
                            _enemyShoot.Shooting();
                        }
                    }
                    break;
                
                case EnemyType.Puffed:
                    if (distanceToPlayer > _minAttackDistance)
                    {
                        _agent.SetDestination(_hit);
                    }
                    else
                    {
                        _agent.ResetPath();
                        if (Time.time >= _lastAttackTime + _attackCooldown)
                        {
                            _lastAttackTime = Time.time;
                            _enemyBig.Fight();
                        }
                    }
                    break;
            }

        }

        private void Patrol()
        {
            if (m_playerDetected || _target.Count == 0) return;


            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _currentTarget = (_currentTarget + 1) % _target.Count;
                _agent.SetDestination(_target[_currentTarget].position);
            }
        }

        #endregion


        #region Private



        [Header("Attack Settings")]
        [SerializeField] private float _minAttackDistance = 3f;
        [SerializeField] private float _attackCooldown = 2f;
        private WeaponEnemyDamage _enemySword;
        private float _lastAttackTime;

        private NavMeshAgent _agent;

        [Header("Waypoints List")] [SerializeField]
        private List<Transform> _target;
        
        private int _currentTarget;

        [Header("Detected Player")] [SerializeField]
        private float _detectionDistance = 10f;

        [SerializeField] private float _detectionAngle = 45;
        private Vector3 _hit;
        private float _lostPlayerTimer;
        [SerializeField] private float _lostPlayerDelay = 3;
        [SerializeField] private float _rotationSpeed = 5f;
        

        [SerializeField] private float _searchDuration = 3f;
        [SerializeField] private float _maxsearchTimer = 8f;
        [SerializeField] private float _arrivalEpsilon = 0.2f;
        private float _globalSearchTimer;
        
        private bool _isSearching;
        private float _searchTimer;
        private Vector3 _lastKnowPlayerPosition;
        private bool _hasSeenPlayer;
        private EnemyShoot _enemyShoot;
        
        private bool _canInitOnEnable;
        
        [Header("Hit Coton")]
        private Vector3? _lastCotonPosition;
        private float _lastCotonHitTime = 0f;
        private Queue<Vector3> _cotonQueue = new ();
		private bool _isCollectingCoton;
        private bool _isWaitingToCollectNext;
        private float _timeBetweenCotonCollects = 1f;

        private EnemyBig _enemyBig;
        private bool _enemyIsDetected;

        #endregion
    }
}
