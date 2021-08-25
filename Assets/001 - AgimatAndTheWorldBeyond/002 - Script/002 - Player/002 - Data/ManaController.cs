using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    [SerializeField] private PlayerCore core;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float depleteSpeed;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] float currentManaReduce;
    [ReadOnly] [SerializeField] bool canUseFirstSkill;
    [ReadOnly] [SerializeField] bool canUseSecondtSkill;
    [ReadOnly] [SerializeField] bool canUseThirdSkill;
    [ReadOnly] [SerializeField] float lastChargeTime;

    private void OnEnable()
    {
        SetManaOnPlayerChange();
        CheckIfSkillCanBeUsed();
        GameManager.instance.PlayerStats.onPlayerCharacterChange += CharacterChange;
        GameManager.instance.PlayerStats.onManaChange += ManaChange;
    }

    private void OnDisable()
    {
        GameManager.instance.PlayerStats.onPlayerCharacterChange -= CharacterChange;
        GameManager.instance.PlayerStats.onManaChange -= ManaChange;
    }

    private void Update()
    {
        ChargeSkillToUseMana();
        RecoverMana();
        RecoverManaWhenNotSelectedCharacter();
    }

    private void ManaChange(object sender, EventArgs e)
    {
        CheckIfSkillCanBeUsed();
    }

    private void CharacterChange(object sender, EventArgs e)
    {
        SetManaOnPlayerChange();
    }

    private void SetManaOnPlayerChange()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetCurrentMana =
                GameManager.instance.PlayerStats.GetSetLukasMana;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetCurrentMana =
                GameManager.instance.PlayerStats.GetSetLilyMana;
    }

    private void ChargeSkillToUseMana()
    {
        if (GameManager.instance.gameplayController.isSkillCurrentlyCharging)
        {
            core.ChangeBattleState();

            if (GameManager.instance.PlayerStats.GetSetChargeMana
                < GameManager.instance.PlayerStats.GetSetCurrentMana)
                GameManager.instance.PlayerStats.GetSetChargeMana += chargeSpeed *
                    Time.deltaTime;

            if (GameManager.instance.PlayerStats.GetSetChargeMana
                >= GameManager.instance.PlayerStats.GetSetCurrentMana)
            {
                GameManager.instance.PlayerStats.GetSetChargeMana =
                    GameManager.instance.PlayerStats.GetSetCurrentMana;

                Debug.Log("use all mana when reach charge to mana");

                //  100% charge always use third skill
                UseMana();

                //GameManager.instance.gameplayController.UseManaCharging();
            }
        }
        else if (!GameManager.instance.gameplayController.isSkillCurrentlyCharging)
        {
            if (GameManager.instance.PlayerStats.GetSetChargeMana != 0f)
            {
                UseMana();
            }
        }
    }

    private void UseMana()
    {
        if (GameManager.instance.PlayerStats.GetSetChargeMana < 25f)
            GameManager.instance.PlayerStats.GetSetChargeMana = 0f;

        //  First Skill
        else if (GameManager.instance.PlayerStats.GetSetChargeMana > 25f &&
            GameManager.instance.PlayerStats.GetSetChargeMana <= 30f)
        {
            if (canUseFirstSkill)
            {
                Debug.Log("33.33 % mana");
                StartCoroutine(ReduceManaBasedOnSkillCharge(core.playerRawData.
                    firstSkillUsePercentage));
            }
        }

        //  Second Skill
        else if (GameManager.instance.PlayerStats.GetSetChargeMana > 30f &&
            GameManager.instance.PlayerStats.GetSetChargeMana <= 60f)
        {
            if (canUseSecondtSkill)
            {
                Debug.Log("66.66 % mana");
                StartCoroutine(ReduceManaBasedOnSkillCharge(core.playerRawData.
                    secondSkillUsePercentage));
            }
            else
            {
                Debug.Log("33.33 % mana");
                StartCoroutine(ReduceManaBasedOnSkillCharge(core.playerRawData.
                    firstSkillUsePercentage));
            }
        }

        //  Third Skill
        else if (GameManager.instance.PlayerStats.GetSetChargeMana > 60f &&
            GameManager.instance.PlayerStats.GetSetChargeMana <= 100f)
        {
            if (canUseThirdSkill)
            {
                Debug.Log("100 % mana");

                StartCoroutine(ReduceManaBasedOnSkillCharge(core.playerRawData.
                    thirdSkillUsePercentage));
            }
            else
            {
                Debug.Log("66.66 % mana");
                StartCoroutine(ReduceManaBasedOnSkillCharge(core.playerRawData.
                    secondSkillUsePercentage));
            }
        }
    }

    IEnumerator ReduceManaBasedOnSkillCharge(float reduce)
    {
        GameManager.instance.PlayerStats.GetSetChargeMana = 0f;

        //  Debug Purpose
        currentManaReduce = GameManager.instance.PlayerStats.GetSetCurrentMana -
            reduce;

        if (currentManaReduce < 0.9f)
            currentManaReduce = 0f;

        while (GameManager.instance.PlayerStats.GetSetCurrentMana > currentManaReduce)
        {
            GameManager.instance.PlayerStats.GetSetCurrentMana -= depleteSpeed * Time.deltaTime;

            yield return null;
        }

        if (GameManager.instance.PlayerStats.GetSetCurrentMana < 0.9f)
            GameManager.instance.PlayerStats.GetSetCurrentMana = 0f;

        lastChargeTime = Time.time;
    }

    private void CheckIfSkillCanBeUsed()
    {
        if (!GameManager.instance.gameplayController.isSkillCurrentlyCharging)
        {
            if (GameManager.instance.PlayerStats.GetSetCurrentMana >= 33.33f)
                canUseFirstSkill = true;
            else
                canUseFirstSkill = false;

            if (GameManager.instance.PlayerStats.GetSetCurrentMana >= 66.66f)
                canUseSecondtSkill = true;
            else
                canUseSecondtSkill = false;

            if (GameManager.instance.PlayerStats.GetSetCurrentMana >= 100f)
                canUseThirdSkill = true;
            else
                canUseThirdSkill = false;
        }
    }

    private void RecoverMana()
    {
        if (Time.time >= lastChargeTime + core.playerRawData.manaRecoveryDelay)
        {
            if (GameManager.instance.PlayerStats.GetSetCurrentMana < 100f)
                GameManager.instance.PlayerStats.GetSetCurrentMana +=
                    core.playerRawData.startingManaRecoveryPerSecond * Time.deltaTime;

            if (GameManager.instance.PlayerStats.GetSetCurrentMana > 100f)
                GameManager.instance.PlayerStats.GetSetCurrentMana = 100f;

            UpdateManaForCharacter();
        }
    }

    private void UpdateManaForCharacter()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetLukasMana =
                GameManager.instance.PlayerStats.GetSetCurrentMana;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetLilyMana =
                GameManager.instance.PlayerStats.GetSetCurrentMana;
    }

    private void RecoverManaWhenNotSelectedCharacter()
    {
        if (Time.time >= lastChargeTime + core.playerRawData.manaRecoveryDelay)
        {
            if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            {
                if (GameManager.instance.PlayerStats.GetSetLilyMana < 100f)
                    GameManager.instance.PlayerStats.GetSetLilyMana += 2f * Time.deltaTime;

                else if (GameManager.instance.PlayerStats.GetSetLilyMana > 100f)
                    GameManager.instance.PlayerStats.GetSetLilyMana = 100f;
            }
            else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            {
                if (GameManager.instance.PlayerStats.GetSetLukasMana < 100f)
                    GameManager.instance.PlayerStats.GetSetLukasMana += 2f * Time.deltaTime;

                else if (GameManager.instance.PlayerStats.GetSetLukasMana > 100f)
                    GameManager.instance.PlayerStats.GetSetLukasMana = 100f;
            }
        }
    }
}
