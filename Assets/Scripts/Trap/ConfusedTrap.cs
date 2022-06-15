using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class ConfusedTrap : Trap
{
    [SerializeField] private StatusEffectTemplate trapStatusEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ConfusedTrap");
        if (collision.GetComponent<Health>())
        {
            if (visionManager.IsInPlayerVision(transform.position)) audioSource.Play();
            collision.GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText("Confused", Color.yellow);
            if (collision.tag == "Player")
            {
                collision.GetComponent<StatusEffectPort>().AddStatusEffect(new Confused(trapStatusEffect));
            }
            else if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<StatusEffectPort>().AddStatusEffect(new Confused(trapStatusEffect));
            }
        }
    }


}
