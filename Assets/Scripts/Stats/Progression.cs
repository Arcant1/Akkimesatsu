using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Akki Messatsu/Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] internal ProgressionCharacterClass[] characterClasses = { };
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();
            if (!lookUpTable.ContainsKey(characterClass))
            {
                Debug.Log(stat);
            }
            float[] levels = lookUpTable[characterClass][stat];
            if (levels.Length < level) return 0;
            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();
            return lookUpTable[characterClass][stat].Length;
        }

        private void BuildLookUp()
        {
            if (lookUpTable != null) return;        //don't need to build the whole look up table
            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            // For every class
            foreach (ProgressionCharacterClass progressionCharacterClass in characterClasses)
            {
                // For every stat
                var statLevels = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionCharacterClass.stats)
                {
                    statLevels[progressionStat.stat] = progressionStat.levels;
                }
                lookUpTable[progressionCharacterClass.characterClass] = statLevels;
            }

        }

        [System.Serializable]
        internal class ProgressionCharacterClass
        {
            [SerializeField] internal CharacterClass characterClass = default;
            public ProgressionStat[] stats = { };
        }
        [System.Serializable]
        public class ProgressionStat
        {
            [SerializeField] internal Stat stat = default;
            [SerializeField] internal float[] levels = { };
        }
    }
}
