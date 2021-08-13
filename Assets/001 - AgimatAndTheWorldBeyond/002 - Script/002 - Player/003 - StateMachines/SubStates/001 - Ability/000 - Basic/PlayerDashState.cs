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
    private Vector2 lastAfterImagePosition;
    private Vector3 lastDirection;

    public PlayerDashState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
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
    }

    #region DASH FUNCTIONS

    private void SettingsSetter()
    {
        //  For game controller input
        canDash = false;
        GameManager.instance.gameInputController.UseDashInput();

        //  For Dash
        isHolding = true;

        dashIndirection = Vector2.right * statemachineController.core.GetFacingDirection;

        startTime = Time.time;

        statemachineController.core.dashDirectionIndicator.gameObject.SetActive(true);
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            //  ANIMATION
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("yVelocity",
                statemachineController.core.GetCurrentVelocity.y);
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
                Mathf.Abs(statemachineController.core.GetCurrentVelocity.x));

            if (isHolding)
            {
                //  ANIMATION STATE INFO
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.DASHCHARGE;

                if (GameManager.instance.gameInputController.rawDashDirectionInput != Vector2.zero)
                {
                    dashIndirection = GameManager.instance.gameInputController.dashDirectionInput;
                }

                //  Rotation of arrow and direction of dash
                angle = Vector2.SignedAngle(Vector2.right, dashIndirection);
                statemachineController.core.dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle);

                if (GameManager.instance.gameInputController.dashInputStop || Time.time >= startTime + movementData.maxHoldTime)
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
                    PlaceAfterImage();
                }
            }
            else
            {
                //  ANIMATION STATE INFO
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = 
                    PlayerStats.AnimatorStateInfo.DASHBURST;

                //  AFTER IMAGE EFFECTS
                CheckIfShouldPlaceAfterImage();

                //  DONE ABILITY IF TIME IS GREATER THAN DASH TIME
                if ((Time.time >= startTime + movementData.dashTime))
                {
                    statemachineController.core.childPlayer.rotation = Quaternion.Euler(0f,
                        statemachineController.core.childPlayer.eulerAngles.y,
                        lastDirection.z);

                    GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("burstDash", false);
                    statemachineController.core.playerRB.drag = 0f;
                    isAbilityDone = true;
                    canDash = true;

                    lastDashTime = Time.time;

                }
                //  DONE ABILITY IF TOUCHING WALL
                else if (isTouchingWall || isTouchingClimbWall)
                {
                    statemachineController.core.childPlayer.rotation = Quaternion.Euler(0f,
                        statemachineController.core.childPlayer.eulerAngles.y,
                        lastDirection.z);

                    GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("burstDash", false);
                    statemachineController.core.playerRB.drag = 0f;
                    isAbilityDone = true;
                    canDash = true;

                    lastDashTime = Time.time;

                    if (!isTouchingLedge && isSameHeightToPlatform &&
                        GameManager.instance.PlayerStats.GetSetCurrentStamina
                        >= movementData.ledgeStamina)
                        statemachineChanger.ChangeState(statemachineController.ledgeClimbState);
                }
            }
        }
    }

    private void PlayerDashMove()
    {
        if (!isHolding)
        {
            statemachineController.core.SetVelocityDash(movementData.dashVelocity,
                dashIndirection);
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(statemachineController.transform.position, lastAfterImagePosition) >= movementData.distanceBetweenAfterImages)
            PlaceAfterImage();
    }

    private void PlaceAfterImage()
    {
        GameManager.instance.afterImagePooler.GetFromPool();
        lastAfterImagePosition = statemachineController.transform.position;
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
}
