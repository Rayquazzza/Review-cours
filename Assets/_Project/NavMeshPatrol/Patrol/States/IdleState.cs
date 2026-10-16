using NavMeshPatrol.Guard;
using UnityEngine;


namespace NavMeshPatrol.Guard
{
    public class IdleState : IState
    {
        private readonly Guard _guard;

        public IdleState(Guard guard)
        {
            _guard = guard;
        }

        public void Enter()
        {

        }

        public void Exit()
        {

        }

        public void Tick(float deltaTime)
        {

        }
    }
}

