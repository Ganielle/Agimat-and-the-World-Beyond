using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool canDash;
    private bool isHolding;
    private float lastDashTime;
    private float angle;

    private Vector2 dashIndirection;
    private Vector3 lastDirection;

    public PlayerDashState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        SettingsSetter();
    }

    public override void Exit()
    {
        base.Exit();

        if (statemachineController.core.GetCurrentVelocity.y > 0)
            statemachineController.core.SetVelocityY(statemachineController.core.GetCurrentVelocity.y *
            movementData.dashEndYMultiplier);
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

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        PlayerDashMove();
        SetPositionAfterTick();
    }

    private void SettingsSetter()
    {
        //  For game controller input
        canDash = false;
        GameManager.instance.gameplayController.UseDashInput();

        //  For Dash
        isHolding = true;

        dashIndirection = Vector2.right * statemachineController.core.GetFacingDirection;

        startTime = Time.time;

        statemachineController.core.dashDirectionIndicator.gameObject.SetActive(true);
    }

    #region ANIMATION

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            ChangeAnimationBaseOnVelocity();

            if (isAbilityDone)
                statemachineController.core.SetVelocityZero();
            else
            {
                if (isHolding)
                    DashCharge();
                else if (!isHolding)
                    DashBurst();
            }
        }
    }

    private void ChangeAnimationBaseOnVelocity()
    {
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("yVelocity",
            statemachineController.core.GetCurrentVelocity.y);
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
            Mathf.Abs(statemachineController.core.GetCurrentVelocity.x));
    }

    #endregion

    #region DASH MECHANICS

    private void PlayerDashMove()
    {
        if (!isHolding)
        {
            statemachineController.core.SetVelocityDash(movementData.dashVelocity,
                dashIndirection);
        }
    }

    private void SetPositionAfterTick()
    {
        if (!isHolding && DashRotation() == 0 && isGrounded)
        {
            Vector2 feetOffset = new Vector2(0f, statemachineController.core.feetOffsetCollider.bounds.min.y -
                statemachineController.core.playerRB.position.y);
            Vector2 feetPosAfterTick = (Vector2)statemachineController.transform.position + feetOffset +
                statemachineController.core.GetCurrentVelocity * Time.deltaTime;

            float maxFloorCheckDist = 1.0f;

            RaycastHit2D groundCheckAfterTick = Physics2D.Raycast(feetPosAfterTick + Vector2.up *
                maxFloorCheckDist, Vector2.down, maxFloorCheckDist * movementData.slopeCheckDistance, 
                statemachineController.core.groundPlayerController.whatIsGround);

            float angle = Vector2.Angle(Physics2D.Raycast(feetPosAfterTick + Vector2.up *
                maxFloorCheckDist, Vector2.down, maxFloorCheckDist * movementData.slopeCheckDistance,
                statemachineController.core.groundPlayerController.whatIsGround).normal, -statemachineController.transform.up);


            if (groundCheckAfterTick && angle < statemachineController.core.groundPlayerController.minimumSlopeAngle && angle >=
                statemachineController.core.groundPlayerController.maxSlopeAngle)
            {
                Vector2 wantedFeetPosAfterTick = groundCheckAfterTick.point;

                if (wantedFeetPosAfterTick != feetPosAfterTick)
                {
                    //statemachineController.core.SetVelocityZero();

                    // look for corner of ramp+landing. 
                    // Offsets ensure we don't raycast from inside/above it
                    float floorCheckOffsetHeight = 0.25f;
                    float floorCheckOffsetWidth = 0.5f;
                    RaycastHit2D rampCornerCheck = Physics2D.Raycast(
                            wantedFeetPosAfterTick
                            - floorCheckOffsetHeight * Vector2.up
                            - floorCheckOffsetWidth * Mathf.Sign(statemachineController.core.GetCurrentVelocity.x) * Vector2.right,
                            Mathf.Sign(statemachineController.core.GetCurrentVelocity.x) * Vector2.right,
                            statemachineController.core.groundPlayerController.whatIsGround);

                    if (rampCornerCheck.collider != null)
                    {
                        // put feet at x=corner position
                        Vector2 cornerPos = new Vector2(rampCornerCheck.point.x,
                                wantedFeetPosAfterTick.y);

                        statemachineController.core.playerRB.position = cornerPos
                            - feetOffset;
                        // adjust velocity so that physics will take them from corner 
                        // to landing position
                        Vector2 wantedVelocity = (wantedFeetPosAfterTick - cornerPos)
                                / Time.fixedDeltaTime;

                        statemachineController.core.playerRB.velocity = wantedVelocity;
                    }
                }
            }
        }
    }

    private void DashCharge()
    {
        //  ANIMATION STATE INFO
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.DASHCHARGE;

        if (GameManager.instance.gameplayController.rawDashDirectionInput != Vector2.zero)
        {
            dashIndirection = GameManager.instance.gameplayController.dashDirectionInput;
        }

        //  Rotation of arrow and direction of dash
        angle = Vector2.SignedAngle(Vector2.right, dashIndirection);
        statemachineController.core.dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle);

        if (GameManager.instance.gameplayController.dashInputStop || Time.time >= startTime + movementData.maxHoldTime)
        {
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("chargeDash", false);
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("burstDash", true);
            isHolding = false;
            startTime = Time.time;
            statemachineController.core.CheckIfShouldFlip(Mathf.RoundToInt(dashIndirection.x));
            statemachineController.core.playerRB.drag = movementData.drag;
            statemachineController.core.SetVelocityDash(movementData.dashVelocity, dashIndirection);

            lastDirection = statemachineController.transform.eulerAngles;

            statemachineController.core.childPlayer.Rotate(0f, statemachineController.transform.rotation.y,
                 DashRotation());

            statemachineController.core.dashDirectionIndicator.gameObject.SetActive(false);
            statemachineController.core.PlaceAfterImage();
        }
    }

    private void DashBurst()
    {
        //  ANIMATION STATE INFO
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo =
            PlayerStats.AnimatorStateInfo.DASHBURST;

        //  AFTER IMAGE EFFECTS
        statemachineController.core.CheckIfShouldPlaceAfterImage();

        //  DONE ABILITY IF TIME IS GREATER THAN DASH TIME
        if (Time.time >= startTime + movementData.dashTime)
            DoneDashingState();

        //  DONE ABILITY IF TOUCHING WALL
        else if (isTouchingWall || isTouchingClimbWall)
        {
            DoneDashingState();

            if (!isTouchingLedge && isSameHeightToPlatform &&
                GameManager.instance.PlayerStats.GetSetCurrentStamina
                >= movementData.ledgeStamina)
                statemachineChanger.ChangeState(statemachineController.ledgeClimbState);
        }
        //  SLOPE SLIDE
        //else if (isGrounded && isFrontFootTouchSlope &&
        //    !statemachineController.core.groundPlayerController.canWalkOnSlope)
        //{
        //    statemachineController.core.SetVelocityZero();

        //    Debug.Log("can steep slope slide");

        //    DoneDashingState();

        //    statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
        //}
    }

    private void DoneDashingState()
    {
        statemachineController.core.childPlayer.rotation = Quaternion.Euler(0f,
                statemachineController.core.childPlayer.eulerAngles.y,
                lastDirection.z);

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("burstDash", false);
        statemachineController.core.playerRB.drag = 0f;
        isAbilityDone = true;
        canDash = true;

        lastDashTime = startTime;
    }

    public bool CheckIfCanDash()
    {
        return canDash && Time.time >= lastDashTime + movementData.dashCooldown;
    }

    public void ResetCanDash() => canDash = true;

    private float DashRotation()
    {
        switch (angle)
        {
            case 180:
                return 0;
            case 45:
                return angle;
            case -45:
                return angle;
            case 135:
                return 45;
            case -135:
                return -45;
            default:
                return angle;
        }
    }

    #endregion

    #region DASH EFFECT

    #endregion
}
