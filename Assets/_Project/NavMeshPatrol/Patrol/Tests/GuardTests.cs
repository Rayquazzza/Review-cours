using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace NavMeshPatrol.Guard.Tests
{
    public class GuardTests
    {
        private const float Step = 0.25f;

        private Guard _guard;
        private GuardSettings _settings;
        private IGuardBody _body;

        [SetUp]
        public void SetUp()
        {
            _settings = new GuardSettings();
            _body = new SimulatedBody();
            _guard = new Guard(_body, _settings);
        }

        private void Run(float seconds)
        {
            int steps = Mathf.RoundToInt(seconds / Step);
            for (int i = 0; i < steps; i++) Tick();
        }

        private void RunUntil<TState>(float timeoutSeconds = 30f) where TState : IState
        {
            for (float elapsed = 0f; elapsed < timeoutSeconds; elapsed += Step)
            {
                if (_guard.CurrentState is TState) return;
                Tick();
            }
            Assert.Fail($"The worker never reached {typeof(TState).Name}; it is in {_guard.CurrentState?.GetType().Name}.");
        }

        private void Tick()
        {
            _body.Advance(Step);
            _guard.Tick(Step);
        }

        [Test]
        public void NewGuard_IsIdleAtStart()
        {
            Assert.That(_guard.CurrentState, Is.InstanceOf<IdleState>());
        }

        [Test]
        public void GuardReachesPoints_AtLeastOneTime()
        {
            _settings.PatrolPoints.Add(new Vector3(0, 0, 0));
            _settings.PatrolPoints.Add(new Vector3(10, 0, 0));
            _settings.PatrolPoints.Add(new Vector3(10, 0, 10));

            _guard = new Guard(_body, _settings);

            Run(8.0f);

            var patrolState = _guard.Patrol;

            Assert.That(patrolState.ReachedPositions, Is.Not.Empty, "Le gardien devrait avoir atteint au moins un point.");

            Assert.That(patrolState.ReachedPositions, Does.Contain(new Vector3(10, 0, 0)).Using(Vector3EqualityComparer.Instance),
                "Le gardien aurait dû atteindre le point (10, 0, 0).");
        }
    }
}

