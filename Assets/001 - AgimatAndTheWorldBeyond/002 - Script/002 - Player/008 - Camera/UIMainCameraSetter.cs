using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainCameraSetter : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    private void OnEnable()
    {
        canvas.worldCamera = GameManager.instance.uiCamera;
    }
}
