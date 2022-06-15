using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoomTemplate")]
public class RoomTemplate : ScriptableObject
{
    [SerializeField]private int minTurn;
    [SerializeField] private int maxTurn;
    [SerializeField] private int probabilityIncrement;

    [SerializeField] private float producerDensity=1;
    [SerializeField] private float primaryConsumerDensity = 1;
    [SerializeField] private float secondaryConsumerDensity = 1;
    [SerializeField] private float puzzleDensity = 1;
    [SerializeField] private float resourceDensity = 1;
    [SerializeField] private float trapDensity = 1;

    public int getMinTurn() => minTurn;
    public int getMaxTurn() => maxTurn;
    public int getProbabilityIncrement() => probabilityIncrement;

    public float getProducerDensity() => producerDensity;
    public float getPrimaryConsumerDensity() => primaryConsumerDensity;
    public float getSecondaryConsumerDensity() => secondaryConsumerDensity;
    public float getPuzzleDensity() => puzzleDensity;
    public float getResourceDensity() => resourceDensity;
    public float getTrapDensity() => trapDensity;
}
