using System.Collections;
using System.Collections.Generic;

using RPG.Inventories;
using RPG.Stats;

using UnityEngine;
namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
#pragma warning disable CS0649
        [SerializeField]
        Modifier[] additiveModifier;
        [SerializeField]
        Modifier[] percentageModifiers;
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            foreach (var modifier in additiveModifier)
                if (modifier.stat == stat)
                    yield return modifier.value;
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
                if (modifier.stat == stat)
                    yield return modifier.value;
        }

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }
#pragma warning restore CS0649
    }
}
