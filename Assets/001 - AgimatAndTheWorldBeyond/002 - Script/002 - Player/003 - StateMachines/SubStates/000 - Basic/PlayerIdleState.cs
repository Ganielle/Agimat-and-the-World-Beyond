using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    private float idleEnterTime;
    private bool canTauntIdle;

    public PlayerIdleState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
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

        canTauntIdle = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        TransitionTauntIdleTimer();

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }

    private void SettingsSetter()
    {
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.IDLE;

        idleEnterTime = Time.time;
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

            if (GameManager.instance.gameInputController.GetSetMovementNormalizeX != 0f)
            {
                if (GameManager.instance.gameInputController.GetSetMovementNormalizeX !=
                    statemachineController.core.GetFacingDirection)
                {
                    statemachineController.changeIdleDirectionState.SpriteDirectionAfterAnimation(
                        GameManager.instance.gameInputController.GetSetMovementNormalizeX);
                    statemachineChanger.ChangeState(statemachineController.changeIdleDirectionState);
                }

                else
                    statemachineChanger.ChangeState(statemachineController.moveState);
            }

            else if (canTauntIdle)
                statemachineChanger.ChangeState(statemachineController.tauntIdleState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameInputController.movementNormalizeY == 1f)
                statemachineChanger.ChangeState(statemachineController.lookingUpState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameInputController.movementNormalizeY == -1)
                statemachineChanger.ChangeState(statemachineController.lookingDownState);

            else if (GameManager.instance.gameInputController.jumpInput &&
                canJump)
            {
                statemachineChanger.ChangeState(statemachineController.jumpState);
                GameManager.instance.gameInputController.UseJumpInput();
            }

            else if (GameManager.instance.gameInputController.dodgeInput &&
                statemachineController.playerDodgeState.CheckIfCanDodge())
                statemachineChanger.ChangeState(statemachineController.playerDodgeState);

            else if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == 0 &&
                GameManager.instance.gameInputController.switchPlayerLeftInput &&
                GameManager.instance.gameInputController.switchPlayerRightInput &&
                statemachineController.switchPlayerState.CheckIfCanSwitch())
                statemachineChanger.ChangeState(statemachineController.switchPlayerState);

        }
    }

    private void TransitionTauntIdleTimer()
    {
        if (Time.time >= idleEnterTime + movementData.idleToTauntIdleTime)
            canTauntIdle = true;
    }
}
