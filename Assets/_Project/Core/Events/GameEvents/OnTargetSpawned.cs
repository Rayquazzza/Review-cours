using UnityEngine;

namespace Core.Events
{
    public struct OnTargetSpawned
    {
        static OnTargetSpawned e;

        public string Name;
        public Vector3 Position;

        public static void Trigger(string name, Vector3 position)
        {
            e.Position = position;
            e.Name = name;
            EventBus.Publish(e);
        }
    }
}


