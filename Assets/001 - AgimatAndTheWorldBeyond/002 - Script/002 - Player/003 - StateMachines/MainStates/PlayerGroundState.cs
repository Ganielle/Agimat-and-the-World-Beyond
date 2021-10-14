using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerStatesController
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isFootTouchGround;
    protected bool isFrontFootTouchSlope;
    protected bool isFrontFootTouchDefaultGround;
    protected Vector2 checkPos;

    private bool isTouchingLedge;

    public PlayerGroundState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, 
        string animBoolName, bool isBoolAnim) : base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        isFrontFootTouchDefaultGround = statemachineController.core.groundPlayerController.CheckIfFrontFootTouchDefaultGround;
        isFootTouchGround = statemachineController.core.groundPlayerController.CheckIfFrontFootTouchGround;
        isFrontFootTouchSlope = statemachineController.core.groundPlayerController.CheckIfFrontTouchingSlope;
        isTouchingWall = statemachineController.core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingLedge = statemachineController.core.groundPlayerController.CheckIfTouchingLedge;
        checkPos = statemachineController.transform.position - (Vector3)(new Vector2(0f,
            statemachineController.core.colliderSize.y / 2));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();

        statemachineController.core.weaponChangerController.DoneSwitchingWeapon();
        statemachineController.core.weaponChangerController.SwitchWeapon();
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            if (!isGrounded)
                statemachineChanger.ChangeState(statemachineController.inAirState);

            else if (isGrounded && GameManager.instance.gameplayController.sprintTapCount == 2
                && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND &&
                GameManager.instance.PlayerStats.GetSetCurrentStamina > 0f)
                statemachineChanger.ChangeState(statemachineController.playerSprintState);

            // TODO: coyote time for JumpState
            else if (isTouchingWall && GameManager.instance.gameplayController.grabWallInput &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND && isTouchingLedge &&
                GameManager.instance.PlayerStats.GetSetCurrentStamina >=
                movementData.wallGrabHoldStamina)
                statemachineChanger.ChangeState(statemachineController.wallGrabState);

            else if (statemachineController.playerDashState.CheckIfCanDash() &&
                statemachineController.core.groundPlayerController.canWalkOnSlope &&
                GameManager.instance.gameplayController.dashInput &&
                !GameManager.instance.gameplayController.switchPlayerLeftInput &&
                (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.LOOKINGDOWN && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.LOOKINGUP))
                statemachineChanger.ChangeState(statemachineController.playerDashState);
        }
    }
}
