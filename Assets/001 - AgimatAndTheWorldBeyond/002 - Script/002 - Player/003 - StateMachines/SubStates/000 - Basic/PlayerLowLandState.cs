using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLowLandState : PlayerGroundState
{
    public PlayerLowLandState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) 
        : base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LOWLAND;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityX(0f, statemachineController.core.GetCurrentVelocity.y);

        if (!isExitingState)
        {
            if (!isAnimationFinished)
            {
                if (GameManager.instance.gameInputController.GetSetMovementNormalizeX != 0)
                    statemachineChanger.ChangeState(statemachineController.moveState);

                //  Slope slide
                else if (statemachineController.core.groundPlayerController.isOnSlope &&
                    !statemachineController.core.groundPlayerController.canWalkOnSlope)
                    statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
            }
            else
            {
                if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);
            }
        }
    }
}
