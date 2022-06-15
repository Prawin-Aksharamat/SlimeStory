using GameDevTV.Core.UI.Tooltips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectTooltipSpawner : TooltipSpawner
{
    private StatusEffect statusEffect;

    public override bool CanCreateTooltip()
    {
        return true;
    }

    public void UpdateTooltipFromPort(StatusEffect statusEffect, GameObject statusEffectSprite)
    {
        this.statusEffect = statusEffect;
        UpdateTooltip(statusEffectSprite);
    }

    public override void UpdateTooltip(GameObject statusEffectSprite)
    {
        if (statusEffectSprite.GetComponentInChildren<StatusEffectTooltip>() == null) return;
            statusEffectSprite.GetComponentInChildren<StatusEffectTooltip>().Setup(statusEffect.GetStatusEffectName(), statusEffect.GetTurnLeft(), statusEffect.GetStatusEffectDescription());
        }
}
