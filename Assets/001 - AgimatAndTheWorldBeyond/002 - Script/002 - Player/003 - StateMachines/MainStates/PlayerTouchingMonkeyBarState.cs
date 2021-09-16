using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingMonkeyBarState : PlayerStatesController
{
    protected bool isTouchingMonkeyBar;
    protected bool isTouchingMonkeyBarFront;

    protected Vector2 holdPosition;

    public PlayerTouchingMonkeyBarState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) : base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isTouchingMonkeyBar = statemachineController.core.CheckIfTouchingMonkeyBar;
        isTouchingMonkeyBarFront = statemachineController.core.CheckIfTouchingMonkeyBarFront;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!GameManager.instance.gameplayController.grabMonkeyBarInput)
            statemachineChanger.ChangeState(statemachineController.inAirState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void HoldPosition(float x, float y)
    {
        statemachineController.transform.position = new Vector2(x, y);

        statemachineController.core.SetVelocityZero();
    }
}
