using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    private bool canReduceSpeed;
    private bool canBreakRun;
    private float runStateEnterTime;
    private int lastDirection;

    public PlayerMoveState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        SettingsSetter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= runStateEnterTime + 3f)
            canBreakRun = true;

        if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
            lastDirection = GameManager.instance.gameplayController.GetSetMovementNormalizeX;

        //  Flip Player
        statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        MovePlayer();

        ReduceVelocity();
    }

    private void SettingsSetter()
    {
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.RUNNING;

        runStateEnterTime = Time.time;
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            AttackInitiate();

            //  Slope slide
            if (!statemachineController.core.groundPlayerController.canWalkOnSlope &&
                isFrontFootTouchSlope)
                statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);

            else if (statemachineController.core.groundPlayerController.canWalkOnSlope)
            {
                //  Running break
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0f)
                    canReduceSpeed = false;

                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0f)
                {
                    canReduceSpeed = true;

                    if (statemachineController.core.GetCurrentVelocity.x == 0 && canBreakRun)
                    {
                        statemachineController.runningBreakState.SetCurrentDirection(lastDirection);
                        statemachineChanger.ChangeState(statemachineController.runningBreakState);
                        canBreakRun = false;
                    }

                    else if (statemachineController.core.GetCurrentVelocity.x == 0 && !canBreakRun)
                        statemachineChanger.ChangeState(statemachineController.idleState);
                }

                else if (GameManager.instance.gameplayController.jumpInput &&
                    statemachineController.core.groundPlayerController.canWalkOnSlope)
                {
                    statemachineChanger.ChangeState(statemachineController.jumpState);
                    GameManager.instance.gameplayController.UseJumpInput();
                }

                else if (GameManager.instance.gameplayController.dodgeInput
                    && statemachineController.playerDodgeState.CheckIfCanDodge() &&
                    GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                    PlayerStats.AnimatorStateInfo.HIGHLAND)
                    statemachineChanger.ChangeState(statemachineController.playerDodgeState);
            }
        }
    }

    private void MovePlayer()
    {
        if (statemachineController.core.GetFacingDirection == GameManager.instance.gameplayController.GetSetMovementNormalizeX &&
            isFrontFootTouchDefaultGround)
            return;

        statemachineController.core.SetVelocityX(movementData.movementSpeed *
         GameManager.instance.gameplayController.GetSetMovementNormalizeX,
         statemachineController.core.GetCurrentVelocity.y);

        statemachineController.core.groundPlayerController.SlopeMovement();
    }

    private void ReduceVelocity()
    {
        if (canReduceSpeed)
        {
            //  right
            if (statemachineController.core.GetFacingDirection == 1)
            {
                statemachineController.core.GetCurrentVelocity.x -= 100f * Time.deltaTime;

                if (statemachineController.core.GetCurrentVelocity.x <= 1f)
                {
                    statemachineController.core.GetCurrentVelocity.x = 0f;
                    canReduceSpeed = false;
                }
            }
            //  left
            else if (statemachineController.core.GetFacingDirection == -1)
            {
                statemachineController.core.GetCurrentVelocity.x += 100f * Time.deltaTime;


                if (statemachineController.core.GetCurrentVelocity.x >= -1f)
                {
                    statemachineController.core.GetCurrentVelocity.x = 0f;
                    canReduceSpeed = false;
                }
            }
            statemachineController.core.SetVelocityX(statemachineController.core.GetCurrentVelocity.x,
                0f);
        }
    }
}
