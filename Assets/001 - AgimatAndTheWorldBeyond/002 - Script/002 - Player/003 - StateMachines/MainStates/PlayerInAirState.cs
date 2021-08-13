using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerStatesController
{
    private bool isGrounded;
    private bool isTouchingGroundFrontFoot;
    private bool reachMaxJumpHeight;
    private bool isJumping;
    private bool isTouchingClimbWall;
    private bool isTouchingLedge;
    private bool isSameHeightToPlatform;
    private bool isTouchingWall;
    private bool isTouchingMonkeyBar;
    private bool isTouchingRope;

    /// <summary>
    /// TODO : Coyote time when you finish the 
    /// skill sets like double jump
    /// </summary>

    public PlayerInAirState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        isTouchingGroundFrontFoot = statemachineController.core.groundPlayerController.CheckIfFrontFootTouchGround;
        isTouchingLedge = statemachineController.core.groundPlayerController.CheckIfTouchingLedge;
        isSameHeightToPlatform = statemachineController.core.groundPlayerController.PlayerToPlatformHeightCheck;
        isTouchingClimbWall = statemachineController.core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingWall = statemachineController.core.groundPlayerController.CheckIfTouchWall;
        isTouchingMonkeyBar = statemachineController.core.CheckIfTouchingMonkeyBar;
        isTouchingRope = statemachineController.core.ropePlayerController.CheckIfTouchingRope;

        statemachineController.core.groundPlayerController.PhysicsMaterialChanger(movementData.noFriction);

        if (isTouchingClimbWall && !isTouchingLedge)
            statemachineController.ledgeClimbState.SetDetectedPosition(statemachineController.transform.position);
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

        SwitchWeapon();
        DoneSwitchingWeapon();

        AnimationChanger();
        CheckAnimationState();
        CheckIfReachMaxVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        HighLowJump();
        MovePlayerWhileInAir();
    }

    #region AIR STATE FUNCTIONS

    private void CheckAnimationState()
    {
        if (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
            PlayerStats.AnimatorStateInfo.WALLJUMP)
        {
            if (!isGrounded && statemachineController.core.GetCurrentVelocity.y >= 0.01f)
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.JUMPING;
            else if (!isGrounded && statemachineController.core.GetCurrentVelocity.y < 0.01f)
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.FALLING;
        }
    }

    private void AnimationChanger()
    {
        if (isGrounded && statemachineController.core.GetCurrentVelocity.y < 1f)
        {
            if (!reachMaxJumpHeight)
                statemachineChanger.ChangeState(statemachineController.lowLandState);
            else
            {
                statemachineChanger.ChangeState(statemachineController.highLandState);
                reachMaxJumpHeight = false;
            }
        }

        //  TODO : For Double Jump (Jump State)
        //else if (GameManager.instance.gameInputController.JumpInput &&
        //    GameManager.instance.JumpState.CanJump)
        //{

        //}

        //  Wall Climb && Ledge

        else if (statemachineController.ledgeClimbState.CheckIfCanLedgeClimb()
            && isSameHeightToPlatform && isTouchingClimbWall && !isTouchingLedge)
            statemachineChanger.ChangeState(statemachineController.ledgeClimbState);

        else if (isTouchingClimbWall && isSameHeightToPlatform &&
            GameManager.instance.gameInputController.GetSetMovementNormalizeX ==
                statemachineController.core.GetFacingDirection &&
                !GameManager.instance.gameInputController.jumpInput &&
                statemachineController.core.GetCurrentVelocity.y < 0.01f)
            statemachineChanger.ChangeState(statemachineController.wallSlideState);
        else if (isTouchingClimbWall && isSameHeightToPlatform &&
            GameManager.instance.gameInputController.grabWallInput)
            statemachineChanger.ChangeState(statemachineController.wallGrabState);

        //  Monkey Bar
        else if (isTouchingMonkeyBar &&
            GameManager.instance.gameInputController.grabMonkeyBarInput)
            statemachineChanger.ChangeState(statemachineController.monkeyBarGrab);

        //  Rope
        else if (isTouchingRope &&
            GameManager.instance.gameInputController.ropeInput)
            statemachineChanger.ChangeState(statemachineController.ropeStartGrab);

        //  Dash
        else if (GameManager.instance.gameInputController.dashInput &&
            statemachineController.playerDashState.CheckIfCanDash())
            statemachineChanger.ChangeState(statemachineController.playerDashState);

        else
            statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameInputController.GetSetMovementNormalizeX);
    }

    private void HighLowJump()
    {
        if (isJumping)
        {
            if (GameManager.instance.gameInputController.jumpInputStop)
            {
                statemachineController.core.SetVelocityY(statemachineController
                    .core.GetCurrentVelocity.y *
                    movementData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (statemachineController.core.GetCurrentVelocity.y <= 0f)
                isJumping = false;
        }
    }

    private void MovePlayerWhileInAir()
    {
        if (isGrounded || isTouchingWall || isTouchingGroundFrontFoot)
            return;

        statemachineController.core.SetVelocityX(movementData.movementSpeed *
            GameManager.instance.gameInputController.GetSetMovementNormalizeX,
                    statemachineController.core.GetCurrentVelocity.y);

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("yVelocity",
            statemachineController.core.GetCurrentVelocity.y);
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
            Mathf.Abs(statemachineController.core.GetCurrentVelocity.x));
    }

    private void CheckIfReachMaxVelocity()
    {
        if (statemachineController.core.GetCurrentVelocity.y <=
            movementData.maxJumpHeight && !isGrounded)
            reachMaxJumpHeight = true;
    }

    public void SetIsJumping() => isJumping = true;

    #endregion

    #region WEAPON SWITCH


    private void SwitchWeapon()
    {
        if (GameManager.instance.gameInputController.canSwitchWeapon &&
            GameManager.instance.gameInputController.GetWeaponSwitchInput == 2 &&
            statemachineController.weaponSwitchState.CheckIfCanWeaponSwitch())
        {
            statemachineController.core.ChangeWeapon();
            GameManager.instance.gameInputController.UseCanSwitchWeaponInput();
        }
    }

    private void DoneSwitchingWeapon()
    {
        if (Time.time >= statemachineController.lastShowWeaponSlotsTime + 5f
            && GameManager.instance.gameInputController.GetWeaponSwitchInput != 0)
            GameManager.instance.gameInputController.ResetSwitchWeaponInput();
    }

    #endregion
}
