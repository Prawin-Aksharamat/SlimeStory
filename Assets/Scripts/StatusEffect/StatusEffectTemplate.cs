using UnityEngine;

[CreateAssetMenu(menuName = ("StatusEffectTemplate"))]
public class StatusEffectTemplate : ScriptableObject
{
    [SerializeField] private string StatusEffectName;
    [SerializeField] private string StatusEffectDescription;
    [SerializeField] private GameObject StatusEffectSprite;
    [SerializeField] private  int turnLeft;
    [SerializeField] private bool isBuff = false ;

    public string GetStatusEffectName()
    {
        return StatusEffectName;
    }

    public bool GetIsBuff() => isBuff;

    public string GetStatusEffectDescription()
    {
        return StatusEffectDescription;
    }

    public GameObject GetStatusEffectSprite()
    {
        return StatusEffectSprite;
    }

    public int GetTurnLeft() => turnLeft;

}
