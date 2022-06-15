using System;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;
using Rogue.Stats;

public class Attacker : MonoBehaviour
{
    [SerializeField] GameObject attackEffect;
    [SerializeField] AudioClip attackSound;
    sfxPlayer sfx;

    private VisionManager visionManager;
    private GameObject gameManager;
    private TurnOrder turnOrder;

    private Animator animator;



    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        animator = GetComponentInChildren<Animator>();
        visionManager = gameManager.GetComponent<VisionManager>();
        turnOrder = gameManager.GetComponent<TurnOrder>();
        sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
    }
    
    public void Attack(Vector2 turnDirection)
    {

        string stateName= null;

        if (visionManager.IsInPlayerVision(transform.position))
        {
            ClearTrigger();
            if (turnDirection.x == 1)
            {
                animator.SetTrigger("IdleRight");
                stateName = "AttackRight";
            }
            else if (turnDirection.x == -1)
            {
                animator.SetTrigger("IdleLeft");
                stateName = "AttackLeft";
            }
            else if (turnDirection.y == 1)
            {
                animator.SetTrigger("IdleUp");
                stateName = "AttackUp";
            }
            else if (turnDirection.y == -1)
            {
                animator.SetTrigger("IdleDown");
                stateName = "AttackDown";
            }
            sfx.PlayThisClip(attackSound);
            animator.SetTrigger("Attack");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, turnDirection, 1f, LayerMask.GetMask("RaycastLayer"));
            if (hit.collider != null)
            {
                Health health = hit.collider.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(GetComponent<BaseStats>().GetStat(Stats.Damage), gameObject);
                }

                DestroyableWall destroyableWall = hit.collider.GetComponent<DestroyableWall>();
                if (destroyableWall != null)
                {
                    destroyableWall.DestroyWall();
                }
            }

            /*while (AnimatorIsPlaying(stateName))
            {

            }*/

            /*
            if (turnDirection.x == 1)
            {
                animator.SetTrigger("IdleRight");
            }
            else if (turnDirection.x == -1)
            {
                animator.SetTrigger("IdleLeft");
            }
            else if (turnDirection.y == 1)
            {
                animator.SetTrigger("IdleUp");
            }
            else if (turnDirection.y == -1)
            {
                animator.SetTrigger("IdleDown");
            }*/

            turnOrder.EndTurn();

            Debug.Log("EndOfAttackInVision");
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, turnDirection, 1f, LayerMask.GetMask("RaycastLayer"));
            if (hit.collider != null)
            {
                Health health = hit.collider.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(GetComponent<BaseStats>().GetStat(Stats.Damage), gameObject);
                }
            }
            turnOrder.EndTurn();
        }
    }

    private void ClearTrigger()
    {
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkRight");
        animator.ResetTrigger("IdleUp");
        animator.ResetTrigger("IdleDown");
        animator.ResetTrigger("IdleLeft");
        animator.ResetTrigger("IdleRight");
        animator.ResetTrigger("Attack");
    }

}