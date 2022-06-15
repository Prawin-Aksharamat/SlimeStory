using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Consumable Item/Panacea"))]
    public class Panacea : ConsumableItem
    {
        public override void Use()
        {
            StatusEffectPort statusEffectPort = GameObject.FindGameObjectWithTag("Player").GetComponent<StatusEffectPort>();
            statusEffectPort.RandomRemoveOneStatusEffect();
            statusEffectPort.UpdateStatusEffectDisplay();

        }
    }
}
