using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    private float idleEnterTime;
    private bool canTauntIdle;

    public PlayerIdleState(PlayerStateMachinesController movementController,
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

        if (statemachineController.core.groundPlayerController.canWalkOnSlope)
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
            //  Attack
            AttackInitiate();

            //  Slope slide
            if (!statemachineController.core.groundPlayerController.canWalkOnSlope &&
                isFrontFootTouchSlope)
                statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);

            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0f)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX !=
                    statemachineController.core.GetFacingDirection)
                {
                    statemachineController.changeIdleDirectionState.SpriteDirectionAfterAnimation(
                        GameManager.instance.gameplayController.GetSetMovementNormalizeX);
                    statemachineChanger.ChangeState(statemachineController.changeIdleDirectionState);
                }

                else
                    statemachineChanger.ChangeState(statemachineController.moveState);
            }

            else if (GameManager.instance.gameplayController.jumpInput &&
                statemachineController.core.groundPlayerController.canWalkOnSlope)
            {
                statemachineChanger.ChangeState(statemachineController.jumpState);
                GameManager.instance.gameplayController.UseJumpInput();
            }

            else if (canTauntIdle)
                statemachineChanger.ChangeState(statemachineController.tauntIdleState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == 1f)
                statemachineChanger.ChangeState(statemachineController.lookingUpState);

            else if (!isAnimationFinished &&
                GameManager.instance.gameplayController.movementNormalizeY == -1)
                statemachineChanger.ChangeState(statemachineController.lookingDownState);

            else if (GameManager.instance.gameplayController.dodgeInput &&
                statemachineController.playerDodgeState.CheckIfCanDodge())
                statemachineChanger.ChangeState(statemachineController.playerDodgeState);

            else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0 &&
                GameManager.instance.gameplayController.switchPlayerLeftInput &&
                GameManager.instance.gameplayController.switchPlayerRightInput &&
                statemachineController.switchPlayerState.CheckIfCanSwitch())
                statemachineChanger.ChangeState(statemachineController.switchPlayerState);

            else if (!isFootTouchGround && !isFrontFootTouchSlope)
            {
                statemachineController.nearLedgeState.SetLastDirection(statemachineController.core.GetFacingDirection);
                statemachineChanger.ChangeState(statemachineController.nearLedgeState);
            }
        }
    }

    private void TransitionTauntIdleTimer()
    {
        if (Time.time >= idleEnterTime + movementData.idleToTauntIdleTime)
            canTauntIdle = true;
    }
}
