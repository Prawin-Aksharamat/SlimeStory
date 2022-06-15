using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTutorial : MonoBehaviour
{
    [SerializeField] float speedToFade;
    private CanvasGroup objectToFade;
    private AudioSource audioSource;

    private void Awake()
    {
        objectToFade = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator FadeIn(Action AfterFinished )
    {
        while (objectToFade.alpha != 1)
        {
            objectToFade.alpha += Time.deltaTime/speedToFade;
            yield return null;
        }
        AfterFinished();
    }

    public IEnumerator FadeOut()
    {
        audioSource.Play();
        while (objectToFade.alpha != 0)
        {
            objectToFade.alpha -= Time.deltaTime / speedToFade;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
