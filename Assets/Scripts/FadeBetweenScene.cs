using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FadeBetweenScene : MonoBehaviour
{
    [SerializeField] float speedToFade;
    private CanvasGroup objectToFade;

    private void Awake()
    {
        objectToFade = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        StartCoroutine(FadeOutIntoScene());
    }

    private IEnumerator FadeOutIntoScene()
    {
        while (objectToFade.alpha != 0)
        {
            objectToFade.alpha -=Time.deltaTime/speedToFade;
            yield return null;
        }
    }

    public IEnumerator FadeInToNextScene(Action AfterFinished)
    {
        while (objectToFade.alpha != 1)
        {
            objectToFade.alpha += Time.deltaTime / speedToFade;
            yield return null;
        }
        if (AfterFinished != null)
        {
            AfterFinished();

            StartCoroutine(FadeOutIntoScene());
        }
        else
        {

        }
    }

    public IEnumerator FinaleFade(Action AfterFinished)
    {
        while (objectToFade.alpha != 1)
        {
            objectToFade.alpha += Time.deltaTime / speedToFade;
            yield return null;
        }
        if (AfterFinished != null)
        {
            AfterFinished();

        }
        else
        {

        }
    }
}
