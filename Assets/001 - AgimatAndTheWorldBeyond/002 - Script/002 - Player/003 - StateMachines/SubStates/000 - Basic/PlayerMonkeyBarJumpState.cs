using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkeyBarJumpState : PlayerAbilityState
{
    public PlayerMonkeyBarJumpState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.MONKEYBARJUMP;

        statemachineController.core.SetVelocityY(statemachineController.core.playerRawData.monkeyBarJumpVelocity);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (isTouchingWall && !isTouchingLedge)
            statemachineController.ledgeClimbState.SetDetectedPosition(statemachineController.transform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("yVelocity",
            statemachineController.core.GetCurrentVelocity.y);
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
            Mathf.Abs(statemachineController.core.GetCurrentVelocity.x));

        if (!isExitingState)
        {
            if (Time.time >= startTime + 0.05f)
                isAbilityDone = true;
        }
    }
}
