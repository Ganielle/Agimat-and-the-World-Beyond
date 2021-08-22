using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteepSlopeSlideState : PlayerGroundState
{
    private int lastDirection;

    public PlayerSteepSlopeSlideState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName) : base(movementController, stateMachine, movementData, 
            animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        statemachineController.core.SetVelocityZero();

        if (statemachineController.core.GetCurrentVelocity.x > 0)
            statemachineController.core.CheckIfShouldFlip(1);
        else if (statemachineController.core.GetCurrentVelocity.x < 0)
            statemachineController.core.CheckIfShouldFlip(-1);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (statemachineController.core.GetCurrentVelocity.x > 0)
            statemachineController.core.CheckIfShouldFlip(1);
        else if (statemachineController.core.GetCurrentVelocity.x < 0)
            statemachineController.core.CheckIfShouldFlip(-1);

        if (!isExitingState)
        {
            if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == 0f)
                statemachineChanger.ChangeState(statemachineController.idleState);

            else if (GameManager.instance.gameInputController.GetSetMovementNormalizeX != 0f)
                statemachineChanger.ChangeState(statemachineController.moveState);

            //  TODO: SLOPE ASCEND MOVEMENT
        }
    }

    public void SetLastDirection(int lastDirection) => this.lastDirection = lastDirection;
}
