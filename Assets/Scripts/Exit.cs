using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Map;
using System;

public class Exit : MonoBehaviour
{
    private Action changeFloor;

    private void Awake()
    {
        changeFloor = GameObject.FindObjectOfType<DungeonManager>().nextLevel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ChoiceUI choiceUI =GameObject.FindGameObjectWithTag("NextFloorChoicePanel").GetComponent<ChoiceUI>();

            choiceUI.OpenChoice(changeFloor);

            //GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>().TriggerSuddenStop();

        }

    }
}
