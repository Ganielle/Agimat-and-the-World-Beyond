﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookUpState : PlayerGroundState
{
    public PlayerLookUpState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.LOOKINGUP;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityZero();

        if (GameManager.instance.gameInputController.movementNormalizeY == 0)
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool(animBoolName, false);

        if (!isExitingState)
        {
            if (isAnimationFinished)
            {
                if (GameManager.instance.gameInputController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);
                else
                    statemachineChanger.ChangeState(statemachineController.moveState);
            }
        }
    }
}