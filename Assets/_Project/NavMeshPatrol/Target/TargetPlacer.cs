using UnityEngine;

namespace NavMeshPatrol.Targets
{
    public class TargetPlacer : MonoBehaviour
    {
        [SerializeField] private Target _target;

        [SerializeField] private Vector3 _spawnPosition;

        [ContextMenu("Spawn Target")]
        public void Spawn()
        {
            Instantiate(_target, _spawnPosition, Quaternion.identity);
        }
    }
}

