using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChanger : MonoBehaviour
{
    [SerializeField] private string tagPlayer;

    [SerializeField] private AudioClip clip;
    [SerializeField] private bool canChangeClipWhenExit;
    [SerializeField] private bool isAmbianceSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tagPlayer)
        {
            if (isAmbianceSound)
                StartCoroutine(GameManager.instance.soundManager.ChangeSound(GameManager.instance.soundManager.ambianceAudioSource, clip));
            else
                StartCoroutine(GameManager.instance.soundManager.ChangeSound(GameManager.instance.soundManager.bgMusicAudioSource, clip));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tagPlayer)
        {
            if (isAmbianceSound)
                StartCoroutine(GameManager.instance.soundManager.FadeSound(GameManager.instance.soundManager.ambianceAudioSource, 0.5f, 0f));
            else
                StartCoroutine(GameManager.instance.soundManager.FadeSound(GameManager.instance.soundManager.bgMusicAudioSource, 0.5f, 0f));
        }
    }
}
