using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryExample.UI;


public class InventoryLock : StatusEffect
{
    ShowHideUI exampleUI;

    public InventoryLock(StatusEffectTemplate statusEffectTemplate) : base(statusEffectTemplate)
    {

    }

    public override void ActivateStatusEffect(GameObject entity)
    {

        if (entity.GetComponent<Player>() == null) return;

        exampleUI = entity.GetComponentInChildren<ShowHideUI>();

        if (exampleUI != null)
        {
            if (GetTurnLeft() > 1)
            {
                exampleUI.LockInventory();
            }
            else
            {
                exampleUI.UnlockInventory();
            }
        }

    }


}
