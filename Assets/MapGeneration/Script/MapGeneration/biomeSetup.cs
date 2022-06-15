using Rogue.Data;
using System.Collections;
using UnityEngine;

namespace Rogue.Map
{
    public class BiomeSetup
    {

        public void StartBiomeSetup(SpaceNode root, Biome[] biomes) {

            SpaceNode[] allSpacePartitions = root.getAllChildsFormCurrentNode();

            foreach(SpaceNode i in allSpacePartitions){
                if (biomes.Length <= 1)
                {
                    if (i.getParent() == root)
                    {
                        i.setBiome(biomes[Mathf.RoundToInt(Random.Range(0, biomes.Length))]);
                    }
                    else if (i != root)
                    {
                        i.setBiome(i.getParent().getBiome());
                    }
                }
                else
                {
                    if (i == root.getRight())
                    {
                        i.setBiome(biomes[0]);
                    }
                    else if (i == root.getLeft())
                    {
                        i.setBiome(biomes[1]);
                    }
                    else if(i != root)
                    {
                        i.setBiome(i.getParent().getBiome());
                    }
                }
            }

        }

    }
}
