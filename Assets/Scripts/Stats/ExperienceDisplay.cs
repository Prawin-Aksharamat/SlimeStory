using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rogue.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        BaseStats baseStats;


        private void Update()
        {
            if (baseStats == null)
            {
                baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            }
            GetComponent<TextMeshProUGUI>().text = "Level : " + baseStats.GetLevel();
        }
    }
}