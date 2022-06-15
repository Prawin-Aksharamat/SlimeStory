using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioClip bossTheme;
    AudioSource audioSource;
    int index=0;
    bool isBossFloor = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isBossFloor) {
            if ((!audioSource.isPlaying))
            {
                audioSource.clip = bossTheme;
                audioSource.PlayDelayed(1f);
            }
        }
        else if(!audioSource.isPlaying)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
            index = (index + 1) % audioClips.Length;
        }
    }

    public void PlayBossTheme()
    {
        audioSource.Stop();
        audioSource.clip = bossTheme;
        audioSource.PlayDelayed(1f);
        isBossFloor = true;
    }

}
