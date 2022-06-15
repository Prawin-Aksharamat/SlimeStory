using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldSpaceTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject spaceTextPrefab;
    private GameObject gameManager;
    private VisionManager visionManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        visionManager = gameManager.GetComponent<VisionManager>();
    }

    public void spawnWorldSpaceText(string text,Color textColor)
    {
        if (!visionManager.IsInPlayerVision(transform.position)) return;

        spaceTextPrefab.GetComponentInChildren<TextMeshProUGUI>().text = text;
        spaceTextPrefab.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
        Instantiate(spaceTextPrefab, transform);
    }
}
