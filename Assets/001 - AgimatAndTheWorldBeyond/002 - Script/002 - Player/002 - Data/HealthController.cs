using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private PlayerCore core;

    [Header("Settings")]
    [SerializeField] private float depletionSpeed;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] float currentHealthWithDamage;
    [ReadOnly] [SerializeField] float lastDamageTime;

    private void OnTriggerEnter(Collider other)
    {
        /* 
         * TODO:
         *  FOR HITBOX 
         *  
         *  NOTE:
         *  DON'T PUT IT ON THE PARENT PLAYER
         *  PUT IT ON THE PLAYER HITBOX GO
         */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            core.ChangeBattleState();

            currentHealthWithDamage = GameManager.instance.PlayerStats.GetSetCurrentHealth -
                20f;
            if (currentHealthWithDamage <= 0)
                currentHealthWithDamage = 0;

            StartCoroutine(DepleteHealth(true));
        }

        RecoverHealth();
        RecoverHealthWhenNotSelectedCharacter();
    }

    private void OnEnable()
    {
        SetHealthOnPlayerChange();
        GameManager.instance.PlayerStats.onPlayerCharacterChange += CharacterChange;
    }

    private void OnDisable()
    {
        GameManager.instance.PlayerStats.onPlayerCharacterChange -= CharacterChange;
    }

    private void CharacterChange(object sender, EventArgs e)
    {
        SetHealthOnPlayerChange();
    }

    private void SetHealthOnPlayerChange()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetCurrentHealth =
                GameManager.instance.PlayerStats.GetSetLukasHealth;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetCurrentHealth =
                GameManager.instance.PlayerStats.GetSetLilyHealth;
    }

    IEnumerator DepleteHealth(bool updateOnTime)
    {
        if (updateOnTime)
        {
            while (GameManager.instance.PlayerStats.GetSetCurrentHealth >
                currentHealthWithDamage)
            {
                lastDamageTime = Time.time;

                GameManager.instance.PlayerStats.GetSetCurrentHealth -=
                    depletionSpeed * Time.deltaTime;

                UpdateCurrentHealthToCharacter();

                yield return null;
            }
        }
        else
        {
            GameManager.instance.PlayerStats.GetSetCurrentHealth =
                currentHealthWithDamage;

            UpdateCurrentHealthToCharacter();

            lastDamageTime = Time.time;
        }
    }

    private void RecoverHealth()
    {
        if (Time.time >= lastDamageTime + core.playerRawData.healthRecoveryDelay)
        {
            if (GameManager.instance.PlayerStats.GetSetCurrentHealth < 100f)
                GameManager.instance.PlayerStats.GetSetCurrentHealth +=
                    core.playerRawData.startingHealthRecoverPerSecond * Time.deltaTime;

            if (GameManager.instance.PlayerStats.GetSetCurrentHealth > 100f)
                GameManager.instance.PlayerStats.GetSetCurrentHealth = 100f;

            UpdateCurrentHealthToCharacter();
        }
    }

    private void UpdateCurrentHealthToCharacter()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
               PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetLukasHealth =
                GameManager.instance.PlayerStats.GetSetCurrentHealth;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetLilyHealth =
                GameManager.instance.PlayerStats.GetSetCurrentHealth;
    }

    private void RecoverHealthWhenNotSelectedCharacter()
    {
        if (Time.time >= lastDamageTime + core.playerRawData.healthRecoveryDelay)
        {
            if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            {
                if (GameManager.instance.PlayerStats.GetSetLilyHealth < 100f)
                    GameManager.instance.PlayerStats.GetSetLilyHealth += 2f * Time.deltaTime;

                else if (GameManager.instance.PlayerStats.GetSetLilyHealth > 100f)
                    GameManager.instance.PlayerStats.GetSetLilyHealth = 100f;
            }
            else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            {
                if (GameManager.instance.PlayerStats.GetSetLukasHealth < 100f)
                    GameManager.instance.PlayerStats.GetSetLukasHealth += 2f * Time.deltaTime;

                else if (GameManager.instance.PlayerStats.GetSetLukasHealth > 100f)
                    GameManager.instance.PlayerStats.GetSetLukasHealth = 100f;
            }
        }
    }
}
