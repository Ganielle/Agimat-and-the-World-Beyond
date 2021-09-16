using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerNormalAbilityState
{
    public PlayerWallJumpState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (isTouchingWall && !isTouchingLedge)
            statemachineController.ledgeClimbState.SetDetectedPosition(statemachineController.transform.position);
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.WALLJUMP;
        
        statemachineController.core.SetVelocityWallJump(movementData.wallJumpVelocity, movementData.wallJumpAngle,
            -statemachineController.core.GetFacingDirection);
        statemachineController.core.CheckIfShouldFlip(-statemachineController.core.GetFacingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //  Animation in air velocity setter
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("yVelocity",
            statemachineController.core.GetCurrentVelocity.y);
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
            Mathf.Abs(statemachineController.core.GetCurrentVelocity.x));

        if (!isExitingState)
        {
            if (Time.time >= startTime + movementData.wallJumpTime)
                isAbilityDone = true;

            //  Monkey bar
            else if (GameManager.instance.gameplayController.grabMonkeyBarInput &&
                statemachineController.core.CheckIfTouchingMonkeyBar)
            {
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.monkeyBarGrab);
            }

            //  Rope
            else if (GameManager.instance.gameplayController.ropeInput &&
                statemachineController.core.ropePlayerController.CheckIfTouchingRope)
            {
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.ropeStartGrab);
            }

            //  For Dash State
            else if (GameManager.instance.gameplayController.dashInput &&
                statemachineController.playerDashState.CheckIfCanDash() &&
                Time.time >= startTime + movementData.delayToUseDash)
            {
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.playerDashState);
            }

            else if (isGrounded && Time.time >= startTime + movementData.delayToCheckForGround)
                isAbilityDone = true;

            //  For ledgeClimb state
            else if (isTouchingWall && !isTouchingLedge &&
                Time.time >= startTime + movementData.delayToCheckForLedge)
            {        
                isAbilityDone = true;
                statemachineChanger.ChangeState(statemachineController.ledgeClimbState);
            }

            else if (isTouchingWall)
                isAbilityDone = true;
        }
    }
}
