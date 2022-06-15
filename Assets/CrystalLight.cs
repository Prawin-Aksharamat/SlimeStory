using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CrystalLight : MonoBehaviour
{
    Light2D crystalLight;
    float lightInt;
    [SerializeField] float minInt=0.1f;
    [SerializeField] float maxInt=0.7f;
    private bool finish = true;
    // Start is called before the first frame update
    void Start()
    {
        crystalLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finish)
        {
            lightInt = Random.Range(minInt, maxInt);
            StartCoroutine(ChangeLight(lightInt));
            finish = false;
        }
    }

    private IEnumerator ChangeLight(float targetIntensity)
    {
        bool isGreater = targetIntensity > crystalLight.intensity;
        if (isGreater)
        {
            while (crystalLight.intensity < targetIntensity)
            {
                crystalLight.intensity = crystalLight.intensity + 0.001f;
                crystalLight.pointLightInnerRadius = crystalLight.pointLightInnerRadius + 0.001f;
                crystalLight.pointLightOuterRadius = crystalLight.pointLightOuterRadius + 0.001f;
                yield return null;
            }
        }
        else
        {
            while (crystalLight.intensity > targetIntensity)
            {
                crystalLight.intensity = crystalLight.intensity - 0.001f;
                crystalLight.pointLightInnerRadius = crystalLight.pointLightInnerRadius - 0.001f;
                crystalLight.pointLightOuterRadius = crystalLight.pointLightOuterRadius - 0.001f;
                yield return null;
            }
        }
        finish = true;
    }
}
