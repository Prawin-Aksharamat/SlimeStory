using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Title
{

    public class PressAnyButton : MonoBehaviour
    {
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                audioSource.Play();
                StartCoroutine(GetComponentInChildren<FadePanel>().FadeOutStartScene());
            }
        }
    }
}