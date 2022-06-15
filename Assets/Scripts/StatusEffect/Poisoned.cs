using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;


public class Poisoned : StatusEffect
{

    public Poisoned(StatusEffectTemplate statusEffectTemplate):base(statusEffectTemplate)
    {

    }

    public override void ActivateStatusEffect(GameObject entity)
    {
        entity.GetComponent<Health>().TakePercentageDamage(4,Color.green);
    }
}
