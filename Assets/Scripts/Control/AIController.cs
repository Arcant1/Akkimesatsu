using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Utils;
using System;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour, ISaveable
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float dwellTime = 1f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float waypointTolerance = 0.5f;
        [SerializeField] float alertCooldownTime = 4f;
        [SerializeField] float alertRadius = 5f;

        private GameObject player;
        private Mover mover;
        private Fighter fighter;
        private Health health;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArriveAtWaypoint = Mathf.Infinity;
        private float timeSinceAlerted = Mathf.Infinity;
        private LazyValue<Vector3> guardLocation;
        private int currentWaypointIndex = 0;
        [Range(0f, 1f)]
        private float patrolSpeedFraction = 0.2f;

        private void Awake()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player");
            guardLocation = new LazyValue<Vector3>(InitializeGuardLocation);
        }

        private Vector3 InitializeGuardLocation()
        {
            return transform.position;
        }

        private void Start()
        {
            guardLocation.ForceInit();
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
        private void Update()
        {
            if (health.IsDead()) return;
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        public void Alert()
        {
            timeSinceAlerted = 0f;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveAtWaypoint += Time.deltaTime;
            timeSinceAlerted += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardLocation.value;
            if (patrolPath)
            {
                if (AtWaypoint())
                {
                    timeSinceArriveAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArriveAtWaypoint > dwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
            AlertNearbyEnemies();
        }

        private void AlertNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, alertRadius, Vector3.forward, 0f);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.transform.GetComponent<AIController>();
                ai?.Alert();
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAlerted < alertCooldownTime;
        }

        public object CaptureState()
        {
            return timeSinceAlerted;
        }

        public void RestoreState(object state)
        {
            timeSinceAlerted = (float)state;
        }
    }
}
