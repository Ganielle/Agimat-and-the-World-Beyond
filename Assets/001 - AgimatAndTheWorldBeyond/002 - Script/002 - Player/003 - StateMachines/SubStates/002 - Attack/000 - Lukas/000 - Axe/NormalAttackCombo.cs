using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackCombo : PlayerGroundAttackState
{
    int currentAttackIndex;
    int lastCurrentCheckAttackIndex;

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

            statemachineController.core.attackController.canNextAttack = true;
            statemachineController.core.attackController.currentAttacking = false;
            statemachineController.core.attackController.onLastAttackCombo = false;

            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                currentAttackIndex);

            statemachineController.core.attackController.parameter = "";
        }
        else
        {
            //statemachineController.core.attackController.canNextAttack = false;
            //statemachineController.core.attackController.currentAttacking = false;

            //if (lastCurrentCheckAttackIndex == currentAttackIndex)
            //{
            //    currentAttackIndex = 0;

            //    GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
            //        currentAttackIndex);
            //}
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
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

        if (!statemachineController.core.attackController.onLastAttackCombo)
        {
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetInteger(statemachineController.core.attackController.parameter,
                currentAttackIndex);

            Debug.Log("can next attack");

            if (statemachineController.core.attackController.canNextAttack && GameManager.instance.gameplayController.attackInput)
            {
                Debug.Log("attacking next");
                statemachineController.core.attackController.canNextAttack = false;
                GameManager.instance.gameplayController.UseAttackInput();
                lastCurrentCheckAttackIndex = statemachineController.core.attackController.attackComboIndex;
                statemachineController.core.attackController.attackComboIndex++;
                currentAttackIndex = statemachineController.core.attackController.attackComboIndex;
            }
        }
    }

    public void SetComboIndexParameter(string parameter) => statemachineController.core.attackController.parameter = parameter;
}
