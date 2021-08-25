using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    private event EventHandler showStaminaChange;
    private event EventHandler onShowStaminaChange
    {
        add
        {
            if (showStaminaChange == null || !showStaminaChange.GetInvocationList().Contains(value))
                showStaminaChange += value;
        }
        remove { showStaminaChange -= value; }
    }
    bool GetSetShowStaminaStats
    {
        get { return showStaminaUI; }
        set
        {
            showStaminaUI = value;
            showStaminaChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  =================================================

    [SerializeField] private PlayerCore core;

    [Space]
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private float animationSpeed;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private CanvasGroup staminaCG;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] float staminaOnEnable;
    [ReadOnly] [SerializeField] float currentStamina;
    [ReadOnly] [SerializeField] bool showStaminaUI;
    [ReadOnly] public bool canRecover;
    [ReadOnly] public float dodgeStaminaPercentage;
    [ReadOnly] public float dashStaminaPercentage;
    [ReadOnly] public float wallJumpStaminaPercentage;
    [ReadOnly] public float lastStaminaRecoverEnter;

    private void OnEnable()
    {
        StaminaSkillPercentageSetter();

        staminaOnEnable = GameManager.instance.PlayerStats.GetSetCurrentStamina
            / 100f;

        staminaSlider.value = staminaOnEnable ;

        CheckStaminaToShow();
        ShowStaminaAnimation();

        onShowStaminaChange += ShowStaminaCheck;
        GameManager.instance.PlayerStats.onAnimatorStateInfoChange += AnimationChange;
    }

    private void OnDisable()
    {
        onShowStaminaChange -= ShowStaminaCheck;
        GameManager.instance.PlayerStats.onAnimatorStateInfoChange -= AnimationChange;
    }

    void Update()
    {
        UpdateStaminaWhenCharacterIsHidden();
        CheckStaminaToShow();
        StaminaSliderValue();
    }


    #region PLAYER STATS

    public void StaminaSkillPercentageSetter()
    {
        dodgeStaminaPercentage = (float)GameManager.instance.PlayerStats.GetSetCurrentStamina
            * core.playerRawData.dodgePercentage;
        dashStaminaPercentage = (float)GameManager.instance.PlayerStats.GetSetCurrentStamina
            * core.playerRawData.dashPercentage;
        wallJumpStaminaPercentage = (float)GameManager.instance.PlayerStats.GetSetCurrentStamina
            * core.playerRawData.wallJumpPercentage;
    }

    public void StaminaReducer(float reducer, bool reduceOverTime)
    {
        if (lastStaminaRecoverEnter > 0f)
            lastStaminaRecoverEnter = 0f;

        if (reduceOverTime)
        {
            GameManager.instance.PlayerStats.GetSetCurrentStamina -=
                reducer * Time.deltaTime;
            StaminaUpdateCharacter();
        }
        else
        {
            GameManager.instance.PlayerStats.GetSetCurrentStamina -= reducer;
            StaminaUpdateCharacter();
        }
    }

    public void StaminaAdder()
    {
        if (canRecover)
        {
            if (Time.time >= lastStaminaRecoverEnter + core.playerRawData.staminaRecoveryDelay)
            {
                GameManager.instance.PlayerStats.GetSetCurrentStamina +=
                    core.playerRawData.staminaRecoverWhenActive * Time.deltaTime;

                StaminaUpdateCharacter();

                if (GameManager.instance.PlayerStats.GetSetCurrentStamina >= 100f)
                {
                    canRecover = false;
                    lastStaminaRecoverEnter = 0f;
                    GameManager.instance.PlayerStats.GetSetCurrentStamina = 100f;
                    StaminaUpdateCharacter();
                }
            }
        }
    }

    private void StaminaUpdateCharacter()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetLukasStamina =
                GameManager.instance.PlayerStats.GetSetCurrentStamina;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetLilyStamina =
                GameManager.instance.PlayerStats.GetSetCurrentStamina;
    }

    #endregion

    #region STAMINA VALUE

    private void UpdateStaminaWhenCharacterIsHidden()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LUKAS)
        {
            if (GameManager.instance.PlayerStats.GetSetLilyStamina < 100f)
            {
                GameManager.instance.PlayerStats.GetSetLilyStamina +=
                    core.playerRawData.staminaRecoverWhenNotActive *
                    Time.deltaTime;
            }
            else
            {
                GameManager.instance.PlayerStats.GetSetLilyStamina = 100f;
            }
        }
        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter ==
            PlayerStats.PlayerCharacter.LILY)
        {
            if (GameManager.instance.PlayerStats.GetSetLukasStamina < 100f)
            {
                GameManager.instance.PlayerStats.GetSetLukasStamina +=
                    core.playerRawData.staminaRecoverWhenNotActive *
                    Time.deltaTime;
            }
            else
            {
                GameManager.instance.PlayerStats.GetSetLukasStamina = 100f;
            }
        }
    }

    #endregion

    #region STAMINA UI

    private void AnimationChange(object sender, EventArgs e)
    {
        if (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo ==
             PlayerStats.AnimatorStateInfo.SWITCHING)
            GetSetShowStaminaStats = false;
    }

    private void ShowStaminaCheck(object sender, EventArgs e)
    {
        ShowStaminaAnimation();
    }

    private void CheckStaminaToShow()
    {
        if (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
             PlayerStats.AnimatorStateInfo.SWITCHING)
        {
            if (GetSetShowStaminaStats)
            {
                if (staminaSlider.value >= 1f)
                    GetSetShowStaminaStats = false;
            }
            else
            {
                if (staminaSlider.value < 1f)
                    GetSetShowStaminaStats = true;
            }
        }
    }

    private void StaminaSliderValue()
    {
        currentStamina = GameManager.instance.PlayerStats.GetSetCurrentStamina
            / 100f;

        staminaSlider.value = currentStamina;
    }

    private void ShowStaminaAnimation()
    {
        if (GetSetShowStaminaStats)
        {
            staminaCG.alpha = 0f;
            staminaCG.gameObject.SetActive(true);
            LeanTween.alphaCanvas(staminaCG, 1f, animationSpeed).setEase(easeType);
        }
        else
        {
            staminaCG.alpha = 1f;
            LeanTween.alphaCanvas(staminaCG, 0f, animationSpeed).setEase(easeType).
                setOnComplete(() => {
                    if (!GetSetShowStaminaStats) staminaCG.gameObject.SetActive(false);
                });
        }
    }

    #endregion
}
