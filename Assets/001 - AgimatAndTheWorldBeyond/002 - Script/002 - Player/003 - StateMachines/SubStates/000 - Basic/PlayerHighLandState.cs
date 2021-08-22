using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHighLandState : PlayerGroundState
{
    public PlayerHighLandState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.HIGHLAND;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityZero();

        if (!isExitingState)
        {
            if (isAnimationFinished)
                statemachineChanger.ChangeState(statemachineController.idleState);
            else
            {
                //  Slope slide
                //statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
            }
        }

    }
}
