using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rogue.Title;
using System;

public class TextPlayer : MonoBehaviour
{
    [SerializeField] StoryText[] texts;
    [SerializeField] GameObject tutorial;
    private AudioSource audioSource;
    private TextMeshProUGUI textObject;
    private bool isReadyForNextText = true;
    private float textDelay = 0.03f;
    int index = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        textObject = GetComponentInChildren<TextMeshProUGUI>();
        texts[7].SetWaiting(true);
    }

    private void Start()
    {
        StartCoroutine(UpdateText());
    }

    private void Update()
    {
        if (isReadyForNextText && Input.anyKeyDown && index == texts.Length)
        {
            StartCoroutine(LastTextUpdate());
        }
        else if (isReadyForNextText && Input.anyKeyDown && index > texts.Length)
        {
            StartCoroutine(tutorial.GetComponent<FadeTutorial>().FadeOut());
            //loadNextSceneDelegate
        }
        else if (isReadyForNextText && Input.anyKeyDown && !texts[index].GetIsWaiting())
        {
            StartCoroutine(UpdateText());
        }
        else if (isReadyForNextText && Input.anyKeyDown && texts[index].GetIsWaiting())
        {
            isReadyForNextText = false;
            textObject.text = "";
            StartCoroutine((gameObject.transform.parent).GetComponentInChildren<FadePanel>().FadeInVideo());
        }
    }

    public void CancelWaiting()
    {
        StartCoroutine(UpdateText());
    }

    public IEnumerator UpdateText()
    {
        isReadyForNextText = false;
        textObject.text = "";
        string FullText = texts[index].GetText();
        audioSource.Play();
        foreach(char c in FullText)
        {
            textObject.text += c ;
            yield return new WaitForSeconds(textDelay);
        }
        audioSource.Stop();
        isReadyForNextText = true;
        index++;
    }

    public IEnumerator LastTextUpdate()
    {
        isReadyForNextText = false;
        string FullText = textObject.text;
        audioSource.Play();
        for (int i= FullText.Length-1; i>=0;i--)
        {
            textObject.text = FullText.Substring(0,i);
            yield return new WaitForSeconds(textDelay);
        }
        audioSource.Stop();
        textObject.text = "";
        Action dummy = SetReadyForNextText;
        StartCoroutine(tutorial.GetComponent<FadeTutorial>().FadeIn(dummy));
        index++;
        
    }

    public void SetReadyForNextText()
    {
        isReadyForNextText = true;
    }
}
