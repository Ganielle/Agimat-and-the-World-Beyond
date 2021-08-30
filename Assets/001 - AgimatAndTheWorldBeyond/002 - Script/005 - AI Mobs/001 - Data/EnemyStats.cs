using System;
using System.Linq;
using UnityEngine;

public class EnemyStats
{
    #region GENERIC ENEMY STATS
    //  For the type of enemy mob
    public enum EnemyCharacter
    {
        NONE,
        DIWATA,
        GINGER
    }
    EnemyCharacter enemyCharacter;
    public EnemyCharacter GetSetEnemyCharacter
    {
        get { return enemyCharacter; }
        set 
        {
            enemyCharacter = value;
        }
    }

    //  For the life status of the enemy mob
    public enum EnemyState
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
    EnemyState playerState;
    public EnemyState GetSetPlayerState
    {
        get { return playerState; }
        set
        {
            playerState = value;
            playerStateChange?.Invoke(this, EventArgs.Empty);
        }
    }
    #endregion

    #region SPRITES

    SpriteRenderer enemySR;
    public SpriteRenderer GetSetEnemySR
    {
        get { return enemySR; }
        set { enemySR = value; }
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
    float currentHealth, startingHealth;
    public float GetSetCurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;
            heatlhChange?.Invoke(this, EventArgs.Empty);
        }
    }
    public float GetSetStartingHealth
    {
        get { return startingHealth; }
        set { startingHealth = value; }
    }

    #endregion

    #region ANIMATOR

    public enum AnimatorStateInfo
    {
        NONE,
        PATROLLING,
        CLIMBING,
        CHASING, 
        ATTACKING
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

    Animator enemyAnimator;
    public Animator GetSetEnemyAnimator
    {
        get { return enemyAnimator; }
        set { enemyAnimator = value; }
    }

    #endregion
}
