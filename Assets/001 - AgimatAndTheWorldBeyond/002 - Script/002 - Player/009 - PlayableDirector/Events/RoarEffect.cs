using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RoarEffect : MonoBehaviour
{
    [SerializeField] private Volume postProcess;
    [Range(0, 1)]
    [SerializeField] private float rangeStart;
    [Range(0, 1)]
    [SerializeField] private float rangeEnd;
    [SerializeField] private float speed;
    [SerializeField] private float duration;

    ChromaticAberration roarEffect;
    float currentDuration;
    float range;

    private void Awake()
    {
        ChromaticAberration effect;
        if (postProcess.profile.TryGet<ChromaticAberration>(out effect))
            roarEffect = effect;
    }

    public void PlayChromaticAberration()
    {
        StartCoroutine(RoaringEffect());
    }

    IEnumerator RoaringEffect()
    {
        while (currentDuration < duration)
        {
            range = Random.Range(rangeStart, rangeEnd);
            currentDuration += Time.deltaTime;

            roarEffect.intensity.Override(Mathf.Lerp(rangeStart, range, currentDuration));

            yield return null;
        }

        roarEffect.intensity.Override(0f);
    }
}
