using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats
{
    public enum PlayerCharacter
    {
        NONE,
        LUKAS,
        LILY
    }
    private event EventHandler playerCharacterChange;
    public event EventHandler onPlayerCharacterChange
    {
        add
        {
            if (playerCharacterChange == null || !playerCharacterChange.GetInvocationList().Contains(value))
                playerCharacterChange += value;
        }
        remove { playerCharacterChange -= value; }
    }
    PlayerCharacter playerCharacter;
    public PlayerCharacter GetSetPlayerCharacter
    {
        get { return playerCharacter; }
        set
        {
            playerCharacter = value;
            playerCharacterChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum PlayerState
    {
        NONE,
        ALIVE,
        DEAD
    }
    private event EventHandler playerStateChange;
    public event EventHandler onPlayerStateChange
    {
        add
        {
            if (playerStateChange == null || !playerStateChange.GetInvocationList().Contains(value))
                playerStateChange += value;
        }
        remove { playerStateChange -= value; }
    }
    PlayerState playerState;
    public PlayerState GetSetPlayerState
    {
        get { return playerState; }
        set
        {
            playerState = value;
            playerStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum PlayerBattleState
    {
        NONE,
        ADVENTURING,
        BATTLE
    }
    private event EventHandler playerBattleStateChange;
    public event EventHandler onPlayerBattleStateChange
    {
        add
        {
            if (playerBattleStateChange == null || !playerBattleStateChange.GetInvocationList().Contains(value))
                playerBattleStateChange += value;
        }
        remove { playerBattleStateChange -= value; }
    }
    PlayerBattleState playerBattleState;
    public PlayerBattleState GetSetBattleState
    {
        get { return playerBattleState; }
        set
        {
            playerBattleState = value;
            playerBattleStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler playerCharacterObjChange;
    public event EventHandler onPlayerCharacterObjChange
    {
        add
        {
            if (playerCharacterObjChange == null || !playerCharacterObjChange.GetInvocationList().Contains(value))
                playerCharacterObjChange += value;
        }
        remove { playerCharacterObjChange -= value; }
    }
    GameObject playerCharacterObj;
    public GameObject GetSetPlayerCharacterObj
    {
        get { return playerCharacterObj; }
        set
        {
            playerCharacterObj = value;
            playerCharacterObjChange?.Invoke(this, EventArgs.Empty);
        }
    }

    Collider2D playerCollider;
    public Collider2D GetSetPlayerCollider
    {
        get { return playerCollider; }
        set { playerCollider = value; }
    }

    #region WEAPONS

    string weaponEquipBoolInPlayerAnim;
    public string GetSetWeaponEquipBoolInPlayerAnim
    {
        get { return weaponEquipBoolInPlayerAnim; }
        set { weaponEquipBoolInPlayerAnim = value; }
    }

    #endregion

    #region SPRITES

    SpriteRenderer playerSR;
    public SpriteRenderer GetSetPlayerSR
    {
        get { return playerSR; }
        set { playerSR = value; }
    }

    #endregion

    #region HEALTH

    private event EventHandler heatlhChange;
    public event EventHandler onHealthChange
    {
        add
        {
            if (heatlhChange == null || !heatlhChange.GetInvocationList().Contains(value))
                heatlhChange += value;
        }
        remove { heatlhChange -= value; }
    }
    float currentHealth, lukasHealth, lilyHealth;
    public float GetSetCurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            heatlhChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public float GetSetLukasHealth
    {
        get { return lukasHealth; }
        set { lukasHealth = value; }
    }
    public float GetSetLilyHealth
    {
        get { return lilyHealth; }
        set { lilyHealth = value; }
    }

    #endregion

    #region MANA

    private event EventHandler manaChange;
    public event EventHandler onManaChange
    {
        add
        {
            if (manaChange == null ||
                !manaChange.GetInvocationList().Contains(value))
                manaChange += value;
        }
        remove { manaChange -= value; }
    }
    float chargeMana, currentMana, lukasMana, lilyMana;
    public float GetSetCurrentMana
    {
        get { return currentMana; }
        set 
        { 
            currentMana = value;
            manaChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public float GetSetLukasMana
    {
        get { return lukasMana; }
        set { lukasMana = value; }
    }
    public float GetSetLilyMana
    {
        get { return lilyMana; }
        set { lilyMana = value; }
    }
    public float GetSetChargeMana
    {
        get { return chargeMana; }
        set { chargeMana = value; }
    }
    #endregion

    #region STAMINA

    private event EventHandler staminaChange;
    public event EventHandler onStaminaChange
    {
        add
        {
            if (staminaChange == null || !staminaChange.GetInvocationList().Contains(value))
                staminaChange += value;
        }
        remove { staminaChange -= value; }
    }
    float currentStamina, lukasCurrentStamina, lilyCurrentStamina;
    public float GetSetCurrentStamina
    {
        get { return currentStamina; }
        set
        {
            currentStamina = value;
            staminaChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public float GetSetLukasStamina
    {
        get { return lukasCurrentStamina; }
        set { lukasCurrentStamina = value; }
    }
    public float GetSetLilyStamina
    {
        get { return lilyCurrentStamina; }
        set { lilyCurrentStamina = value; }
    }

    #endregion

    #region ANIMATOR

    public enum AnimatorStateInfo
    {
        NONE,
        IDLE,
        TAUNTIDLE,
        LOOKINGUP,
        LOOKINGDOWN,
        RUNNING,
        RUNNINGBREAK,
        JUMPING,
        FALLING,
        LOWLAND,
        HIGHLAND,
        WALLGRAB,
        WALLCLIMB,
        WALLSLIDE,
        WALLJUMP,
        LEDGEHOLD,
        LEDGECLIMB,
        MONKEYBARGRAB,
        MONKEYBARMOVE,
        MONKEYBARJUMP,
        ROPEGRAB,
        ROPECLIMB,
        ROPESLIDE,
        ROPESWING,
        ROPEJUMP,
        SPRINT,
        DASHCHARGE,
        DASHBURST,
        DODGE,
        SWITCHING,
        SWITCHWEAPON,
        CHANGEIDLEDIRECTION,
        NEARLEDGE,
        STEEPSLOPE,
        AXEATTACK
    }
    AnimatorStateInfo animStateInfo, lastAnimStateInfo;
    private event EventHandler animatorStateInfoChange;
    public event EventHandler onAnimatorStateInfoChange
    {
        add
        {
            if (animatorStateInfoChange == null || !animatorStateInfoChange.GetInvocationList().Contains(value))
                animatorStateInfoChange += value;
        }
        remove { animatorStateInfoChange -= value; }
    }
    public AnimatorStateInfo GetSetAnimatorStateInfo
    {
        get { return animStateInfo; }
        set
        {
            lastAnimStateInfo = animStateInfo;
            animStateInfo = value;
            animatorStateInfoChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public AnimatorStateInfo GetSetLastAnimatorStateInfo
    {
        get { return lastAnimStateInfo; }
        set { lastAnimStateInfo = value; }
    }

    Animator playerAnimator;
    public Animator GetSetPlayerAnimator
    {
        get { return playerAnimator; }
        set { playerAnimator = value; }
    }

    #endregion

    #region SKILL SET

    //private event EventHandler skillListChange;
    //public event EventHandler onSkillListChange
    //{
    //    add
    //    {
    //        if (skillListChange == null || !skillListChange.GetInvocationList().Contains(value))
    //            skillListChange += value;
    //    }
    //    remove { skillListChange -= value; }
    //}
    //List<Skill> skillList;
    //public void SetSkillList(Skill skillData)
    //{
    //    if (skillList.Contains(skillData))
    //        return;

    //    GetSkillList.Add(skillData);
    //    skillListChange?.Invoke(this, EventArgs.Empty);
    //}
    //public List<Skill> GetSkillList
    //{
    //    get { return skillList; }
    //}

    #endregion
}
