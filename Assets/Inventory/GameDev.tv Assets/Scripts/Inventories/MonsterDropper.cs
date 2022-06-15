using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;
using Rogue.Stats;

public class MonsterDropper : ItemDropper
{
    [SerializeField] DropLibrary dropLibrary;

    public void RandomDrop()
    {
        if (dropLibrary == null) return;
        var item = dropLibrary.SelectRandomItem(GetComponent<BaseStats>().GetLevel());
        if (item != null)
        {
            DropItem(item, 1);
        }
    }
}
