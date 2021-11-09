using System;
using System.Collections.Generic;

using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("Akki Messatsu/Items/DropLibrary"))]
    public class DropLibrary : ScriptableObject
    {
        [SerializeField] DropConfig[] potentialDrops;
        [SerializeField] float[] dropChancePercentage;
        [SerializeField] int[] minDrops;
        [SerializeField] int[] maxDrops;

        [System.Serializable]
        class DropConfig
        {
#pragma warning disable CA2235 // Mark all non-serializable fields
            public InventoryItem item;
#pragma warning restore CA2235 // Mark all non-serializable fields
            public float[] relativeChance;
            public int[] minNumber;
            public int[] maxNumber;
            public int GetRandomNumber(int level)
            {
                if (item.IsStackable())
                    return 1;
                return UnityEngine.Random.Range(GetByLevel(minNumber, level), GetByLevel(maxNumber, level) + 1);
            }
        }
        static T GetByLevel<T>(T[] values, int level)
        {
            if (values.Length == 0)
                return default;
            if (level > values.Length)
                return values[values.Length - 1];
            if (level <= 0)
                return default;
            return values[level - 1];
        }
        public struct Dropped
        {
            public InventoryItem item;
            public int number;
        }

        public IEnumerable<Dropped> GetRandomDrops(int level)
        {
            if (!ShouldRandomDrop(level))
            {
                yield break;
            }
            for (int i = 0; i < GetRandomNumberOfDrops(level); i++)
            {
                yield return GetRandomDrop(level);
            }
        }

        DropConfig SelectRandomItem(int level)
        {
            float totalChance = GetTotalChance(level);
            float randomRoll = UnityEngine.Random.Range(0, totalChance);
            float chanceTotal = 0;
            foreach (var drop in potentialDrops)
            {
                chanceTotal += GetByLevel(drop.relativeChance,level);
                if (chanceTotal > randomRoll)
                    return drop;
            }
            return null;
        }

        private float GetTotalChance(int level)
        {
            float total = 0;
            foreach (var drop in potentialDrops)
            {
                total += GetByLevel(drop.relativeChance, level);
            }
            return total;
        }

        Dropped GetRandomDrop(int level)
        {
            var drop = SelectRandomItem(level);
            var result = new Dropped();
            result.item = drop.item;
            result.number = drop.GetRandomNumber(level);
            return result;
        }
        private int GetRandomNumberOfDrops(int level)
        {
            return UnityEngine.Random.Range(GetByLevel(minDrops, level), GetByLevel(maxDrops, level));
        }

        private bool ShouldRandomDrop(int level)
        {
            return UnityEngine.Random.Range(0, 100) < GetByLevel(dropChancePercentage, level);
        }
    }
}
