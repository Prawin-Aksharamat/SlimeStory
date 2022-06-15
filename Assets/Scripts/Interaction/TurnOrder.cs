using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrder : MonoBehaviour //[SerializeField]
{
    private ArrayList entitylist; //array for executing
    private bool isPlayerTurn = true;
    private bool allowPlayerInput = true;
    private int entitylistindex;

    private bool nextTurn=false;

    private ArrayList temporaryAddlist = null;

    private GameObject player;

    private void Awake()
    {
        entitylist = new ArrayList();
    }

    void Update()
    {
        if (nextTurn)
        {
            nextTurn = false;
            PrivateEndTurn();
        }
    }

    public void AddToList(GameObject entity)
    {
        /*if (temporaryAddlist == null) temporaryAddlist = new ArrayList();
        temporaryAddlist.Add(entity);*/
        entitylist.Add(entity);
    }

    public void EndTurn() => nextTurn = true;

    public void PrivateEndTurn()
    {
        Debug.Log("EndTurn");
        if (isPlayerTurn)
        {/*
            if (temporaryAddlist != null)
            {
                foreach (GameObject i in temporaryAddlist)
                {
                    entitylist.Add(i);
                }
                temporaryAddlist = null;
            }*/
            //Debug.Log("ShouldBePlayerEndTurn");
            isPlayerTurn = false;


            //ReduceStatusEffectTurnLeft
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }


            if (entitylist.Count == 0)
            {

                if (player == null)
                {
                    player = GameObject.FindGameObjectWithTag("Player");
                }
                player.GetComponent<StatusEffectPort>().ActivateStatusEffect();


                isPlayerTurn = true;
                TriggerAllowPlayerInput(true);
                return;
            }
            entitylistindex = 0;
            //activateStatusEffect
            if ((GameObject)entitylist[entitylistindex] == null)
            {
                entitylistindex += 1;
                EndTurn();
                return;
            }
            if (((GameObject)entitylist[entitylistindex]).GetComponent<Enemy>() != null)
            {
                //((GameObject)entitylist[entitylistindex]).GetComponent<StatusEffectPort>().ActivateStatusEffect();
                ((GameObject)entitylist[entitylistindex++]).GetComponent<Enemy>().Move/*Pass*/();
            }
            else
            {
                ((GameObject)entitylist[entitylistindex++]).GetComponent<Egg>().Incubating();
            }
            //ReduceStatusEffectTurnLeft
        }
        else
        {
            //Debug.Log("ShouldBeEnemyEndTurn");
            if (entitylistindex < entitylist.Count)
            {
                //activateStatusEffect
                if((GameObject)entitylist[entitylistindex]==null)
                {
                    entitylistindex += 1;
                    EndTurn();
                    return;
                }
                if (((GameObject)entitylist[entitylistindex]).GetComponent<Enemy>() != null)
                {
                    //((GameObject)entitylist[entitylistindex]).GetComponent<StatusEffectPort>().ActivateStatusEffect();
                    ((GameObject)entitylist[entitylistindex++]).GetComponent<Enemy>().Move/*Pass*/();
                }
                else
                {
                    ((GameObject)entitylist[entitylistindex++]).GetComponent<Egg>().Incubating();
                }
                //ReduceStatusEffectTurnLeft


            }
            else
            {
                ArrayList objectToRemove = new ArrayList();
                foreach(GameObject i in entitylist)
                {
                    if (i == null)
                    {
                        objectToRemove.Add(i);
                    }
                }
                foreach (GameObject i in objectToRemove)
                {
                    entitylist.Remove(i);
                }
                player.GetComponent<StatusEffectPort>().ActivateStatusEffect();
                TriggerAllowPlayerInput(true);
                isPlayerTurn = true;
                //activatePlayerStatusEffect
                if (player == null)
                {
                    player = GameObject.FindGameObjectWithTag("Player");
                }
                
            }
        }
    }

    public int GetSize()
    {
        return entitylist.Count;
    }

    public bool AllowPlayerInput()
    {
        return allowPlayerInput;
    }

    public void TriggerAllowPlayerInput(bool state)
    {
        allowPlayerInput = state;
    }

    private void ActivateStatusEffect()
    {
        //UseStatus

        //ReduceTurnLeft

    }

    public void ResetTurnOrder()
    {
        isPlayerTurn = true;
        allowPlayerInput = true;
        entitylistindex = 0;
    }

}
