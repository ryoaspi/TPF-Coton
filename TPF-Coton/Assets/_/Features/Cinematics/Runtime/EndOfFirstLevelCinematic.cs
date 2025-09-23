using System.Collections.Generic;
using Enemy.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Cinematics.Runtime
{
    public class EndOfFirstLevelCinematic : MonoBehaviour
    {
        void Start()
        {
            _cameraChanger=GetComponent<CameraChanger>();
            _deadCount = 0;

            // Récupère tous les NavMeshAgents actifs dans la scène
            NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
            _allAgents.AddRange(agents);

            // Lier l'événement OnDeath pour chaque ennemi de la liste
            foreach (var enemy in _enemies)
            {
                if (enemy == null) continue;

                EnemyStat enemyStat = enemy.GetComponentInChildren<EnemyStat>();
                if (enemyStat != null) 
                {
                    enemyStat.OnDeath += OnEnemyDeath;
                }
            }
        }

        void Update()
        {
            if (_isCinematicActive)
            {
                _cinematicTimer += Time.deltaTime;
                if (_cinematicTimer >= _cinematicDuration)
                {
                    EndCinematic();
                }
            }
        }

        private void OnEnemyDeath()
        {
            _deadCount++;

            if (_deadCount == _enemies.Count && _enemies.Count > 0)
            {
                if (_cameraChanger != null)
                    StartCinematic();
            }
        }

        private void StartCinematic()
        {
            _cameraChanger.StartCinematic(_cinematicIndex);

            // Stop tous les NavMeshAgents
            foreach (var agent in _allAgents)
            {
                if (agent != null) agent.isStopped = true;
            }

            _cinematicTimer = 0f;
            _isCinematicActive = true;
        }

        private void EndCinematic()
        {
            _cameraChanger.StopCinematic(_cinematicIndex);

            // Reprendre tous les NavMeshAgents
            foreach (var agent in _allAgents)
            {
                if (agent != null) agent.isStopped = false;
            }

            _isCinematicActive = false;
        }

        [Header("Enemies")]
        [SerializeField] private List<GameObject> _enemies;
        private int _deadCount;

        [Header("Cinematic")]
        private CameraChanger _cameraChanger;
        [SerializeField] private int _cinematicIndex = 0;
        [SerializeField] private float _cinematicDuration = 3f;

        private bool _isCinematicActive = false;
        private float _cinematicTimer = 0f;

        private List<NavMeshAgent> _allAgents = new List<NavMeshAgent>();
    }
}
