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
        string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        SettingsSetter();

        runStateEnterTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= runStateEnterTime + 3f)
            canBreakRun = true;

        if (GameManager.instance.gameInputController.GetSetMovementNormalizeX != 0)
            lastDirection = GameManager.instance.gameInputController.GetSetMovementNormalizeX;

        //  Flip Player
        statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameInputController.GetSetMovementNormalizeX);

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        ReduceVelocityOnX();
        MovePlayer();
        statemachineController.core.groundPlayerController.SlopeMovement();
    }

    private void SettingsSetter()
    {
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.RUNNING;
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            //  Slope slide
            //if (statemachineController.core.groundPlayerController.isOnSlope &&
            //    !statemachineController.core.groundPlayerController.canWalkOnSlope)
            //{
            //    statemachineController.steepSlopeSlide.SetLastDirection(statemachineController.core.GetFacingDirection);
            //    statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
            //}

            //  Running break
            if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == 0f)
            {
                canReduceSpeed = true;

                if (statemachineController.core.GetCurrentVelocity.x == 0f)
                    statemachineChanger.ChangeState(statemachineController.idleState);

                else if (canBreakRun && lastDirection == statemachineController.core.GetFacingDirection &&
                    statemachineController.core.GetCurrentVelocity.x != 0f)
                {
                    statemachineController.runningBreakState.SetCurrentDirection(lastDirection);
                    statemachineChanger.ChangeState(statemachineController.runningBreakState);
                    canBreakRun = false;
                }
            }


            else if (GameManager.instance.gameInputController.jumpInput && canJump)
            {
                statemachineChanger.ChangeState(statemachineController.jumpState);
                GameManager.instance.gameInputController.UseJumpInput();
            }

            else if (GameManager.instance.gameInputController.dodgeInput
                && statemachineController.playerDodgeState.CheckIfCanDodge() &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND)
                statemachineChanger.ChangeState(statemachineController.playerDodgeState);
        }
    }

    private void MovePlayer()
    {
        //if (!statemachineController.core.groundPlayerController.isOnSlope)

        statemachineController.core.SetVelocityX(movementData.movementSpeed *
         GameManager.instance.gameInputController.GetSetMovementNormalizeX,
         statemachineController.core.GetCurrentVelocity.y);

        //else if (statemachineController.core.groundPlayerController.isOnSlope &&
        //    statemachineController.core.groundPlayerController.canWalkOnSlope)
        //{
        //    statemachineController.core.SetVelocityX(movementData.movementSpeed *
        //    statemachineController.core.groundPlayerController.slopeNormalPerp.x *
        //    -GameManager.instance.gameInputController.GetSetMovementNormalizeX,
        //    movementData.movementSpeed *
        //    statemachineController.core.groundPlayerController.slopeNormalPerp.y *
        //    -GameManager.instance.gameInputController.GetSetMovementNormalizeX);
        //}
    }

    private void ReduceVelocityOnX()
    {
        if (canReduceSpeed)
        {
            if (statemachineController.core.GetCurrentVelocity.x != 0f)
                statemachineController.core.SetVelocityX(
                statemachineController.core.GetCurrentVelocity.x -= 25f * Time.fixedDeltaTime,
                statemachineController.core.GetCurrentVelocity.y);

            else
            {
                statemachineController.core.GetCurrentVelocity.x = 0f;
                canReduceSpeed = false;
            }
        }
    }
}
