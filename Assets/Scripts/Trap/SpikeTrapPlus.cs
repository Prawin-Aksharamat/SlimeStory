using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class SpikeTrapPlus : Trap
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Activate");
        if (collision.GetComponent<Health>())
        {
            if (visionManager.IsInPlayerVision(transform.position)) audioSource.Play();
            collision.GetComponent<Health>().TakeDamage(10);
        }
    }
    

}
