using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource bgMusicAudioSource;
    public AudioSource ambianceAudioSource;

    public IEnumerator FadeSound(AudioSource soundSource, float speed, float targetVolume)
    {
        float currentTime = 0;
        float start = soundSource.volume;

        while (currentTime < speed)
        {
            currentTime += Time.unscaledDeltaTime;

            soundSource.volume = Mathf.Lerp(start, targetVolume, currentTime / speed);

            yield return null;
        }
    }

    public IEnumerator ChangeSound(AudioSource audioSource, AudioClip clip)
    {
        yield return StartCoroutine(FadeSound(audioSource, 0.5f, 0f));

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

        yield return StartCoroutine(FadeSound(audioSource, 0.5f, 1f));
    }
}
