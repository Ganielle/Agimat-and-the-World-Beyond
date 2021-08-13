using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkeyBarGrabState : PlayerTouchingMonkeyBarState
{

    public PlayerMonkeyBarGrabState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName) : 
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.MONKEYBARGRAB;

        holdPosition = statemachineController.core.MonkeyBarPosition().position;
        HoldPosition(statemachineController.core.transform.position.x,
            holdPosition.y - statemachineController.core.playerRawData.mbStartOffset.y);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        HoldPosition(statemachineController.core.transform.position.x,
            holdPosition.y - statemachineController.core.playerRawData.mbStartOffset.y);

        statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameInputController.GetSetMovementNormalizeX);

        if (!isExitingState)
        {
            if (isTouchingMonkeyBarFront &&
                GameManager.instance.gameInputController.grabMonkeyBarInput &&
                GameManager.instance.gameInputController.GetSetMovementNormalizeX != 0)
                statemachineChanger.ChangeState(statemachineController.monkeyBarMove);

            else if (GameManager.instance.gameInputController.jumpInput)
            {
                //GameManager.instance.gameInputController.UseGrabMonkeyBarInput();
                statemachineChanger.ChangeState(statemachineController.monkeyBarJump);
                GameManager.instance.gameInputController.UseJumpInput();
            }
        }
    }
}
