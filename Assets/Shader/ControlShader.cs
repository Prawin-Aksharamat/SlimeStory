using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//In case for changeColor Shaderhttps://www.youtube.com/watch?v=SQjeNhTp_Xg

public class ControlShader : MonoBehaviour
{
    private Material material;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    public void change()
    {
        material.SetFloat("_Fade", 0.5f);
    }
}
