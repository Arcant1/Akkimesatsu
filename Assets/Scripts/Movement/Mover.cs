using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private NavMeshAgent agent;
        private Animator animator;
        private Health health;
        private float maxSpeed = 5.33f;
        [SerializeField] float maxNavPathLength = 40f;

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            if (!NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navMeshPath)) return false;
            if (navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(navMeshPath) > maxNavPathLength) return false;
            return true;
        }
        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            animator = GetComponent<Animator>();
        }
        void Update()
        {
            if (health.IsDead()) return;
            agent.enabled = !health.IsDead();
            HandleAnimation();
        }
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);  // only move
            MoveTo(destination, speedFraction);
        }
        public void Cancel()
        {
            agent.isStopped = true;
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            agent.isStopped = false;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.destination = destination;
        }
        public void HandleAnimation()
        {
            animator.SetFloat(
                "forwardSpeed",
                transform.InverseTransformDirection(agent.velocity).z);
        }
        public object CaptureState()
        {
            MoverSaveData saveData = new MoverSaveData();
            saveData.position = new SerializableVector3(transform.position);
            saveData.rotation = new SerializableVector3(transform.eulerAngles);
            return saveData;
        }
        public void RestoreState(object state)
        {
            MoverSaveData saveData = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = saveData.position.ToVector();
            transform.eulerAngles = saveData.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
        [System.Serializable]
        struct MoverSaveData
        {
            internal SerializableVector3 position;
            internal SerializableVector3 rotation;
        }
        private float GetPathLength(NavMeshPath path)
        {
            float sum = 0f;
            if (path.corners.Length < 2) return sum;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                sum += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return sum;
        }
    }
}
