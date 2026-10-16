using Codice.CM.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace NavMeshPatrol.Guard
{
    public class SimulatedBody : IGuardBody
    {
        public Vector3 CurrentPosition { get; private set; }
        private Vector3 _destination;
        private float _speed = 3f;
        public bool HasArrived => CurrentPosition == _destination;

        public void MoveTo(Vector3 destination)
        {
            _destination = destination;
        }

        public void Stop()
        {
            _destination = CurrentPosition;
        }

        public void Advance(float deltaTime)
        {
            CurrentPosition = Vector3.MoveTowards(CurrentPosition, _destination, _speed * deltaTime);
        }
    }
}

