using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Map;

public class Producer : MonoBehaviour
{

    [SerializeField] private BiomeType biomeType;

    private GameObject gameManager;
    private MapManager mapManager;

    private GameObject mapGeneration;
    private void Awake()
    {
        mapGeneration = GameObject.FindGameObjectWithTag("MapGeneration");
        mapGeneration.GetComponent<DungeonManager>().AddEntityToDungeon(gameObject);
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        mapManager = gameManager.GetComponent<MapManager>();
    }

    private void Start()
    {
        mapManager.UpdateMap(transform.position, 3, gameObject);
    }

    public BiomeType GetBiomeType()
    {
        return biomeType;
    }
}
