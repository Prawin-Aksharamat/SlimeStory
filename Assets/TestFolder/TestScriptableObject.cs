using UnityEngine;

[CreateAssetMenu(menuName = ("TestScriptableObject"))]
public class TestScriptableObject : ScriptableObject
{
    [Range(0,5)]
    [SerializeField] private int testInt;
    [SerializeField] private string testString;
}

