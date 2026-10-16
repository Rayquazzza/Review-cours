using UnityEngine;
using Core.Events;

namespace NavMeshPatrol.Targets
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private string _nameTarget;
        private void OnEnable()
        {
            OnTargetSpawned.Trigger(_nameTarget,transform.position);
        }
    }

}
