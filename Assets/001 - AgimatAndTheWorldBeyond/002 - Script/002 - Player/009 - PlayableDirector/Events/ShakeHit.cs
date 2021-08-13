using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeHit : MonoBehaviour
{
    [SerializeField] private float durationInner;
    [SerializeField] private float amplitudeGain;
    [SerializeField] private CinemachineVirtualCamera noise;
    

    public void HitShake()
    {
        StartCoroutine(GameManager.instance.cameraShaker.
            ShakeNormalCamera(durationInner, amplitudeGain, 0f, GameManager.instance.mainCamera.transform));
    }
}
