using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnscaledTimeUpdate : MonoBehaviour
{
    [SerializeField] private Renderer graphicsRenderer;

    private void Update()
    {
        graphicsRenderer.material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
