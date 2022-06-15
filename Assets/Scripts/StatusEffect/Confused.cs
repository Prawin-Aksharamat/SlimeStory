using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confused : StatusEffect
{
    Player player;

    public Confused(StatusEffectTemplate statusEffectTemplate) : base(statusEffectTemplate)
    {

    }

    public override void ActivateStatusEffect(GameObject entity)
    {
        Debug.Log("ConfusedActivated!");

        player = entity.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("Confused"+GetTurnLeft());
            if (GetTurnLeft() > 1)
            {
                player.SetConfused(true);
            }
            else
            {
                player.SetConfused(false);
            }
        }
        else
        {

            Enemy enemy=entity.GetComponent<Enemy>();
            if (GetTurnLeft() > 1)
            {
                enemy.SetConfused(true);
            }
            else
            {
                enemy.SetConfused(false);
            }
        }
    }
    

    public override void RemoveStatusEffect(GameObject entity)
    {
        player = entity.GetComponent<Player>();
        player.SetConfused(false);
    }
}
