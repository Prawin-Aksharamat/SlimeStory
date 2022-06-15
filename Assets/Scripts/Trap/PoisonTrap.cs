using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class PoisonTrap : Trap
{
    [SerializeField] private StatusEffectTemplate trapStatusEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PoisonTrap");
        GetComponent<Animator>().SetTrigger("Activate");
        if (collision.GetComponent<Health>())
        {
            if (visionManager.IsInPlayerVision(transform.position)) audioSource.Play();
            collision.GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText("Poison", Color.green);
            StartCoroutine(AnimateTakePoison(collision));

            if (collision.tag == "Player")
            {
                collision.GetComponent<StatusEffectPort>().AddStatusEffect(new Poisoned(trapStatusEffect));
            }
            else if (collision.GetComponent<Enemy>() != null)
            {
                collision.GetComponent<StatusEffectPort>().AddStatusEffect(new Poisoned(trapStatusEffect));
            }
        }
    }

    private IEnumerator AnimateTakePoison(Collider2D collision)
    {
        SpriteRenderer renderer = collision.GetComponentInChildren<SpriteRenderer>();
        Color normalColor = renderer.color;
        renderer.color = Color.green;
        yield return new WaitForSeconds(0.25f);
        renderer.color = normalColor;
    }
}
