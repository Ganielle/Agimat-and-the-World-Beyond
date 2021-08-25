using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBasicAttackState : PlayerGroundAttackState
{
    private bool canNextAttack;
    private int lastCurrentAttackIndex;

    public AxeBasicAttackState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (lastCurrentAttackIndex == statemachineController.core.attackComboIndex || 
            statemachineController.core.onLastAttackCombo)
        {
            statemachineController.core.onLastAttackCombo = false;
            statemachineController.core.attackComboIndex = 0;
            canNextAttack = false;
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        canNextAttack = true;
        lastCurrentAttackIndex = statemachineController.core.attackComboIndex;
    }

    public override void Enter()
    {
        base.Enter();

        statemachineController.core.SetVelocityZero();

        //  This is to populate the first attack combo index
        lastCurrentAttackIndex = statemachineController.core.attackComboIndex;

        statemachineController.core.ChangeBattleState();
    }

    public override void Exit()
    {
        base.Exit();

        canNextAttack = false;
        statemachineController.core.onLastAttackCombo = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger("axeAttackCombo", statemachineController.core.attackComboIndex);

        if (GameManager.instance.gameplayController.attackInput && canNextAttack)
        {
            canNextAttack = false;
            lastCurrentAttackIndex = statemachineController.core.attackComboIndex;
            statemachineController.core.attackComboIndex++;
            statemachineController.core.ChangeBattleState();
            GameManager.instance.gameplayController.UseAttackInput();
        }
    }
}
