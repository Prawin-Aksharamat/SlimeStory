using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class StatusEffectTrap : Trap
{
    [SerializeField] StatusEffectTemplate statusEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>())
        {
            //collison.GetComponent<Status>().GiveStatusEffect(statusEffect);
        }
    }
}
