using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackCombo : PlayerGroundAttackState
{
    int currentAttackIndex;
    int lastCurrentCheckAttackIndex;

    //  TO FIX COMBO SYSTEM

    public NormalAttackCombo(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        canTransition = true;

        if (statemachineController.core.attackController.onLastAttackCombo)
        {
            currentAttackIndex = 0;
            statemachineController.core.attackController.attackComboIndex = 0;

            statemachineController.core.attackController.canNextAttack = false;
            statemachineController.core.attackController.currentAttacking = false;
            statemachineController.core.attackController.onLastAttackCombo = false;

            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                currentAttackIndex);

            statemachineController.core.attackController.parameter = "";
        }
        else
        {
            statemachineController.core.attackController.canNextAttack = false;
            statemachineController.core.attackController.currentAttacking = false;

            if (lastCurrentCheckAttackIndex == currentAttackIndex)
            {
                currentAttackIndex = 0;

                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                    currentAttackIndex);
            }
            else
            {
                lastCurrentCheckAttackIndex++;
            }
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        statemachineController.core.attackController.canNextAttack = true;
    }

    public override void Enter()
    {
        base.Enter();

        statemachineController.core.SetVelocityZero();

        statemachineController.core.attackController.currentAttacking = true;

        currentAttackIndex = statemachineController.core.attackController.attackComboIndex;

        lastCurrentCheckAttackIndex = statemachineController.core.attackController.attackComboIndex;

        statemachineController.core.ChangeBattleState();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (statemachineController.core.attackController.canNextAttack && GameManager.instance.gameplayController.attackInput)
        {
            statemachineController.core.attackController.canNextAttack = false;
            statemachineController.core.attackController.currentAttacking = true;
            lastCurrentCheckAttackIndex = statemachineController.core.attackController.attackComboIndex;
            statemachineController.core.attackController.attackComboIndex++;
            Debug.Log("Attack index plus plus can next attack");
            currentAttackIndex = statemachineController.core.attackController.attackComboIndex;
            GameManager.instance.gameplayController.UseAttackInput();
        }

        if (!statemachineController.core.attackController.onLastAttackCombo)
        {
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                currentAttackIndex);
        }
    }

    public void SetComboIndexParameter(string parameter) => statemachineController.core.attackController.parameter = parameter;
}
