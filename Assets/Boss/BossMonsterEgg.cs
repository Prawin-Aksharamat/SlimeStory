using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterEgg : Egg
{
    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().ReportDying();
    }
}
