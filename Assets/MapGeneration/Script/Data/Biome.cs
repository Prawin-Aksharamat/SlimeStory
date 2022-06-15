using System;
using UnityEngine;
using System.Linq;

namespace Rogue.Data
{
    [CreateAssetMenu(menuName = "Biome")]
    public class Biome : ScriptableObject
    {
        [SerializeField] GameObject[] spaceTiles;
        [SerializeField] GameObject[] wallTiles;
        [SerializeField] GameObject[] topLeftTiles;
        [SerializeField] GameObject[] topTiles;
        [SerializeField] GameObject[] topRightTiles;
        [SerializeField] GameObject[] bottomLeftTiles;
        [SerializeField] GameObject[] bottomTiles;
        [SerializeField] GameObject[] bottomRightTiles;
        [SerializeField] GameObject[] leftTiles;
        [SerializeField] GameObject[] rightTiles;

        [SerializeField] GameObject[] bottomLeftProtrudeTiles;
        [SerializeField] GameObject[] bottomRightProtrudeTiles;
        [SerializeField] GameObject[] topLeftProtrudeTiles;
        [SerializeField] GameObject[] topRightProtrudeTiles;

        [SerializeField] GameObject[] pebbleTiles;
        [SerializeField] GameObject[] bedrockTiles;


        [SerializeField] GameObject[] producers;
        [SerializeField] int[] producersEncounterChance;
        [SerializeField] GameObject[] primaryConsumers;
        [SerializeField] int[] primaryConsumersEncounterChance;
        [SerializeField] GameObject[] secondaryConsumers;
        [SerializeField] int[] secondaryConsumersEncounterChance;
        [SerializeField] GameObject[] puzzles;
        [SerializeField] int[] puzzlesEncounterChance;
        [SerializeField] GameObject[] resources;
        [SerializeField] int[] resourcesEncounterChance;
        [SerializeField] GameObject[] traps;
        [SerializeField] int[] trapsEncounterChance;


        public GameObject[] getSpaceTiles() { return spaceTiles; }
        public GameObject[] getWallTiles() { return wallTiles; }
        public GameObject[] getTopLeftTiles() { return topLeftTiles; }
        public GameObject[] getTopTiles() { return topTiles; }
        public GameObject[] getTopRightTiles() { return topRightTiles; }
        public GameObject[] getBottomLeftTiles() { return bottomLeftTiles; }
        public GameObject[] getBottomTiles() { return bottomTiles; }
        public GameObject[] getBottomRightTiles() { return bottomRightTiles; }
        public GameObject[] getLeftTiles() { return leftTiles; }
        public GameObject[] getRightTiles() { return rightTiles; }
        public GameObject[] getBottomLeftProtrudeTiles() { return bottomLeftProtrudeTiles; }
        public GameObject[] getBottomRightProtrudeTiles() { return bottomRightProtrudeTiles; }
        public GameObject[] getTopLeftProtrudeTiles() { return topLeftProtrudeTiles; }
        public GameObject[] getTopRightProtrudeTiles() { return topRightProtrudeTiles; }
        public GameObject[] getPebbleTiles() { return pebbleTiles; }
        public GameObject[] getBedrockTiles() { return bedrockTiles; }


        public GameObject GetProducer()
        {
            return producers[getRandomIndexFromChance(producersEncounterChance)];
        }
        public GameObject GetPrimaryConsumer()
        {
            return primaryConsumers[getRandomIndexFromChance(primaryConsumersEncounterChance)];
        }
        public GameObject GetSecondaryConsumer()
        {
            return secondaryConsumers[getRandomIndexFromChance(secondaryConsumersEncounterChance)];
        }

        public GameObject GetResource()
        {
            return resources[getRandomIndexFromChance(resourcesEncounterChance)];
        }

        public GameObject GetPuzzle()
        {
            return puzzles[getRandomIndexFromChance(puzzlesEncounterChance)];
        }

        public GameObject GetTrap()
        {
            return traps[getRandomIndexFromChance(trapsEncounterChance)];
        }

        private int getRandomIndexFromChance(int[] Chance)
        {
            int i = Mathf.RoundToInt(UnityEngine.Random.Range(0, Chance.Sum())); 
            for(int x=0;x<=Chance.Length;x++)
            {
                i -= Chance[x];
                
                if (i <= 0) return x;
            }
            return -1;
        }
    }
}
