using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTauntIdleState : PlayerGroundState
{
    private float tauntIdleEnterTime;
    private bool canIdle;
    private bool canCheckTime;

    public PlayerTauntIdleState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.TAUNTIDLE;

        tauntIdleEnterTime = Time.time;
        canCheckTime = true;
    }

    public override void Exit()
    {
        base.Exit();

        canIdle = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        TauntIdleToIdleTransitionTimer();

        if (!isExitingState)
        {
            if (canIdle || !canCheckTime)
                GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool(animBoolName, false);
            else
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0 ||
                    GameManager.instance.gameplayController.movementNormalizeY == 1 ||
                    GameManager.instance.gameplayController.movementNormalizeY == -1 ||
                    GameManager.instance.gameplayController.jumpInput ||
                    GameManager.instance.gameplayController.attackInput)
                    canCheckTime = false;
            }

            if (isAnimationFinished)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);

                else if(GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                {
                    statemachineChanger.ChangeState(statemachineController.moveState);
                }

                else if (GameManager.instance.gameplayController.movementNormalizeY == 1)
                    statemachineChanger.ChangeState(statemachineController.lookingUpState);

                else if (GameManager.instance.gameplayController.movementNormalizeY == 1)
                    statemachineChanger.ChangeState(statemachineController.lookingUpState);
            }
        }
    }

    private void TauntIdleToIdleTransitionTimer()
    {
        if (Time.time >= tauntIdleEnterTime + movementData.tauntIdleToIdleTime && canCheckTime)
            canIdle = true;
    }
}
