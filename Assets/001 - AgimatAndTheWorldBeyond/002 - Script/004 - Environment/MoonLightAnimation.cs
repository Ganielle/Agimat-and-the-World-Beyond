using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MoonLightAnimation : MonoBehaviour
{
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private float speed;
    [SerializeField] private Light2D moonLight;
    [SerializeField] private float startIntensity;
    [SerializeField] private float endIntensity;

    private void Awake()
    {
        moonLight.intensity = startIntensity;

        LeanTween.value(gameObject, startIntensity, endIntensity, speed).setOnUpdate((float val)=> {
            moonLight.intensity = val;
        }).setEase(easeType).setLoopPingPong();
    }
}
