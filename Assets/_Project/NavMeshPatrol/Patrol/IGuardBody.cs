using UnityEngine;

namespace NavMeshPatrol.Guard
{
    public interface IGuardBody
    {
        Vector3 CurrentPosition { get; }    
        bool HasArrived { get; }
        void MoveTo(Vector3 destination);

        void Advance(float deltaTime);

        void Stop();
    }

}
