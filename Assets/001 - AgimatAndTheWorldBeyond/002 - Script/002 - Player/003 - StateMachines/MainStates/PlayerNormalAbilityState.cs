using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAbilityState : PlayerStatesController
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingClimbWall;
    protected bool isTouchingLedge;
    protected bool isSameHeightToPlatform;
    protected bool isFootTouchGround;
    protected Vector2 checkSlopePos;

    public PlayerNormalAbilityState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
    }


    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        isTouchingWall = statemachineController.core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingClimbWall = statemachineController.core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingLedge = statemachineController.core.groundPlayerController.CheckIfTouchingLedge;
        isFootTouchGround = statemachineController.core.groundPlayerController.CheckIfFrontFootTouchGround;
        checkSlopePos = statemachineController.transform.position - (Vector3)(new Vector2(0f,
            statemachineController.core.colliderSize.y / 2));
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone)
        {
            if (isGrounded && statemachineController.core.GetCurrentVelocity.y < 0.01f)
            {
                statemachineChanger.ChangeState(statemachineController.idleState);
            }
            else
            {
                statemachineChanger.ChangeState(statemachineController.inAirState);
            }
        }
    }
}
