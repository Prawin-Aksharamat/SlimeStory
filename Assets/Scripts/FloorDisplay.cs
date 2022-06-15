using Rogue.Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorDisplay : MonoBehaviour
{
    DungeonManager dungeonManager;
    TextMeshProUGUI textMeshPro;

    private void Update()
    {
        if (dungeonManager == null)
        {
            dungeonManager = GameObject.FindGameObjectWithTag("MapGeneration").GetComponent<DungeonManager>();
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        textMeshPro.text = "Floor " + ((int)dungeonManager.getCurrentFloor()+1);
    }
}
