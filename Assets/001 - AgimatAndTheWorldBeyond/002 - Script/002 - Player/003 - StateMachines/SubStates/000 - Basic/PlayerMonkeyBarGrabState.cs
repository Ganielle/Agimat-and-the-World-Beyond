using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkeyBarGrabState : PlayerTouchingMonkeyBarState
{

    public PlayerMonkeyBarGrabState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
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

        statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);

        if (!isExitingState)
        {
            if (isTouchingMonkeyBarFront &&
                GameManager.instance.gameplayController.grabMonkeyBarInput &&
                GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                statemachineChanger.ChangeState(statemachineController.monkeyBarMove);

            else if (GameManager.instance.gameplayController.jumpInput)
            {
                //GameManager.instance.gameInputController.UseGrabMonkeyBarInput();
                statemachineChanger.ChangeState(statemachineController.monkeyBarJump);
                GameManager.instance.gameplayController.UseJumpInput();
            }
        }
    }
}
