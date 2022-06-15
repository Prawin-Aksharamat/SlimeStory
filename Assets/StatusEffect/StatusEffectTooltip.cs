using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusEffectTooltip : MonoBehaviour
{
    // CONFIG DATA
    [SerializeField] TextMeshProUGUI titleText = null;
    [SerializeField] TextMeshProUGUI turnText = null;
    [SerializeField] TextMeshProUGUI bodyText = null;

    // PUBLIC

    public void Setup(string statusEffectName,int turnLeft,string statusEffectDescription)
    {
        titleText.text = statusEffectName;
        turnText.text = "Turn Left : "+turnLeft.ToString();
        bodyText.text = statusEffectDescription;
    }
}
