using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Consumable Item/Potion"))]
    public class Potion : ConsumableItem
    {
        [SerializeField] int AmountToHeal;
        public override void Use()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().Heal(AmountToHeal);
        }
    }
}
