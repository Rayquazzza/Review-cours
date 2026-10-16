using UnityEngine;
using System;

namespace NavMeshPatrol.Guard
{
    public class Guard
    {

        private readonly StateMachine _machine;

        public Guard(IGuardBody body, GuardSettings settings, string owner = "")
        {
            Body = body != null ? body : throw new ArgumentNullException(nameof(body));
            Settings = settings != null ? settings : throw new ArgumentNullException(nameof(settings));
            Idle = new IdleState(this);

            _machine = new StateMachine(owner);
            Idle = new IdleState(this);
            Patrol = new PatrolState(this);

            _machine.AddTransition(Idle, Patrol, () => true);

            _machine.ChangeState(Idle);

        }

        public IState CurrentState => _machine.Current;

        public IGuardBody Body { get; }
        public GuardSettings Settings { get; }

        public IdleState Idle {  get; }
        public PatrolState Patrol { get; }

        public void Tick(float deltaTime)
        {
            _machine.Tick(deltaTime);
        }
    }
}

