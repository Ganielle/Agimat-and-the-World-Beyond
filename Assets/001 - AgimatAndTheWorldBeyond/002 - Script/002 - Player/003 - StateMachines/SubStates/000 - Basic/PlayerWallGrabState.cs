           using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 holdPosition;

    public PlayerWallGrabState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
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

        SettingsSetter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HoldPosition();
        AnimationChanger();
    }

    private void SettingsSetter()
    {

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.WALLGRAB;

        holdPosition = statemachineController.transform.position;
        HoldPosition();
    }

    private void AnimationChanger()
    {
        if (isTouchingWall && !isTouchingLedge &&
            GameManager.instance.gameplayController.grabWallInput && !isGrounded)
            statemachineChanger.ChangeState(statemachineController.ledgeClimbState);

        if (!isExitingState)
        {
            if (!GameManager.instance.gameplayController.grabWallInput)
                statemachineChanger.ChangeState(statemachineController.inAirState);

            else if (GameManager.instance.gameplayController.movementNormalizeY == 1f)
                statemachineChanger.ChangeState(statemachineController.wallClimbState);

            else if (GameManager.instance.gameplayController.jumpInput)
                statemachineChanger.ChangeState(statemachineController.wallJumpState);

            else if (GameManager.instance.gameplayController.movementNormalizeY == -1f &&
                GameManager.instance.gameplayController.grabWallInput && !isGrounded)
                statemachineChanger.ChangeState(statemachineController.wallSlideState);

        }
    }

    private void HoldPosition()
    {
        statemachineController.transform.position = holdPosition;

        statemachineController.core.SetVelocityX(0f,
                    statemachineController.core.GetCurrentVelocity.y);
        statemachineController.core.SetVelocityY(0f);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
