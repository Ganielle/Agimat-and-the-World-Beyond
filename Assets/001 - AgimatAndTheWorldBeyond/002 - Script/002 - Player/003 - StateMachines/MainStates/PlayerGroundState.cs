using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerStatesController
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isFootTouchGround;
    protected Vector2 checkPos;

    private bool isTouchingLedge;

    public PlayerGroundState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, 
        string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        isFootTouchGround = statemachineController.core.groundPlayerController.CheckIfFrontFootTouchGround;
        isTouchingWall = statemachineController.core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingLedge = statemachineController.core.groundPlayerController.CheckIfTouchingLedge;
        checkPos = statemachineController.transform.position - (Vector3)(new Vector2(0f,
            statemachineController.core.colliderSize.y / 2));

        statemachineController.core.groundPlayerController.SlopeChecker();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();

        //statemachineController.core.weaponChangerController.DoneSwitchingWeapon();
        //statemachineController.core.weaponChangerController.SwitchWeapon();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //  Slope Calculation
        statemachineController.core.groundPlayerController.CalculateSlopeForward();
        statemachineController.core.groundPlayerController.CalculateGroundAngle();

    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            if (!isGrounded)
                statemachineChanger.ChangeState(statemachineController.inAirState);

            else if (isGrounded && GameManager.instance.gameInputController.sprintTapCount == 2
                && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND &&
                GameManager.instance.PlayerStats.GetSetCurrentStamina > 0f)
                statemachineChanger.ChangeState(statemachineController.playerSprintState);

            // TODO: coyote time for JumpState
            else if (isTouchingWall && GameManager.instance.gameInputController.grabWallInput &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND && isTouchingLedge &&
                GameManager.instance.PlayerStats.GetSetCurrentStamina >=
                movementData.wallGrabHoldStamina)
                statemachineChanger.ChangeState(statemachineController.wallGrabState);

            else if (statemachineController.core.groundPlayerController.canWalkOnSlope &&
                GameManager.instance.gameInputController.dashInput &&
                !GameManager.instance.gameInputController.switchPlayerLeftInput &&
                statemachineController.playerDashState.CheckIfCanDash() && (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.LOOKINGDOWN && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.LOOKINGUP))
                statemachineChanger.ChangeState(statemachineController.playerDashState);
        }
    }
}
