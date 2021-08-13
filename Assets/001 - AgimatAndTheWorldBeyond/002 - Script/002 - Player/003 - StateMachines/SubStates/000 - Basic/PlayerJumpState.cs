using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (isGrounded)
            statemachineController.core.SetVelocityY(movementData.jumpStrength);

        isAbilityDone = true;
        statemachineController.inAirState.SetIsJumping();
    }
}
