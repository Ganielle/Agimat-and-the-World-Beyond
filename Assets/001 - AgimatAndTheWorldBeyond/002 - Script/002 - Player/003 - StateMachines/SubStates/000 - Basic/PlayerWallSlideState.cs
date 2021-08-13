using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName) : 
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.WALLSLIDE;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityY(-movementData.wallSlideVelocity);

        if (!isExitingState)
        {
            if (GameManager.instance.gameInputController.movementNormalizeY == 1f &&
                GameManager.instance.gameInputController.grabWallInput)
                statemachineChanger.ChangeState(statemachineController.wallClimbState);

            else if (GameManager.instance.gameInputController.jumpInput)
                statemachineChanger.ChangeState(statemachineController.wallJumpState);

            else if (GameManager.instance.gameInputController.grabWallInput &&
                GameManager.instance.gameInputController.movementNormalizeY == 0f)
                statemachineChanger.ChangeState(statemachineController.wallGrabState);

            else if (isGrounded && !GameManager.instance.gameInputController.grabWallInput)
                statemachineChanger.ChangeState(statemachineController.idleState);

        }
    }
}
