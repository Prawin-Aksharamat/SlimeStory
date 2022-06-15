using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Stats;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField]Stats statToDisplay;
    TextMeshProUGUI text;
    BaseStats baseStats;

    private void Awake()
    {
        baseStats=GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = (baseStats.GetStat(statToDisplay)).ToString("0");
    }
}
