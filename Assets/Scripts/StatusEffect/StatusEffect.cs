using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    private string StatusEffectName;
    private string StatusEffectDescription;
    private GameObject StatusEffectSprite;
    private int turnLeft = 0;
    private bool isBuff = false;

    public StatusEffect(StatusEffectTemplate statusEffectTemplate)
    {
         StatusEffectName= statusEffectTemplate.GetStatusEffectName();
         StatusEffectDescription= statusEffectTemplate.GetStatusEffectDescription();
         StatusEffectSprite= statusEffectTemplate.GetStatusEffectSprite();
         turnLeft= statusEffectTemplate.GetTurnLeft();
         isBuff= statusEffectTemplate.GetIsBuff();
    }

    public string GetStatusEffectName()
    {
        return StatusEffectName;
    }

    public bool GetIsBuff() => isBuff;

    public string GetStatusEffectDescription()
    {
        return StatusEffectDescription;
    }

    public GameObject GetStatusEffectSprite()
    {
        return StatusEffectSprite;
    }

    public void SetStatusEffectName(string StatusEffectName)
    {
        this.StatusEffectName = StatusEffectName;
    }

    public void SetStatusEffectDescription(string StatusEffectDescription)
    {
        this.StatusEffectDescription = StatusEffectDescription;
    }

    public void SetStatusEffectSprite(GameObject StatusEffectSprite)
    {
        this.StatusEffectSprite = StatusEffectSprite;
    }

    public virtual void ActivateStatusEffect(GameObject entity)
    {
        Debug.Log("Nothing Done");
    }

    public virtual void RemoveStatusEffect(GameObject entity)
    {
        Debug.Log("Nothing Done");
    }

    public void FinishTurn()
    {
        turnLeft -= 1;
    }

    public int GetTurnLeft() => turnLeft;

    public void SetTurnLeft(int turnLeft) => this.turnLeft = turnLeft;
}
