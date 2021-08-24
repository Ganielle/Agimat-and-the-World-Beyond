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
            if (GameManager.instance.gameplayController.movementNormalizeY == 1f &&
                GameManager.instance.gameplayController.grabWallInput)
                statemachineChanger.ChangeState(statemachineController.wallClimbState);

            else if (GameManager.instance.gameplayController.jumpInput)
                statemachineChanger.ChangeState(statemachineController.wallJumpState);

            else if (GameManager.instance.gameplayController.grabWallInput &&
                GameManager.instance.gameplayController.movementNormalizeY == 0f)
                statemachineChanger.ChangeState(statemachineController.wallGrabState);

            else if (isGrounded && !GameManager.instance.gameplayController.grabWallInput)
                statemachineChanger.ChangeState(statemachineController.idleState);

        }
    }
}
