using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Stats;

public class StatusEffectPort : MonoBehaviour, IModifierProvider
{
    private Dictionary<string, StatusEffect> currentStatusEffect = new Dictionary<string, StatusEffect>();
    private StatusEffectDisplay statusEffectDisplay;

    public void AddStatusEffect(StatusEffect statusEffect)
    {

                if (currentStatusEffect.ContainsKey(statusEffect.GetStatusEffectName()))
                {
                    currentStatusEffect[statusEffect.GetStatusEffectName()] = statusEffect;
                }
                else
                {
                    currentStatusEffect.Add(statusEffect.GetStatusEffectName(), statusEffect);
                }
                if (gameObject.tag == "Player")
                {
                    UpdateStatusEffectDisplay();
                }


    }


    public void UpdateStatusEffectDisplay()
    {
        if (statusEffectDisplay == null)
        {
            statusEffectDisplay = FindObjectOfType<StatusEffectDisplay>();
        }
        statusEffectDisplay.ClearStatusEffectDisplay();
        foreach (StatusEffect i in currentStatusEffect.Values)
        {
            GameObject sprite = statusEffectDisplay.AddStatusEffectToDisplay(i);
            sprite.GetComponent<StatusEffectTooltipSpawner>().UpdateTooltipFromPort(i, sprite);
        }
    }

    public void ActivateStatusEffect()
    {

        foreach (StatusEffect i in currentStatusEffect.Values)
        {
                    i.ActivateStatusEffect(gameObject);
        }

        ArrayList keyToRemove = new ArrayList();
        foreach (string i in currentStatusEffect.Keys)
        {
            currentStatusEffect[i].FinishTurn();
            if (currentStatusEffect[i].GetTurnLeft() <= 0)
            {
                Debug.Log(currentStatusEffect[i].GetTurnLeft());
                keyToRemove.Add(i);
            }
        }

        foreach (string i in keyToRemove)
        {
            currentStatusEffect.Remove(i);
        }

        if (gameObject.tag == "Player")
        {
            UpdateStatusEffectDisplay();
        }
    }


    public void RandomRemoveOneStatusEffect()
    {
        string[] keyToRemove = new string[currentStatusEffect.Count];
        if (keyToRemove.Length <= 0) return;
        int a = 0;
        foreach (string i in currentStatusEffect.Keys)
        {
            if(!currentStatusEffect[i].GetIsBuff())keyToRemove[a++]=i;
        }

        string stringKeyToRemove = keyToRemove[Mathf.RoundToInt(UnityEngine.Random.Range(0, keyToRemove.Length))];
        currentStatusEffect[stringKeyToRemove].RemoveStatusEffect(gameObject);
        currentStatusEffect.Remove(stringKeyToRemove);

    }
    /*
    public void RemoveAllStatusEffect()
    {
        ArrayList keyToRemove = new ArrayList();
        foreach (string i in currentStatusEffect.Keys)
        {
            if (!currentStatusEffect[i].GetIsBuff()) keyToRemove.Add(i);
        }

        foreach (string i in keyToRemove)
        {
            currentStatusEffect.Remove(i);
        }
    }*/

    public IEnumerable<float> GetAdditiveModifiers(Stats stat)
    {
        foreach (StatusEffect dummy in currentStatusEffect.Values)
        {
            IModifierProvider provider = dummy as IModifierProvider;
            if (provider as IModifierProvider == null) continue;
            foreach (float modifier in provider.GetAdditiveModifiers(stat))
            {
                yield return modifier;
            }
        }

    }
    public IEnumerable<float> GetPercentageModifiers(Stats stat)
    {
        foreach (StatusEffect dummy in currentStatusEffect.Values)
        {
            IModifierProvider provider = dummy as IModifierProvider;
            if (provider == null) continue;
            foreach (float modifier in provider.GetPercentageModifiers(stat))
            {
                yield return modifier;
            }
        }
    }
}
