using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningBreak : PlayerGroundState
{
    private int currentDirection;

    public PlayerRunningBreak(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.RUNNINGBREAK;
    }

    public override void Exit()
    {
        base.Exit();

        statemachineController.core.CheckIfShouldFlip(currentDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isAnimationFinished)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0f)
                    statemachineChanger.ChangeState(statemachineController.idleState);
                else
                    statemachineChanger.ChangeState(statemachineController.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (statemachineController.core.GetCurrentVelocity.x != 0)
            statemachineController.core.SetVelocityX(1f * statemachineController.core.GetFacingDirection,
            statemachineController.core.GetCurrentVelocity.y);

        else
            statemachineController.core.SetVelocityZero();
    }

    public void SetCurrentDirection(int direction) => currentDirection = direction;
}
