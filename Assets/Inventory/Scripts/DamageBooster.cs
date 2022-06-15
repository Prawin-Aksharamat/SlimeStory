using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevTV.Inventories
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Consumable Item/DamageBooster"))]
    public class DamageBooster : ConsumableItem
    {
        [SerializeField] StatusEffectTemplate statusEffect;
        [SerializeField] float additiveDamage;
        [SerializeField] float percentageDamage;
        public override void Use()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<StatusEffectPort>().AddStatusEffect(new DamageBuff(statusEffect, additiveDamage, percentageDamage));
        }
    }
}
