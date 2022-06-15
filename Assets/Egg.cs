using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;

public class Egg : MonoBehaviour
{
    [SerializeField] GameObject monsterToSpawn;
    [SerializeField] private Entity entityType;
    [SerializeField] private BiomeType biomeType;
    [SerializeField] private int turnBeforeHatch=3;

    private TurnOrder turnOrder;
    private GameObject gameManager;
    private MapManager mapManager;

    private Health health;

    private int IncubateTurn = 0;

    //add  เข้า turn order แล้วนับเพื่อฝัก
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        mapManager = gameManager.GetComponent<MapManager>();
        turnOrder = gameManager.GetComponent<TurnOrder>();
        health = GetComponent<Health>();
    }

    private void Start()
    {
        mapManager.UpdateMap(transform.position, 5,gameObject);
        turnOrder.AddToList(gameObject);
    }

    public void Incubating()
    {
        if (health.IsDead())
        {
            turnOrder.EndTurn();
            return;
        }

        IncubateTurn += 1;
        if (IncubateTurn >= turnBeforeHatch)
        {
            Instantiate(monsterToSpawn, transform.position,Quaternion.identity);
            turnOrder.EndTurn();
            Destroy(gameObject);
        }
        else {
            turnOrder.EndTurn();
        }
    }

    public Entity GetEntityType()
    {
        return entityType;
    }

    public BiomeType GetBiomeType()
    {
        return biomeType;
    }

}
