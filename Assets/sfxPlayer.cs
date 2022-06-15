using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxPlayer : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip inventoryOpen;
    [SerializeField] AudioClip inventoryClose;
    [SerializeField] AudioClip inventoryClick;

    [SerializeField] AudioClip cuteWalk;
    [SerializeField] AudioClip pickUp;

    [SerializeField] AudioClip Die;

    [SerializeField] AudioClip Yes;
    [SerializeField] AudioClip No;
    [SerializeField] AudioClip Select;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayInventoryOpen()
    {
        audioSource.clip = inventoryOpen;
        audioSource.volume = 0.8f;
        audioSource.Play();
    }

    public void PlayInventoryClose()
    {
        audioSource.clip = inventoryClose;
        audioSource.volume = 0.8f;
        audioSource.Play();
    }

    public void PlayInventoryClick()
    {
        audioSource.clip = inventoryClick;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayCuteWalk()
    {
        audioSource.clip = cuteWalk;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayPickUp()
    {
        audioSource.clip = pickUp;
        audioSource.volume = 0.6f;
        audioSource.Play();
    }

    public void PlayThisClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayDie()
    {
        audioSource.clip = Die;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayYes()
    {
        audioSource.clip = Yes;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlayNo()
    {
        audioSource.clip = No;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void PlaySelect()
    {
        audioSource.clip = Select;
        audioSource.volume = 1f;
        audioSource.Play();
    }

}
