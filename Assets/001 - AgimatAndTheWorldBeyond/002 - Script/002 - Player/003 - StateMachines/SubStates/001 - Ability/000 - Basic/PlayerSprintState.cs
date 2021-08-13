using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintState : PlayerAbilityState
{
    private float facingDirection;

    public PlayerSprintState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
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

        isAbilityDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameInputController.GetSetMovementNormalizeX);

        AnimationChanger();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        SprintingMove();
    }

    private void SettingsSetter()
    {
        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.SPRINT;

        isAbilityDone = false;
        facingDirection = statemachineController.core.GetFacingDirection;
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            if (statemachineController.core.GetCurrentVelocity.y < 0.01 &&
                !isGrounded)
                statemachineChanger.ChangeState(statemachineController.inAirState);

            else if (isGrounded && GameManager.instance.gameInputController.jumpInput)
            {
                GameManager.instance.gameInputController.UseJumpInput();
                statemachineChanger.ChangeState(statemachineController.jumpState);
            }

            else if (GameManager.instance.gameInputController.GetSetMovementNormalizeX != 0 &&
                GameManager.instance.gameInputController.GetSetMovementNormalizeX != facingDirection)
            {
                GameManager.instance.gameInputController.sprintTapCount = 0;
                statemachineChanger.ChangeState(statemachineController.moveState);
            }

            else if (GameManager.instance.gameInputController.sprintTapCount == 0
                && GameManager.instance.gameInputController.GetSetMovementNormalizeX == 0)
                statemachineChanger.ChangeState(statemachineController.idleState);

        }
    }

    private void SprintingMove()
    {
        if (!isExitingState)
        {
            if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == facingDirection)
                statemachineController.core.SetVelocityX(movementData.sprintSpeed *
                GameManager.instance.gameInputController.GetSetMovementNormalizeX,
                    statemachineController.core.GetCurrentVelocity.y);
        }
    }
}
