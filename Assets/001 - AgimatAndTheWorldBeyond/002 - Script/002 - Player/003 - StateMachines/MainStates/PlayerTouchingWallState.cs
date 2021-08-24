using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerStatesController
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingWallBack;
    protected bool isTouchingLedge;

    public PlayerTouchingWallState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
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

        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        isTouchingWall = statemachineController.core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingLedge = statemachineController.core.groundPlayerController.CheckIfTouchingLedge;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded && !GameManager.instance.gameplayController.grabWallInput)
            statemachineChanger.ChangeState(statemachineController.idleState);

        else if (!isTouchingWall || (GameManager.instance.gameplayController.GetSetMovementNormalizeX !=
            statemachineController.core.GetFacingDirection &&
            !GameManager.instance.gameplayController.grabWallInput))
            statemachineChanger.ChangeState(statemachineController.inAirState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
