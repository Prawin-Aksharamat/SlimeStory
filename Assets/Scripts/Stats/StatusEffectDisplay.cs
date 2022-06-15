using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDisplay : MonoBehaviour
{

    public GameObject AddStatusEffectToDisplay(StatusEffect statusEffect) {
        GameObject sprite = Instantiate(statusEffect.GetStatusEffectSprite(),gameObject.transform);
        sprite.transform.parent = transform;
        return sprite;
    }

    public void ClearStatusEffectDisplay()
    {
        foreach(Transform i in GetComponentsInChildren<Transform>())
        {
            if (i != transform)
            {
                Destroy(i.gameObject);
            }
        }
    }
}
