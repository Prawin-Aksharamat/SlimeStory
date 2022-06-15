using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    private Animator animator;
    public float speed = 4f;
    private TurnOrder turnOrder;
    private GameObject gameManager;
    private MapManager mapManager;
    private Vector3 currentPosition;
    sfxPlayer sfx;

    private string currentDirection;

    private bool isMoving=false;

    private float reachThreshold = 0.2f;

    private VisionManager visionManager;

    private Vector3 expectedPos;



    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        turnOrder = gameManager.GetComponent<TurnOrder>();
        animator = GetComponentInChildren<Animator>();
        mapManager = gameManager.GetComponent<MapManager>();
        visionManager = gameManager.GetComponent<VisionManager>();
        sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
    }
    public void Move(string direction)
    {


        isMoving = true;
        currentDirection = direction;

        Debug.Log("InMovement");
        currentPosition = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        Vector3 nextPosition= transform.position;
        switch (direction)
        {
            case "up": //Up
                nextPosition = new Vector3(currentPosition.x, currentPosition.y+1, currentPosition.z);
                break;
            case "down": //Down
                nextPosition = new Vector3(currentPosition.x, currentPosition.y-1, currentPosition.z);
                break;
            case "left": //Left
                nextPosition = new Vector3(currentPosition.x-1, currentPosition.y, currentPosition.z);
                break;
            case "right": //Right
                nextPosition = new Vector3(currentPosition.x+1, currentPosition.y, currentPosition.z);
                break;
            default:
                break;
        }

        if (visionManager.IsInPlayerVision(currentPosition,nextPosition))
        {
            ClearWalkTrigger();
            switch (direction)
            {
                case "up": //Up
                    animator.SetTrigger("WalkUp");
                    StartCoroutine(MoveIE(transform.TransformPoint(new Vector3(0, 1, 0))));
                    break;
                case "down": //Down
                    animator.SetTrigger("WalkDown");
                    StartCoroutine(MoveIE(transform.TransformPoint(new Vector3(0, -1, 0))));
                    break;
                case "left": //Left
                    animator.SetTrigger("WalkLeft");
                    StartCoroutine(MoveIE(transform.TransformPoint(new Vector3(-1, 0, 0))));
                    break;
                case "right": //Right
                    animator.SetTrigger("WalkRight");
                    StartCoroutine(MoveIE(transform.TransformPoint(new Vector3(1, 0, 0))));
                    break;
                default:
                    break;
            }
        }
        else
        {
            Vector3 targetPos;
            switch (direction)
            {
                case "up": //Up
                    targetPos = new Vector3(currentPosition.x, (currentPosition.y + 1));
                    transform.position = new Vector3(currentPosition.x, (currentPosition.y + 1));
                    expectedPos = new Vector3(currentPosition.x, (currentPosition.y + 1));
                    mapManager.UpdateMap(currentPosition, targetPos, 4, gameObject);
                    FinishMovement();
                    break;
                case "down": //Down
                    targetPos = new Vector3(currentPosition.x, (currentPosition.y - 1));
                    transform.position = new Vector3(currentPosition.x, (currentPosition.y - 1));
                    expectedPos = new Vector3(currentPosition.x, (currentPosition.y - 1));
                    mapManager.UpdateMap(currentPosition, targetPos, 4, gameObject);
                    FinishMovement();
                    break;
                case "left": //Left
                    targetPos = new Vector3(currentPosition.x-1, (currentPosition.y));
                    transform.position = new Vector3((currentPosition.x-1), currentPosition.y);
                    expectedPos = new Vector3((currentPosition.x - 1), currentPosition.y);
                    mapManager.UpdateMap(currentPosition, targetPos, 4, gameObject);
                    FinishMovement();
                    break;
                case "right": //Right
                    targetPos = new Vector3(currentPosition.x+1, (currentPosition.y));
                    transform.position = new Vector3((currentPosition.x+1), currentPosition.y);
                    expectedPos= new Vector3((currentPosition.x + 1), currentPosition.y);
                    mapManager.UpdateMap(currentPosition, targetPos, 4, gameObject);
                    FinishMovement();
                    break;
                default:
                    break;
            }
        }
    }
    private IEnumerator MoveIE(Vector3 targetPos)
    {
        Debug.Log("InMoveIE");

        targetPos = new Vector3((float)(int)(targetPos.x), (float)(int)(targetPos.y), (float)(int)targetPos.z);
        expectedPos = targetPos;

        mapManager.UpdateMap(currentPosition, targetPos, 4, gameObject);

        if (GetComponent<Player>() != null) sfx.PlayCuteWalk();

        while (Vector2.Distance(transform.position,targetPos)>reachThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed*Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        FinishMovement();
    }


    private void FinishMovement()
    {
        ClearIdleTrigger();
            switch (currentDirection)
            {
                case "up": //Up
                    
                    animator.SetTrigger("IdleUp");
                    break;
                case "down": //Down
                    animator.SetTrigger("IdleDown");
                    break;
                case "left": //Left
                    animator.SetTrigger("IdleLeft");
                    break;
                case "right": //Right
                    animator.SetTrigger("IdleRight");
                    break;
                default:
                    break;
            }

      

        Debug.Log("FinishMovement");
        //this is not good for readability
        //currentPosition = nextPosition; // This line is not necessary?
        isMoving = false;
        turnOrder.EndTurn();
    }

    private void ClearIdleTrigger()
    {
        animator.ResetTrigger("IdleUp");
        animator.ResetTrigger("IdleDown");
        animator.ResetTrigger("IdleLeft");
        animator.ResetTrigger("IdleRight");
    }
    private void ClearWalkTrigger()
    {
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkRight");
    }

    public bool GetIsMoving() => isMoving;
    public Vector3 GetExpectedPos() => expectedPos;

    public void Turn(string direction) //change idle sprite
    {
        ClearIdleTrigger();
        switch (direction)
        {
            case "up": //Up
                animator.SetTrigger("IdleUp");
                break;
            case "down": //Down
                animator.SetTrigger("IdleDown");
                break;
            case "left": //Left
                animator.SetTrigger("IdleLeft");
                break;
            case "right": //Right
                animator.SetTrigger("IdleRight");
                break;
            default:
                break;
        }
    }

    public void Pass()
    {
        turnOrder.EndTurn();
    }
}