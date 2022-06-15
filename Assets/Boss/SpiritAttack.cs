using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class SpiritAttack : MonoBehaviour
{
    [SerializeField] float attackReachThreshold = 0.2f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] int spiritAttackDamage;
    [SerializeField] int piercing = 2;
    private TurnOrder turnOrder;
    private VisionManager visionManager;
    private GameObject gameManager;
    int beamHitCount = 0;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        visionManager = gameManager.GetComponent<VisionManager>();
        turnOrder = gameManager.GetComponent<TurnOrder>();

    }

    private void Start()
    {
        StartCoroutine(SpiritAttackPlayer());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Boss>() != null) return;
        if (collider.GetComponent<DestroyableWall>() != null)
        {
            if (visionManager.IsInPlayerVision(transform.position)) {
                collider.GetComponent<DestroyableWall>().InstantDestroy();
                Destroy(gameObject);
                beamHitCount = 0;
                turnOrder.EndTurn();
            }
        }
        if (collider.GetComponent<Health>() != null)
        {
            collider.GetComponent<Health>().TakeBossDamage(spiritAttackDamage);
            beamHitCount += 1;
            if (beamHitCount > piercing)
            {
                Destroy(gameObject);
                beamHitCount = 0;
                turnOrder.EndTurn();
            }
            if (collider.GetComponent<Player>() != null)
            {
                Destroy(gameObject);
                beamHitCount = 0;
                turnOrder.EndTurn();
            }
        }
    }

    private IEnumerator SpiritAttackPlayer()
    {
        Vector3 targetPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        while (Vector2.Distance(transform.position, targetPos) > attackReachThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, attackSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
