using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMonkeyBarMove : PlayerTouchingMonkeyBarState
{
    public PlayerMonkeyBarMove(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        if (GameManager.instance.PlayerStats.GetSetPlayerSR.sortingOrder == 6)
            GameManager.instance.PlayerStats.GetSetPlayerSR.sortingOrder = 3;
        else
            GameManager.instance.PlayerStats.GetSetPlayerSR.sortingOrder = 6;
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.MONKEYBARMOVE;

        holdPosition = statemachineController.core.MonkeyBarPosition().position;
        HoldPosition(statemachineController.transform.position.x,
            holdPosition.y - statemachineController.core.playerRawData.mbStartOffset.y);
        statemachineController.core.playerRB.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void Exit()
    {
        base.Exit();

        statemachineController.core.playerRB.bodyType = RigidbodyType2D.Dynamic;
        GameManager.instance.PlayerStats.GetSetPlayerSR.sortingOrder = 6;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameInputController.GetSetMovementNormalizeX);

        if (!isExitingState)
        {
            if (isTouchingMonkeyBar && isTouchingMonkeyBarFront &&
                GameManager.instance.gameInputController.grabMonkeyBarInput)
            {
                if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.monkeyBarGrab);

                else if (GameManager.instance.gameInputController.jumpInput)
                {
                    //GameManager.instance.gameInputController.UseGrabMonkeyBarInput();
                    statemachineChanger.ChangeState(statemachineController.monkeyBarJump);
                    GameManager.instance.gameInputController.UseJumpInput();
                }
            }
            else if (isTouchingMonkeyBar && !isTouchingMonkeyBarFront &&
                GameManager.instance.gameInputController.grabMonkeyBarInput)
                statemachineChanger.ChangeState(statemachineController.monkeyBarGrab);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isTouchingMonkeyBar && isTouchingMonkeyBarFront &&
            GameManager.instance.gameInputController.grabMonkeyBarInput &&
            GameManager.instance.gameInputController.GetSetMovementNormalizeX != 0)
        {
            statemachineController.core.SetVelocityX(statemachineController.core.playerRawData.monkeyBarVelocity *
                GameManager.instance.gameInputController.GetSetMovementNormalizeX,
                statemachineController.core.GetCurrentVelocity.y);
        }
    }
}
