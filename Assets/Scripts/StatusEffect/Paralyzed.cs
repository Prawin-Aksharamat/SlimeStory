using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Paralyzed : StatusEffect
{
    private GameObject gameManager;
    private TurnOrder turnOrder;
    Player player;
    Enemy enemy;

    public Paralyzed(StatusEffectTemplate statusEffectTemplate) : base(statusEffectTemplate)
    {

    }

    public override void ActivateStatusEffect(GameObject entity)
    {
        
        player = entity.GetComponent<Player>();
        enemy= entity.GetComponent<Enemy>();

        if (player != null) player.SetParalyzed(false);
        if (GetTurnLeft() > 1)
        {
            if (Random.value > 0.5f)
            {
                entity.GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText("Paralyzed", Color.yellow);
                if(player!=null)player.SetParalyzed(true);
                else enemy.SetParalyzed(true);
            }
        }
        /*
        else
        {
            player.SetParalyzed(false);
        }*/
        
            
    }
}
