using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CanvasGroupFadeAnimation : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    
    int fadeLeanTween;

    public void CanvasGroupAnimation(LeanTweenType easeType, float animSpeed,
        float animDelay, float fadeTo,[Optional] Action actionAfterAnimation)
    {
        LeanTween.cancel(fadeLeanTween);

        fadeLeanTween = LeanTween.value(canvasGroup.gameObject,
            alpha => canvasGroup.alpha = alpha, canvasGroup.alpha,
            fadeTo, animSpeed).setDelay(animDelay).
            setEase(easeType).setOnComplete(actionAfterAnimation).id;
    }
}
