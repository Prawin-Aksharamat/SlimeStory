using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    private Movement movement;
    private Attacker attacker;
    private Node[,] map;
    private Vector3 currentPosition;
    private MapManager mapManager;
    private GameObject gameManager;
    private TurnOrder turnOrder;
    private SpriteRenderer sprite;
    private Vector2 turnDirection;

    private bool isConfused = false;
    private bool isParalyzed = false;
    private bool isOpenUI = false;



    // Start is called before the first frame update
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        mapManager = gameManager.GetComponent<MapManager>();
        movement = GetComponent<Movement>();
        attacker = GetComponent<Attacker>();
        turnOrder = gameManager.GetComponent<TurnOrder>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        currentPosition = transform.position;
        mapManager.UpdateMap(currentPosition, 4, gameObject);
        turnDirection = Vector2.left;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
    // Update is called once per frame
    private void Update()
    {
        if (isParalyzed && turnOrder.AllowPlayerInput())
        {
            Debug.Log("PlayerParalyzedActivated!");
            turnOrder.EndTurn();
            return;
        }

        if (Input.anyKey && turnOrder.AllowPlayerInput() && !isConfused && !isOpenUI)
        {
            Debug.Log("PlayerNormalMove");
            map = mapManager.GetMap();
            currentPosition = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                turnDirection = Vector2.up;
                if (mapManager.CantWalk((int)currentPosition.x, (int)currentPosition.y + 1)) {
                    Debug.Log("PlayerUp");
                    movement.Turn("up");
                    return;
                }
                turnOrder.TriggerAllowPlayerInput(false);
                movement.Move("up");
            }
            else if (Input.GetKey(KeyCode.S))
            {
                turnDirection = Vector2.down;
                if (mapManager.CantWalk((int)currentPosition.x, (int)currentPosition.y - 1))
                {
                    Debug.Log("PlayerDown");
                    movement.Turn("down");
                    return;
                }
                turnOrder.TriggerAllowPlayerInput(false);
                movement.Move("down");
            }
            else if (Input.GetKey(KeyCode.A))
            {
                turnDirection = Vector2.left;
                if (mapManager.CantWalk((int)currentPosition.x - 1, (int)currentPosition.y))
                {
                    Debug.Log("PlayerLeft");
                    movement.Turn("left");
                    return;
                }
                turnOrder.TriggerAllowPlayerInput(false);
                movement.Move("left");
            }
            else if (Input.GetKey(KeyCode.D))
            {
                turnDirection = Vector2.right;
                if (mapManager.CantWalk((int)currentPosition.x + 1, (int)currentPosition.y))
                {
                    Debug.Log("PlayerRight");
                    movement.Turn("right");
                    return;
                }
                turnOrder.TriggerAllowPlayerInput(false);
                movement.Move("right");
            }
            else if (Input.GetMouseButtonDown(0)) //attack
            {
                turnOrder.TriggerAllowPlayerInput(false);
                attacker.Attack(turnDirection);
            }

        }
        else if (Input.anyKey && isConfused && turnOrder.AllowPlayerInput() && !isOpenUI)
        {
            Debug.Log("InConfused");
            if (Input.GetKey(KeyCode.I))
            {

                return;
            }
                map = mapManager.GetMap();
                currentPosition = transform.position;

            bool isFinished = false;

                while (!isFinished)
                {
                Debug.Log("InIsFinish");
                    float randomValue = Random.value;
                    if (randomValue < 0.2f)
                    {

                    turnDirection = Vector2.up;
                        if (mapManager.CantWalk((int)currentPosition.x, (int)currentPosition.y + 1))
                        {
                            
                            continue;
                        }
                        turnOrder.TriggerAllowPlayerInput(false);
                        movement.Move("up");
                        isFinished = true;
                    }
                    else if (randomValue < 0.4f)
                    {

                    turnDirection = Vector2.down;
                        if (mapManager.CantWalk((int)currentPosition.x, (int)currentPosition.y - 1))
                        {
                            
                            continue;
                        }
                        turnOrder.TriggerAllowPlayerInput(false);
                        movement.Move("down");
                        isFinished = true;
                    }
                    else if (randomValue < 0.6f)
                    {

                    turnDirection = Vector2.left;
                        if (mapManager.CantWalk((int)currentPosition.x - 1, (int)currentPosition.y))
                        {
                            
                            continue;
                        }
                        turnOrder.TriggerAllowPlayerInput(false);
                        movement.Move("left");
                    isFinished = true;
                    }
                    else if (randomValue < 0.8f)
                    {

                    turnDirection = Vector2.right;
                        if (mapManager.CantWalk((int)currentPosition.x + 1, (int)currentPosition.y))
                        {
                            
                            continue;
                        }
                        turnOrder.TriggerAllowPlayerInput(false);
                        movement.Move("right");
                        isFinished = true;
                    }
                    else //attack
                    {

                        turnOrder.TriggerAllowPlayerInput(false);
                        attacker.Attack(turnDirection);
                        isFinished = true;
                    }
                }
            Debug.Log("OutIsFinish");

        }
    }

    public void SetConfused(bool x) => isConfused = x;

    public void SetParalyzed(bool x) => isParalyzed = x;

    public MapManager GetMapManager()
    {
        return mapManager;
    }

    public void TriggerIsOpenUI() => isOpenUI = !isOpenUI;
    //if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Idle"))

    private void OnDestroy()
    {
        
    }

}
