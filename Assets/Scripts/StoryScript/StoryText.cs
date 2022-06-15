using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rogue.Story/StoryText")]
public class StoryText : ScriptableObject
{
    [SerializeField] private bool isWaiting = false;
    [SerializeField] string text;

    public string GetText()
    {
        return text;
    }

    public void SetWaiting(bool b)
    {
        isWaiting = b;
    }

    public bool GetIsWaiting()
    {
        return isWaiting;
    }
}
