using RPG.Stats;

using UnityEngine;
using UnityEngine.AI;
namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far can the pickups be scattered from the dropper")]
        [SerializeField] float scatterDistance = 1;
        const uint ATTEMPTS = 30;
#pragma warning disable CS0649
        [SerializeField] DropLibrary dropLibrary;
#pragma warning restore CS0649
        public void RandomDrop()
        {
            var baseStats = GetComponent<BaseStats>();
            var drops = dropLibrary.GetRandomDrops(baseStats.GetLevel());
            foreach (var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }
        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
                if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * scatterDistance, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
                    return hit.position;
            return transform.position;
        }
    }
}
