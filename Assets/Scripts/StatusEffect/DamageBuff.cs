using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;
using Rogue.Stats;


public class DamageBuff : StatusEffect, IModifierProvider
{
    float additiveDamage;
    float percentageDamage;

    public DamageBuff(StatusEffectTemplate statusEffectTemplate, float additiveDamage, float percentageDamage) : base(statusEffectTemplate)
    {
        this.additiveDamage = additiveDamage;
        this.percentageDamage = percentageDamage;
    }

    public override void ActivateStatusEffect(GameObject entity)
    {
        
    }

    public IEnumerable<float> GetAdditiveModifiers(Stats stat)
    {
        if (stat == Stats.Damage)
        {
            yield return additiveDamage;
        }
        yield return 0;
    }
    public IEnumerable<float> GetPercentageModifiers(Stats stat)
    {
        if (stat == Stats.Damage)
        {
            yield return percentageDamage;
        }
        yield return 0;
    }

}
