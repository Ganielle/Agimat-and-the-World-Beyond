using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreObjectSetter : MonoBehaviour
{
    [SerializeField] private bool debugMode;

    [SerializeField] private GameObject characterObj;

    [Header("Animator")]
    [SerializeField] private Animator lukasAnimator;
    [SerializeField] private Animator lilyAnimator;

    [Header("Collider")]
    [SerializeField] private Collider2D lukasCollider;
    [SerializeField] private Collider2D lilyCollider;

    [Header("SpriteRenderer")]
    [SerializeField] private SpriteRenderer lukasSR;
    [SerializeField] private SpriteRenderer lilySR;

    private void Awake()
    {
        GameManager.instance.PlayerStats.GetSetPlayerState = PlayerStats.PlayerState.ALIVE;
        GameManager.instance.PlayerStats.GetSetBattleState = PlayerStats.PlayerBattleState.ADVENTURING;
    }

    private void OnEnable()
    {
        AnimatorSetter();

        GameManager.instance.PlayerStats.GetSetPlayerCharacterObj = characterObj;
        GameManager.instance.PlayerStats.onPlayerCharacterChange += PlayerCharacterChange;
    }

    private void OnDisable()
    {
        GameManager.instance.PlayerStats.onPlayerCharacterChange -= PlayerCharacterChange;
        GameManager.instance.PlayerStats.GetSetPlayerCharacterObj = null;
    }

    private void PlayerCharacterChange(object sender, EventArgs e)
    {
        AnimatorSetter();
    }

    private void AnimatorSetter()
    {
        lukasAnimator.gameObject.SetActive(false);
        lilyAnimator.gameObject.SetActive(false);

        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
        {
            GameManager.instance.PlayerStats.GetSetPlayerAnimator = lukasAnimator;
            GameManager.instance.PlayerStats.GetSetPlayerSR = lukasSR;
            GameManager.instance.PlayerStats.GetSetPlayerCollider = lukasCollider;
            lukasAnimator.gameObject.SetActive(true);
            GameManager.instance.PlayerStats.GetSetCurrentStamina =
                GameManager.instance.PlayerStats.GetSetLukasStamina;
        }

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
        {
            GameManager.instance.PlayerStats.GetSetPlayerAnimator = lilyAnimator;
            GameManager.instance.PlayerStats.GetSetPlayerSR = lilySR;
            GameManager.instance.PlayerStats.GetSetPlayerCollider = lilyCollider;
            lilyAnimator.gameObject.SetActive(true);
            GameManager.instance.PlayerStats.GetSetCurrentStamina =
                GameManager.instance.PlayerStats.GetSetLilyStamina;
        }

        else
        {
            GameManager.instance.PlayerStats.GetSetPlayerAnimator = null;
            GameManager.instance.PlayerStats.GetSetPlayerSR = null;
            GameManager.instance.PlayerStats.GetSetPlayerCollider = null;
            lukasAnimator.gameObject.SetActive(false);
            lilyAnimator.gameObject.SetActive(false);
        }
    }
}
