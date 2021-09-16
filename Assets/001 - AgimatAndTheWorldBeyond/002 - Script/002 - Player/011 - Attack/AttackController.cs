using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("SETTINGS")]
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private float delayAttackTime;

    //  BATTLE ATTACK COMBO
    [ReadOnly] public string parameter;
    [ReadOnly] public float enterDelayAttackTime;
    [ReadOnly] public int attackComboIndex;
    [ReadOnly] public bool canNextAttack;
    [ReadOnly] public bool onLastAttackCombo;
    [ReadOnly] public bool currentAttacking;
    [ReadOnly] public bool canChangeDirectionWhileAttacking;
    [ReadOnly] public bool canCancelAnimation;
    [ReadOnly] public bool canEnterDelayAttack;

    private void Update()
    {
        //DelayNextAttackCounter();
    }

    private void DelayNextAttackCounter()
    {
        if (canEnterDelayAttack && !currentAttacking &&!onLastAttackCombo)
        {
            //  This is to reset the attack combo index
            if (Time.time > enterDelayAttackTime + delayAttackTime) 
            {
                attackComboIndex = 0;
                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(parameter, attackComboIndex);
                parameter = "";
                canEnterDelayAttack = false;
            }
        }
    }
}
