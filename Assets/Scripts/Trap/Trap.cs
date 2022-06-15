using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap:MonoBehaviour, Itrap
{
    protected AudioSource audioSource;
    protected VisionManager visionManager;
    private GameObject gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        audioSource = GetComponent<AudioSource>();
        visionManager = gameManager.GetComponent<VisionManager>();
    }

    public void activateTrap()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("agh!");
    }

}
