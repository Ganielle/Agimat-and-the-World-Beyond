using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteepSlopeSlideState : PlayerGroundState
{
    public PlayerSteepSlopeSlideState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.STEEPSLOPE;

        DirectionChecker();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (statemachineController.core.groundPlayerController.canWalkOnSlope)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0f)
                    statemachineChanger.ChangeState(statemachineController.idleState);

                else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0f)
                    statemachineChanger.ChangeState(statemachineController.moveState);


                //  TODO: SLOPE ASCEND MOVEMENT
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityY(-10f);
    }

    public void DirectionChecker()
    {
        // if facing left while on slope and the slope is super steep on left 
        //   asuming the facing direction is -1 and slopeForward -1 also then
        // we will flip it to right but what if facing direction is 1 and
        // slopeForward is 1 also then should we flip it ?

        if (isFootTouchGround)
        {
            if (statemachineController.core.groundPlayerController.slopeForward.x <
                0)
                statemachineController.core.CheckIfShouldFlip(1);
            else if (statemachineController.core.groundPlayerController.slopeForward.x >
                0)
                statemachineController.core.CheckIfShouldFlip(-1);
        }
    }
}
