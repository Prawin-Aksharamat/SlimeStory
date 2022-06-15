using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

[CreateAssetMenu(menuName="Drop Library")]
public class DropLibrary : ScriptableObject
{
    [SerializeField] float[] dropChancePercentage;
    [SerializeField] DropConfig[] potentialDrops;

    [System.Serializable]
    class DropConfig
    {
        public InventoryItem item;
        public float[] relativeChance;
    }

    public InventoryItem SelectRandomItem(int level)
    {
        if (Random.Range(0,100) > dropChancePercentage[level-1]) return null;

        float totalChance = GetTotalChance(level);
        float randomRoll = Random.Range(0, totalChance);
        float chanceTotal = 0;
        foreach (var drop in potentialDrops)
        {
            chanceTotal += GetByLevel(drop.relativeChance, level);
            if (chanceTotal > randomRoll)
            {
                return drop.item;
            }
        }
        return null;
    }

    float GetTotalChance(int level)
    {
        float total = 0;
        foreach (var drop in potentialDrops)
        {
            total += GetByLevel(drop.relativeChance, level);
        }
        return total;
    }

    static T GetByLevel<T>(T[] values, int level)
    {
        if (values.Length == 0)
        {
            return default;
        }
        if (level > values.Length)
        {
            return values[values.Length - 1];
        }
        if (level <= 0)
        {
            return default;
        }
        return values[level - 1];
    }

    bool ShouldRandomDrop(int level)
    {
        return Random.Range(0, 100) < GetByLevel(dropChancePercentage, level);
    }
}
