using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNearLedgeState : PlayerGroundState
{
    private int lastDirection;

    public PlayerNearLedgeState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.NEARLEDGE;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                statemachineChanger.ChangeState(statemachineController.moveState);

            else if (isFootTouchGround)
                statemachineChanger.ChangeState(statemachineController.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        statemachineController.core.SetVelocityZero();

        //if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == lastDirection)
        //    statemachineController.core.SetVelocityX(movementData.pushForcePlayerWhenFootNotTouchingGround *
        //        statemachineController.core.GetFacingDirection, statemachineController.core.GetCurrentVelocity.y);
    }

    public void SetLastDirection(int direction) => lastDirection = direction;
}
