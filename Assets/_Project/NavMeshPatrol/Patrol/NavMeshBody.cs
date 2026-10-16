using UnityEngine;
using UnityEngine.AI;
using System;

namespace NavMeshPatrol.Guard
{
    public class NavMeshBody : IGuardBody
    {
        private readonly NavMeshAgent _agent;


        public NavMeshBody(NavMeshAgent agent)
        {
            _agent = agent ? agent : throw new ArgumentNullException(nameof(agent));
        }

        public bool HasArrived =>
            !_agent.pathPending &&
            _agent.remainingDistance <= _agent.stoppingDistance + 0.1f &&
            (!_agent.hasPath || _agent.velocity.sqrMagnitude < 0.01f);

        public Vector3 CurrentPosition => _agent.transform.position;

        public void Advance(float deltaTime)
        {
           
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.isStopped = false;
            _agent.SetDestination(destination);
        }

        public void Stop()
        {
            if (_agent != null && _agent.isOnNavMesh)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
        }
    }
}

