using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerNormalAbilityState
{
    private bool dodgeNow;
    private bool canDodge;
    public float lastDodgeTime;

    public PlayerDodgeState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName) : base(movementController, stateMachine, movementData,
            animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        dodgeNow = false;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        dodgeNow = true;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        SettingsSetter();
    }

    public override void Exit()
    {
        base.Exit();

        canDodge = true;
        statemachineController.core.ShadowCaster(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        DodgeMove();
        statemachineController.core.ShadowCaster(true);
    }

    private void SettingsSetter()
    {
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.DODGE;

        isAbilityDone = false;
        canDodge = false;
    }

    private void DodgeMove()
    {
        if (!isExitingState)
        {
            if (dodgeNow)
            {
                statemachineController.core.SetVelocityX(
                    movementData.dodgeVelocity *
                    statemachineController.core.GetFacingDirection,
                    statemachineController.core.GetCurrentVelocity.y);
            }
        }
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            if (!dodgeNow)
            {
                if (!isGrounded)
                {
                    if (statemachineController.core.GetCurrentVelocity.y < 0.01f)
                        statemachineChanger.ChangeState(statemachineController.inAirState);
                }
                else
                {
                    if (!statemachineController.core.groundPlayerController.canWalkOnSlope &&
                        isFrontFootTouchSlope)
                    {
                        Debug.Log("hello");
                        statemachineController.core.SetVelocityZero();

                        statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
                    }

                    else if (GameManager.instance.gameplayController.jumpInput)
                    {
                        statemachineChanger.ChangeState(statemachineController.jumpState);
                        GameManager.instance.gameplayController.UseJumpInput();
                    }

                    else if (isGrounded && GameManager.instance.gameplayController.GetSetMovementNormalizeX
                        == 0)
                        statemachineChanger.ChangeState(statemachineController.idleState);

                    else if (isGrounded && GameManager.instance.gameplayController.GetSetMovementNormalizeX
                        != 0)
                        statemachineChanger.ChangeState(statemachineController.moveState);
                }

                lastDodgeTime = Time.time;
            }
        }
    }

    public bool CheckIfCanDodge()
    {
        return canDodge && Time.time >= lastDodgeTime + movementData.dodgeCooldown;
    }

    public void ResetDodge() => canDodge = true;
}