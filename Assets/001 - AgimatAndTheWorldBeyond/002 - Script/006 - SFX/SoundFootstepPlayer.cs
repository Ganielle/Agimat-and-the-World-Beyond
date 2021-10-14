using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFootstepPlayer : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private AudioSource footstepSource;

    [Header("GROUND")]
    [SerializeField] private string groundTag;

    [Header("AUDIO")]
    [SerializeField] private AudioClip[] runningStoneClips;

    [Header("DEBUGGER")]
    [ReadOnly] [SerializeField] AudioClip selectedClip;
    [ReadOnly] [SerializeField] AudioClip previousClip;

    public void PlayFootstep()
    {
        if (GameManager.instance.gameplayController.sprintTapCount == 0)
        {
            if (playerCore.groundPlayerController.GetGroundTag == groundTag)
                footstepSource.PlayOneShot(GetClip(runningStoneClips));
        }
        //  Sprinting
        else if (GameManager.instance.gameplayController.sprintTapCount == 2)
        {
            //  TODO: SPRINTING
        }
    }

    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        selectedClip = clipArray[UnityEngine.Random.Range(0, clipArray.Length - 1)];

        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[UnityEngine.Random.Range(0, clipArray.Length - 1)];

            attempts--;
        }

        previousClip = selectedClip;
        return selectedClip;
    }
}
