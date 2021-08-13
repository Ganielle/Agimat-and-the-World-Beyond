using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker
{
    public IEnumerator ShakeNormalCamera(float durationInner, float magnitudeInner, float delay, Transform camera)
    {
        yield return new WaitForSecondsRealtime(delay);

        Vector3 originalPos = camera.localPosition;

        float elapsed = 0.0f;

        while (elapsed < durationInner)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitudeInner;
            float y = UnityEngine.Random.Range(-1, 1f) * magnitudeInner;

            camera.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        camera.localPosition = originalPos;
    }
}
