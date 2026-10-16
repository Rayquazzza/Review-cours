using UnityEngine;

namespace NavMeshPatrol.Obstacles
{
    public class ObstaclePlacer : MonoBehaviour
    {
        [SerializeField] private GameObject _obstaclePrefab;

        [SerializeField] private Vector3 _spawnPoint;


        [ContextMenu("Spawn Obstacle")]
        void Spawn()
        {
            Instantiate(_obstaclePrefab, _spawnPoint, Quaternion.identity);
        }
    }

}

