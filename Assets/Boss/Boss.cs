using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Attributes;
using Rogue.Stats;

public class Boss : Enemy
{
    [SerializeField] GameObject exit;
    [SerializeField] GameObject[] eggList;
    [SerializeField] GameObject spiritAttack;

    [SerializeField] int turnBeforeStartLayEgg;
    [SerializeField] int turnBeforeStartSpecialMove;
    [SerializeField] int numberOfEggToSpawn;
    [SerializeField] int maxSpawnAtATime;

    private WorldSpaceTextSpawner worldSpaceTextSpawner;

    private int stackDamage = 1;
    private bool resetStackDamage = true;

    int currentEnemyCount = 0;

    int currentTurn=1;
    int OriginalNumberOfEgg;
    int OriginalTurnBeforeStartSpecialMove;

    private bool onSpecialMoveCooldown=false;

    private void Start()
    {
        base.Start();
        OriginalNumberOfEgg = numberOfEggToSpawn;
        OriginalTurnBeforeStartSpecialMove = turnBeforeStartSpecialMove;
        worldSpaceTextSpawner = GetComponent<WorldSpaceTextSpawner>();
    }

    public override void Move()
    {

        if (health.IsDead())
        {
            turnOrder.EndTurn();
            return;
        }

        resetStackDamage = true;

        BurnDamage(new Vector2(1, 0));
        BurnDamage(new Vector2(-1, 0));
        BurnDamage(new Vector2(0, 1));
        BurnDamage(new Vector2(0, -1));

        if (resetStackDamage) stackDamage = 1;

        if (currentEnemyCount>= maxSpawnAtATime)
        {
            if (!onSpecialMoveCooldown)
            {
                onSpecialMoveCooldown = true;
                SpecialMove();
                return;
            }
            else
            {
                onSpecialMoveCooldown = false;
                Idle();
                return;
            }
            
        }
        
        if(currentTurn% turnBeforeStartLayEgg == 0 )
        {
            if (numberOfEggToSpawn > 0)
            {

                numberOfEggToSpawn -= 1;
                LayEgg();
                currentEnemyCount += 1;


            }
            else
            {
                if (turnBeforeStartSpecialMove == 0)
                {
                    SpecialMove();
                    currentTurn = 1;
                    turnBeforeStartSpecialMove = OriginalTurnBeforeStartSpecialMove;
                    numberOfEggToSpawn = OriginalNumberOfEgg;
                }
                else
                {
                    worldSpaceTextSpawner.spawnWorldSpaceText("Charge!", Color.yellow);
                    turnBeforeStartSpecialMove -= 1;
                    turnOrder.EndTurn();
                }
            }
        }
        else
        {
            currentTurn += 1;
            Idle();
        }
        
        
        /*
        if (Random.value > layEggThreshold) LayEgg();
        else Idle();*/
        //Idle();
        //LayEgg();
        //SpecialMove();
    }

    private void SpecialMove()
    {
        Instantiate(spiritAttack, transform.position, Quaternion.identity);
    }

    protected override void LayEgg()
    {
            map = mapManager.GetMap();
            currentPosition = transform.position;
            List<Node> neighbourNodeList = mapManager.GetNeighbours(map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)]);
            while (neighbourNodeList.Count > 0)
            {
                int randomNumber = (int)Random.Range(0, neighbourNodeList.Count);
                if (neighbourNodeList[randomNumber].Type != 1)
                {
                    neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                }
                else
                {
                    GameObject egg = eggList[Random.Range(0, eggList.Length)];
                    float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(neighbourNodeList[randomNumber].xCoor);
                    float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(neighbourNodeList[randomNumber].yCoor);
                    Vector3 eggPos = new Vector3(xCoordinate, yCoordinate, transform.position.z);
                    Instantiate(egg, eggPos, Quaternion.identity);
                    turnOrder.EndTurn();
                    return;
            }
            }
        turnOrder.EndTurn();     
    }


    private void OnDestroy()
    {
        Instantiate(exit, transform.position, Quaternion.identity);
    }

    public void ReportDying()
    {
        currentEnemyCount -= 1;
    }

    public void ReportSpawn()
    {
        currentEnemyCount += 1;
    }

    private void BurnDamage(Vector2 turnDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, turnDirection, 1f, LayerMask.GetMask("RaycastLayer"));
        if (hit.collider != null)
        {
            Health health = hit.collider.GetComponent<Health>();
            Player player = hit.collider.GetComponent<Player>();
            if (health != null && player!=null)
            {
                health.TakeDamage(stackDamage,Color.yellow);
                stackDamage *= 2;
                resetStackDamage = false;
            }
        }
    }
}
