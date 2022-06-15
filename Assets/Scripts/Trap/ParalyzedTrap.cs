using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class ParalyzedTrap : Trap
{
    [SerializeField] private StatusEffectTemplate trapStatusEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ParalyzeTrap");
        GetComponent<Animator>().SetTrigger("Activate");
        if (collision.GetComponent<Health>())
        {
            collision.GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText("Paralyzed", Color.yellow);
            StartCoroutine(AnimateTakeParalyzed(collision));

            if (collision.tag == "Player")
            {
                collision.GetComponent<StatusEffectPort>().AddStatusEffect(new Paralyzed(trapStatusEffect));
            }
            else if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<StatusEffectPort>().AddStatusEffect(new Paralyzed(trapStatusEffect));
            }
        }
    }

    private IEnumerator AnimateTakeParalyzed(Collider2D collision)
    {
        SpriteRenderer renderer = collision.GetComponentInChildren<SpriteRenderer>();
        Color normalColor = renderer.color;
        renderer.color = Color.yellow;
        yield return new WaitForSeconds(0.25f);
        renderer.color = normalColor;
    }
}
