using UnityEngine;
using UnityEngine.AI;

namespace NavMeshPatrol.Guard
{
    public class GuardController : MonoBehaviour
    {
        [SerializeField] private GuardSettings _settings = new GuardSettings();

        public Guard Guard { get; private set; }

        private IGuardBody _body; 


        private void Start()
        {
            _body = new NavMeshBody(GetComponent<NavMeshAgent>());
            Guard = new Guard(_body, _settings,name);

        }

        private void Update()
        {
            Guard.Tick(Time.deltaTime);
        }


    }
}

