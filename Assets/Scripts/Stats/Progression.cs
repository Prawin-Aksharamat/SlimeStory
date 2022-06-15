using UnityEngine;
using System.Collections.Generic;
using System;

namespace Rogue.Stats
{
    [CreateAssetMenu(fileName = "New Progression", menuName = "Rogue/Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        Dictionary<CharacterClass, Dictionary<Stats, float[]>> lookupTable = null;


        public float GetStat(Stats stat,CharacterClass characterClass,int level)
        {
            BuildLookup();
/*
            Debug.Log(characterClass);
            Debug.Log(level);*/
            
            return lookupTable[characterClass][stat][level-1];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stats, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stats, float[]>();
                foreach (ProgressionStats progressionStat in progressionClass.stats)
                {
                    statLookupTable[progressionStat.stat] = progressionStat.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        public int GetLevels(Stats stat,CharacterClass characterClass) {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStats[] stats;
        }

        [System.Serializable]
        class ProgressionStats
        {
            public Stats stat;
            public float[] levels;
        }
    }
}