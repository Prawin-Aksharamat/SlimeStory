using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class InventoryLockTrap : Trap
{
    [SerializeField] private StatusEffectTemplate trapStatusEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("InventoryLockTrap");
        GetComponent<Animator>().SetTrigger("Activate");
        if (collision.GetComponent<Health>())
        {
            if (visionManager.IsInPlayerVision(transform.position)) audioSource.Play();
            collision.GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText("Locked", Color.red);
        }

        if (collision.tag == "Player")
        {
            collision.GetComponent<StatusEffectPort>().AddStatusEffect(new InventoryLock(trapStatusEffect));
        }

    }


}
