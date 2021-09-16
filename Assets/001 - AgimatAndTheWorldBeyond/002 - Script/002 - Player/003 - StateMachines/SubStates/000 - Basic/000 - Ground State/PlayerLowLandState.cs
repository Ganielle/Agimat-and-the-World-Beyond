using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLowLandState : PlayerGroundState
{
    public PlayerLowLandState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LOWLAND;

        if (statemachineController.core.groundPlayerController.canWalkOnSlope)
            statemachineController.core.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                statemachineChanger.ChangeState(statemachineController.moveState);

            //Slope slide
            if (!statemachineController.core.groundPlayerController.canWalkOnSlope &&
                isFrontFootTouchSlope)
                statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);

            if (isAnimationFinished)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();
    }
}
