using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioClip mouseHoverSound;

    sfxPlayer sfx;

    private void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sfx.PlayThisClip(mouseHoverSound);
    }

}
