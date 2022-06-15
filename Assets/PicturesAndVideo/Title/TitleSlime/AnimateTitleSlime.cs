using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateTitleSlime : MonoBehaviour
{
    [SerializeField]Sprite[] slime;
    Image image;
    private bool isFinished=true;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (isFinished)
        {
            isFinished = false;
            StartCoroutine(Animated());
        }
    }

    private IEnumerator Animated()
    {
        image.sprite = slime[0];
        yield return new WaitForSeconds(0.3f);
        image.sprite = slime[1];
        yield return new WaitForSeconds(0.2f);
        image.sprite = slime[2];
        yield return new WaitForSeconds(0.1f);
        image.sprite = slime[3];
        yield return new WaitForSeconds(0.3f);
        image.sprite = slime[4];
        yield return new WaitForSeconds(0.1f);
        image.sprite = slime[5];
        yield return new WaitForSeconds(0.2f);
        image.sprite = slime[6];
        yield return new WaitForSeconds(0.3f);
        isFinished = true;
    }
}
