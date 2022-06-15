using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Map;
using Rogue.Attributes;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Behaviour currentBehaviour;
    [SerializeField] private GameObject Egg;
    [SerializeField] private Entity entityType;

    [SerializeField] private BiomeType biomeType; 

    [SerializeField] private GameObject pathObject;
    [SerializeField] protected float layEggThreshold = 0.5f;
    [SerializeField] private float huntingThreshold = 0f;

    private bool isConfused = false;
    private bool isParalyzed = false;

    private bool IsHunting = false;
    private GameObject currentHuntingTarget;

    private bool CanLayEgg=false;

    protected Movement movement;
    protected Attacker attacker;
    protected MapManager mapManager;
    protected Vector3 currentPosition;
    protected TurnOrder turnOrder;
    private GameObject gameManager;
    protected Node[,] map;
    private ArrayList direction;
    private SpriteRenderer sprite;
    private Vector2 turnDirection;
    private DungeonManager dungeonManager;

    private int oldSortingOrder;

    private StatusEffectPort statusEffectPort;

    protected Health health;

    protected Pathfinding pathFind;

    // Start is called before the first frame update
    protected void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        dungeonManager = GameObject.FindGameObjectWithTag("MapGeneration").GetComponent<DungeonManager>();
        movement = GetComponent<Movement>();
        attacker = GetComponent<Attacker>();
        mapManager = gameManager.GetComponent<MapManager>();
        turnOrder = gameManager.GetComponent<TurnOrder>();
        sprite = GetComponentInChildren<SpriteRenderer>();       
        pathFind= gameManager.GetComponent<Pathfinding>();
        health = GetComponent<Health>();
        statusEffectPort = GetComponent<StatusEffectPort>();
        GenerateArrayList();
    }

    protected void Start()
    {
        currentPosition = transform.position;
        mapManager.UpdateMap(currentPosition,4,gameObject);
        turnOrder.AddToList(gameObject);
        dungeonManager.AddEntityToDungeon(gameObject);
    }

    public virtual void Move()
    {
        Debug.Log("Move");

        if (health.IsDead())
        {
            turnOrder.EndTurn();
            return;
        }

        statusEffectPort.ActivateStatusEffect();

        if (health.IsDead())
        {
            turnOrder.EndTurn();
            return;
        }

        if (isParalyzed)
        {
            isParalyzed = false;
            turnOrder.EndTurn();
            return;
        }

        if (isConfused)
        {
            Idle();
            Debug.Log("confusedIdle");
            return;
        }

        if (dungeonManager.IsNearlyExtinct(entityType, biomeType))
        {
            UrgentLayEgg();
            return;
        }

        if (CanLayEgg)
        {
            if (Random.value > layEggThreshold) currentBehaviour = Behaviour.LayEgg;
            else currentBehaviour = Behaviour.Idle;
        }
        else if (!IsHunting)
        {
            if (Random.value >= huntingThreshold && dungeonManager.CanLayEgg(entityType)) currentBehaviour = Behaviour.Hunting;
            else currentBehaviour = Behaviour.Idle;
        }


        //CheckIfHasIntruder
        Node intruder = FindIntruder();
        //เงื่อนไขที่ took precedence เหนือสถานะ
        if (intruder != null)
        {
            map = mapManager.GetMap();
            currentPosition = transform.position;
            float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(intruder.xCoor);
            float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(intruder.yCoor);
            if (xCoordinate > currentPosition.x) attacker.Attack(new Vector2(1, 0));
            else if (xCoordinate < currentPosition.x) attacker.Attack(new Vector2(-1, 0));
            else if (yCoordinate > currentPosition.y) attacker.Attack(new Vector2(0, 1));
            else if (yCoordinate < currentPosition.y) attacker.Attack(new Vector2(0, -1));
        }
        //เงื่อนไขที่ took จาก สถานะ
        else
        {
            //Debug.Log("Intruder=null");
            if (currentBehaviour == Behaviour.Hunting)
            {
                Hunting();
            }
            else if (currentBehaviour == Behaviour.LayEgg)
            {
                LayEgg();
            }
            else if (currentBehaviour == Behaviour.Idle)
            {
                Idle();
                Debug.Log("NormalIdle");
            }
            
        }
    }

    protected virtual void Hunting()
    {

        Debug.Log("Hunting");
        map = mapManager.GetMap();
        currentPosition = transform.position;
        if (!IsHunting)
        {
            IsHunting = true;
            currentHuntingTarget = dungeonManager.GetHuntingPrey(entityType,biomeType);
        }

        if (currentHuntingTarget as GameObject == null)
        {
            IsHunting = false;
            currentHuntingTarget = null;
            Idle();
            Debug.Log("NullIdle");
            return;
        }

        //check ว่าถึงหน้า resource ยัง
        if ((currentPosition.x == currentHuntingTarget.transform.position.x + 1 && currentPosition.y == currentHuntingTarget.transform.position.y)
            || (currentPosition.x == currentHuntingTarget.transform.position.x - 1 && currentPosition.y == currentHuntingTarget.transform.position.y)
            || (currentPosition.x == currentHuntingTarget.transform.position.x && currentPosition.y == currentHuntingTarget.transform.position.y + 1)
            || (currentPosition.x == currentHuntingTarget.transform.position.x && currentPosition.y == currentHuntingTarget.transform.position.y - 1)
            )

        {
            Debug.Log("Eating");
            GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText("Eating", Color.green);
            IsHunting = false;
            currentHuntingTarget = null;
            CanLayEgg = true;
            turnOrder.EndTurn();
        }
        else
        {

            Node start = map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)];
            Node target = map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentHuntingTarget.transform.position.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentHuntingTarget.transform.position.y)];

            /* List<Node> neighbourNodeList = mapManager.GetNeighbours(target);
            while (neighbourNodeList.Count > 0)
             {
                 int randomNumber = (int)Random.Range(0, neighbourNodeList.Count);
                 if (neighbourNodeList[randomNumber].Type != 1 && neighbourNodeList[randomNumber].Type != 4 && neighbourNodeList[randomNumber].Type != 5)
                 {
                     neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                 }
                 else
                 {
                     Debug.Log("Start! : " + currentPosition.x + " " + currentPosition.y);                
                     target = neighbourNodeList[randomNumber];
                     Debug.Log("FoundTarget! : " + mapManager.GridmapCoordinateXToWorldCoordinateX(target.xCoor) + " " + mapManager.GridmapCoordinateYToWorldCoordinateY(target.yCoor));
                     break;
                 }
             }*/
            List<Node> path = pathFind.FindPath(start, target);

            Debug.Log("AfterPathFind");
            if (path == null || path.Count==0)
            {/*
                Debug.Log("Meowwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
                Debug.Log("Start at " + mapManager.GridmapCoordinateXToWorldCoordinateX(start.xCoor) + " " + mapManager.GridmapCoordinateYToWorldCoordinateY(start.yCoor));
                Debug.Log("End at " + mapManager.GridmapCoordinateXToWorldCoordinateX(target.xCoor) + " " + mapManager.GridmapCoordinateYToWorldCoordinateY(target.yCoor));
                pathFind.FindPathDebug(start, target, pathObject);*/

                Idle();
                return;
            }
            /*foreach(Node i in path)
            {
                Instantiate(pathObject, new Vector3(mapManager.GridmapCoordinateXToWorldCoordinateX(i.xCoor), mapManager.GridmapCoordinateYToWorldCoordinateY(i.yCoor), transform.position.z), Quaternion.identity);
            }*/

            float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(path[0].xCoor);
            float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(path[0].yCoor);

            if (path[0].Type != 1)
            {
                if (xCoordinate > currentPosition.x) attacker.Attack(new Vector2(1, 0));
                else if (xCoordinate < currentPosition.x) attacker.Attack(new Vector2(-1, 0));
                else if (yCoordinate > currentPosition.y) attacker.Attack(new Vector2(0, 1));
                else if (yCoordinate < currentPosition.y) attacker.Attack(new Vector2(0, -1));
            }
            else
            {
                if (xCoordinate > currentPosition.x) movement.Move("right");
                else if (xCoordinate < currentPosition.x) movement.Move("left");
                else if (yCoordinate > currentPosition.y) movement.Move("up");
                else if (yCoordinate < currentPosition.y) movement.Move("down");
            }
        }
    }

    protected virtual Node FindIntruder()
    {
        Debug.Log("FindIntruder");
        map = mapManager.GetMap();
        currentPosition = transform.position;
        List<Node> neighbourNodeList = mapManager.GetNeighbours(map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)]);

        while (neighbourNodeList.Count > 0)
        {
            int randomNumber = (int)Random.Range(0, neighbourNodeList.Count);
            if (neighbourNodeList[randomNumber].Type != 4 && neighbourNodeList[randomNumber].Type != 5)
            {
                neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
            }
            else
            {
                    GameObject entity = neighbourNodeList[randomNumber].entity;
                    if(entity as GameObject == null)
                     {
                    neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                    continue;
                     }
                    Player entityAsPlayer = entity.GetComponent<Player>();
                    Enemy entityAsEnemy = entity.GetComponent<Enemy>();
                    Egg entityAsEgg = entity.GetComponent<Egg>();
                    if (entityAsPlayer != null)
                    {
                        return neighbourNodeList[randomNumber];
                    }
                    else if (entityAsEnemy != null)
                    {
                        if(entityAsEnemy.GetBiomeType()!=biomeType || entityAsEnemy.GetEntity() != entityType)
                        {
                            return neighbourNodeList[randomNumber];
                        }
                        else
                        {
                            neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                            continue;
                        }
                    }
                    else
                    {
                        if (entityAsEgg.GetBiomeType() != biomeType || entityAsEgg.GetEntityType() != entityType)
                        {
                            return neighbourNodeList[randomNumber];
                        }
                        else
                        {
                            neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                            continue;
                        }
                    }
                
            }
        }
        
        return null;
    }

    private Collider2D GetColliderFromNode(Node node)
    {
        float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(node.xCoor);
        float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(node.yCoor);
        if (xCoordinate > currentPosition.x) return Physics2D.Raycast(transform.position, new Vector2(1, 0), 1f, LayerMask.GetMask("RaycastLayer")).collider;
        else if (xCoordinate < currentPosition.x) return Physics2D.Raycast(transform.position, new Vector2(-1, 0), 1f, LayerMask.GetMask("RaycastLayer")).collider;
        else if (yCoordinate > currentPosition.y) return Physics2D.Raycast(transform.position, new Vector2(0, 1), 1f, LayerMask.GetMask("RaycastLayer")).collider;
        else if (yCoordinate < currentPosition.y) return Physics2D.Raycast(transform.position, new Vector2(0, -1), 1f, LayerMask.GetMask("RaycastLayer")).collider;
        else return null;
    }


    protected void Idle()
    {
        Debug.Log("Idle");
        map = mapManager.GetMap();
        currentPosition= transform.position;
        List<Node> neightbourNodeList = mapManager.GetNeighbours(map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)]);
        while (neightbourNodeList.Count > 0)
        {
            Debug.Log("IdleLoop");
            int randomNumber = (int)Random.Range(0, neightbourNodeList.Count);
            if (neightbourNodeList[randomNumber].Type!=1)
            {
                neightbourNodeList.Remove(neightbourNodeList[randomNumber]);
            }
            else
            {
                float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(neightbourNodeList[randomNumber].xCoor);
                float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(neightbourNodeList[randomNumber].yCoor);
                if(xCoordinate>currentPosition.x) movement.Move("right");
                else if (xCoordinate < currentPosition.x) movement.Move("left");
                else if (yCoordinate > currentPosition.y) movement.Move("up");
                else if (yCoordinate < currentPosition.y) movement.Move("down");
                return;
            }
        }
        turnOrder.EndTurn();
        Debug.Log("OutIdleLoop");
    }

    protected virtual void LayEgg()
    {
        Debug.Log("LayEgg");
        if (dungeonManager.CanLayEgg(entityType))
        {
            map = mapManager.GetMap();
            currentPosition = transform.position;
            List<Node> neighbourNodeList = mapManager.GetNeighbours(map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)]);
            while (neighbourNodeList.Count > 0)
            {
                Debug.Log("In loop can egg " + CanLayEgg);
                int randomNumber = (int)Random.Range(0, neighbourNodeList.Count);
                if (neighbourNodeList[randomNumber].Type!=1)
                {
                    neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                }
                else
                {
                    Debug.Log("LayEggSuccessful");
                    CanLayEgg = false;
                    float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(neighbourNodeList[randomNumber].xCoor);
                    float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(neighbourNodeList[randomNumber].yCoor);
                    Vector3 eggPos = new Vector3(xCoordinate, yCoordinate, transform.position.z);
                    Instantiate(Egg, eggPos, Quaternion.identity);
                    turnOrder.EndTurn();
                    return;
                }
            }
        }
        Idle();
        Debug.Log("IdleAtEgg");
    }

    protected virtual void UrgentLayEgg()
    {

            map = mapManager.GetMap();
            currentPosition = transform.position;
            List<Node> neighbourNodeList = mapManager.GetNeighbours(map[mapManager.WorldCoordinateXToGridmapCoordinateX((int)currentPosition.x), mapManager.WorldCoordinateYToGridmapCoordinateY((int)currentPosition.y)]);
            while (neighbourNodeList.Count > 0)
            {
                Debug.Log("In loop can egg " + CanLayEgg);
                int randomNumber = (int)Random.Range(0, neighbourNodeList.Count);
                if (neighbourNodeList[randomNumber].Type != 1)
                {
                    neighbourNodeList.Remove(neighbourNodeList[randomNumber]);
                }
                else
                {
                    Debug.Log("LayEggSuccessful");
                    CanLayEgg = false;
                    float xCoordinate = mapManager.GridmapCoordinateXToWorldCoordinateX(neighbourNodeList[randomNumber].xCoor);
                    float yCoordinate = mapManager.GridmapCoordinateYToWorldCoordinateY(neighbourNodeList[randomNumber].yCoor);
                    Vector3 eggPos = new Vector3(xCoordinate, yCoordinate, transform.position.z);
                    Instantiate(Egg, eggPos, Quaternion.identity);
                    turnOrder.EndTurn();
                    return;
                }
            }
        
        Idle();
        Debug.Log("IdleAtEgg");
    }

    private void GenerateArrayList()
    {
        direction = new ArrayList{"up", "down", "left", "right"};
    }

    //-----------------------------------Debug-----------------------------------------//
    public void Pass()
    {
        movement.Pass();
    }
    //-----------------------------------Debug-----------------------------------------//
    public Entity GetEntity()
    {
        return entityType;
    }

    public BiomeType GetBiomeType()
    {
        return biomeType;
    }

    private void OnDestroy()
    {
        dungeonManager.RemoveGameObjectFromDungeon(gameObject);
    }

    public void SetConfused(bool x) => isConfused = x;
    public void SetParalyzed(bool x) => isParalyzed = x;

    public GameObject GetCurrentHuntingTarget() => currentHuntingTarget;

    public void GiveEgg() => CanLayEgg = true;

    public void CancelHunting() => IsHunting = false;
}
