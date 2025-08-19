using System.Collections.Generic;
using Damage.Runtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy.Runtime
{
    public class EnemyAI : MonoBehaviour
    {
       #region Public 
       
       [HideInInspector] public bool m_playerDetected;
       
       #endregion
       
       
       #region Unity Api

       private void Start()
       {
           _agent = GetComponent<NavMeshAgent>();
           _agent.updateRotation = true;
           if (_agent == null) Debug.LogError("Naw Mesh Agent is null");
           if (!_enemyIsRanged)
           {
               _enemySword = GetComponentInChildren<WeaponEnemyDamage>();
           }
       }
       
       private void Update()
       {
           IsPlayerDetected();

           if (m_playerDetected)
           {
               transform.LookAt(_hit);

               float distanceToPlayer = Vector3.Distance(transform.position, _hit);

               if (!_enemyIsRanged)
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
           
           Move();
           
       }
       
       #endregion
       
       
       #region Main Method

       private void Move()
       {
           if ( m_playerDetected || _target.Count == 0) return;
           
          
           if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
           {
               _currentTarget = (_currentTarget + 1) % _target.Count;
               _agent.SetDestination(_target[_currentTarget].position);
           }
       }
       
       private void IsPlayerDetected()
       {
           //Détection de tous les objets dans le rayon
           Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionDistance, LayerMask.GetMask("Player"));

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

                            if (_enemyIsRanged)
                            {
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

                            else
                            {
                                if (distance > _minAttackDistance) _agent.SetDestination(_hit);
                                else _agent.ResetPath();
                            }

                            _lostPlayerTimer = 0;
                            return;
                        }
                    }
               }
           }
           
           if (!m_playerDetected)
           {
               _lostPlayerTimer += Time.deltaTime;
               if (_lostPlayerTimer >= _lostPlayerDelay)
               {
                   m_playerDetected = false;
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
       
       #endregion
       
       
       #region Private

       [Header("Enemy Distant")]
       [SerializeField] private bool _enemyIsRanged;
       
       [Header("Attack Settings")]
       [SerializeField] private float _minAttackDistance = 3f;
       [SerializeField] private float _attackCooldown = 2f;
       private float _lastAttackTime;
       
       private NavMeshAgent _agent;
       [Header("Waypoints List")]
       [SerializeField] private List<Transform> _target;
       private int _currentTarget;
       
       [Header("Detected Player")]
       [SerializeField] private float _detectionDistance = 10f;
       [SerializeField] private float _detectionAngle = 45;
       private Vector3 _hit;
       [SerializeField] private float _lostPlayerTimer;
       [SerializeField] private float _lostPlayerDelay = 3 ;
       [SerializeField] private float _rotationSpeed = 5f;
       private WeaponEnemyDamage _enemySword;
       

       
       #endregion
    }
}
