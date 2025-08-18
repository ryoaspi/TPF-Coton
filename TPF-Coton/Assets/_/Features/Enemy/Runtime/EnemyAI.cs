using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
       }
       
       private void Update()
       {
           IsPlayerDetected();
           
           if (m_playerDetected)
           {
               transform.LookAt(_hit);
               // _agent.SetDestination(_hit);
               return;
           }
           // Vector3 velocivty = _agent.velocity;
           // float speed = velocivty.magnitude;
           Move();
           
       }
       
       #endregion
       
       
       #region Main Method

       private void Move()
       {
           if ( m_playerDetected || _target.Count == 0) return;
           
           // Transform target = _target[_currentTarget];
           // float distance = Vector3.Distance(_target[_currentTarget].position, transform.position);
           
           // if (distance <= 0.5f)
           // {
           //     _currentTarget = (_currentTarget + 1) % _target.Count;
           //     target = _target[_currentTarget];
           //     _agent.SetDestination(target.position);
           // }
           
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

                            if (_EnemyDistant)
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
                            else _agent.SetDestination(_hit);

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
       [SerializeField] private bool _EnemyDistant;
       
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
       [SerializeField] private float _minAttackDistance = 3f;
       [SerializeField] private float _rotationSpeed = 5f;

       #endregion
    }
}
