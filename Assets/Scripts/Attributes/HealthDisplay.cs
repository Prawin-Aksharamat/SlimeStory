using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Rogue.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        GameObject player;
        Health health;
        TextMeshProUGUI textMeshPro;

        private void Update()
        {
            if (health == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                health = player.GetComponent<Health>();
                textMeshPro = GetComponent<TextMeshProUGUI>();
            }
            textMeshPro.text = "HP: "+health.GetHealth() + " / "+health.GetBaseHealth();
        }

    }
}