using System;
using System.Collections.Generic;
using UnityEngine;

namespace NavMeshPatrol.Guard
{
    public class PatrolState : IState
    {
        private readonly Guard _guard;
        private readonly List<Vector3> _points;
        private int _currentIndex = 0;

        public HashSet<Vector3> ReachedPositions { get; private set; }

        public PatrolState(Guard guard)
        {
            _guard = guard ?? throw new ArgumentNullException(nameof(guard));
            _points = guard.Settings?.PatrolPoints ?? throw new ArgumentNullException(nameof(guard.Settings.PatrolPoints));

            if (_points.Count == 0)
                Debug.LogWarning("La liste des points de patrouille est vide !");
            ReachedPositions = new HashSet<Vector3>();
        }

        public void Enter()
        {
            if (_points.Count > 0)
            {
                _currentIndex = 0;
                MoveToCurrentPoint();
            }
        }

        public void Exit()
        {
            _guard.Body.Stop();
        }

        public void Tick(float deltaTime)
        {
            if (_points.Count == 0) return;

            if (_guard.Body.HasArrived)
            {
                ReachedPositions.Add(_guard.Body.CurrentPosition);
                _guard.Body.Stop();

                _currentIndex = (_currentIndex + 1) % _points.Count;

                MoveToCurrentPoint();
            }
        }

        private void MoveToCurrentPoint()
        {
            _guard.Body.MoveTo(_points[_currentIndex]);
        }
    }
}