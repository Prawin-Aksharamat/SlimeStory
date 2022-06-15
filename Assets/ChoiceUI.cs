using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceUI : MonoBehaviour
{
    [SerializeField] GameObject uiContainer = null;
    private Action currentAction;
    sfxPlayer sfx;

    private void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
    }

    public void OpenChoice(Action doWhenYes)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TriggerIsOpenUI();
        uiContainer.active = true;
        currentAction = doWhenYes;
    }

    public void CloseChoice()
    {     
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TriggerIsOpenUI();
        uiContainer.active = false;
    }

    public void YesClick()
    {
        currentAction();
        sfx.PlayYes();
        CloseChoice();
    }

    public void NoClick()
    {
        sfx.PlayNo();
        CloseChoice();
    }

    public void MouseHover()
    {
        sfx.PlaySelect();
    }

}
