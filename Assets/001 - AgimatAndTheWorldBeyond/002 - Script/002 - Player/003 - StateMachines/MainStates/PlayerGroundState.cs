using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerStatesController
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isFootTouchGround;
    protected bool canJump;
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

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();
        SwitchWeapon();
        DoneSwitchingWeapon();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        SlopeChecker(); 

        //  This is for near ledge
        if (!isFootTouchGround && !statemachineController.core.groundPlayerController.isOnSlope)
        {
            statemachineController.core.SetVelocityX(movementData.
                pushForcePlayerWhenFootNotTouchingGround *
                statemachineController.core.GetFacingDirection,
                statemachineController.core.GetCurrentVelocity.y);
        }
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

    #region SLOPE

    private void SlopeChecker()
    {
        statemachineController.core.groundPlayerController.CheckSlopeVertical(checkPos);
        statemachineController.core.groundPlayerController.CheckSlopeHorizontal(checkPos);

        if (isGrounded && statemachineController.core.groundPlayerController.slopeDownAngle <=
            statemachineController.core.groundPlayerController.maxSlopeAngle)
            canJump = true;
        else
            canJump = false;
    }

    public void StickToSlopeLanding()
    {
        Vector2 currentVelocity = statemachineController.core.GetCurrentVelocity;
        Vector2 feetPosAfterTick = (Vector2) statemachineController.transform.position +
            (Vector2) statemachineController.core.groundPlayerController.groundCheck.position +
            currentVelocity * Time.deltaTime;

        RaycastHit2D groundCheckAfterTick = Physics2D.Raycast(feetPosAfterTick +
            Vector2.up * movementData.maxFloorCheckDist,
            -Vector2.up, movementData.maxFloorCheckDist * 2f,
            statemachineController.core.groundPlayerController.whatIsGround);

        if (groundCheckAfterTick.collider != null)
        {
            Vector2 wantedFeetPosAfterTick = groundCheckAfterTick.point;

            Debug.Log("detect ground");

            if (wantedFeetPosAfterTick != feetPosAfterTick)
            {

                RaycastHit2D rampCornerCheck = Physics2D.Raycast(
                        wantedFeetPosAfterTick
                        - movementData.floorCheckOffsetHeight * Vector2.up
                        - movementData.floorCheckOffsetWidth * Mathf.Sign(currentVelocity.x) * Vector2.right,
                        Mathf.Sign(currentVelocity.x) * Vector2.right);

                if (rampCornerCheck.collider != null)
                {
                    // put feet at x=corner position
                    Vector2 cornerPos = new Vector2(rampCornerCheck.point.x,
                            wantedFeetPosAfterTick.y);

                    statemachineController.core.playerRB.position = cornerPos -
                        movementData.feetOffset;

                    Vector2 wantedVelocity = (wantedFeetPosAfterTick - cornerPos)
                        / Time.deltaTime;

                    statemachineController.core.SetVelocityX(wantedVelocity.x,
                        wantedVelocity.y);
                }
                Debug.Log(wantedFeetPosAfterTick + "   " + feetPosAfterTick);
            }
        }
    }

    #endregion

    #region WEAPON SWITCHING

    private void SwitchWeapon()
    {
        if (statemachineController.weaponSwitchState.CheckIfCanWeaponSwitch() &&
            GameManager.instance.gameInputController.canSwitchWeapon &&
            GameManager.instance.gameInputController.GetWeaponSwitchInput == 2)
        {
            statemachineController.core.ChangeWeapon();

            if (!isExitingState)
            {
                if (isGrounded &&
                    (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo ==
                    PlayerStats.AnimatorStateInfo.IDLE ||
                    GameManager.instance.PlayerStats.GetSetAnimatorStateInfo ==
                    PlayerStats.AnimatorStateInfo.SWITCHWEAPON))
                {
                    statemachineController.weaponSwitchState.animBoolName =
                        GameManager.instance.PlayerStats.GetSetWeaponEquipBoolInPlayerAnim;
                    statemachineChanger.ChangeState(statemachineController.weaponSwitchState);
                }
            }

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
