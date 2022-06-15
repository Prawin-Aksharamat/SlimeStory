using Rogue.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EXPdisplay : MonoBehaviour
{
    GameObject player;
    BaseStats baseStats;
    TextMeshProUGUI textMeshPro;
    Experience exp;

    private void Update()
    {
        if (baseStats == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            baseStats = player.GetComponent<BaseStats>();
            textMeshPro = GetComponent<TextMeshProUGUI>();
            exp = player.GetComponent<Experience>();
        }
        if (baseStats.GetLevel() < 3)
        {
            textMeshPro.text = "EXP: " + (int)exp.GetPoints() + " / " + baseStats.GetStat(Stats.ExperienceToLevelUp);
        }
        else
        {
            textMeshPro.text = "EXP: " + (int)exp.GetPoints();
        }
    }
}
