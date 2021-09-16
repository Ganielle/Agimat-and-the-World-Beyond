using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeState : PlayerStatesController
{
    public Vector3 parentPlayerOldRot;
    public Vector3 childPlayerOldRot;
    public Vector3 ropeOldPos;
    public Vector3 ropeOldRot;
    protected float direction;

    public PlayerRopeState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) : base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        statemachineController.core.ChangePlayerColliderOffsetY(movementData.colliderRopeOffsetY);
    }

    public override void Exit()
    {
        base.Exit();
        statemachineController.core.ChangePlayerColliderOffsetY(movementData.colliderNormalOffsetY);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if (!GameManager.instance.gameplayController.ropeInput)
        {
            statemachineController.core.ChangePlayerColliderOffsetY(movementData.colliderNormalOffsetY);
            ExitRopeState();
            statemachineChanger.ChangeState(statemachineController.inAirState);
        }
    }

    public void ExitRopeState()
    {
        //  TODO: RETURNING PLAYER TO POSITION AND ROTATION

        statemachineController.transform.parent = null;
        statemachineController.core.ropePlayerController.ropeRB.bodyType = RigidbodyType2D.Kinematic;
        statemachineController.core.playerRB.bodyType = RigidbodyType2D.Dynamic;
        statemachineController.core.playerRB.freezeRotation = true;
        statemachineController.core.ropePlayerController.RemoveRopePlayerHingeJointConnector();

        statemachineController.core.parentPlayer.rotation = Quaternion.Euler(parentPlayerOldRot);
        statemachineController.core.childPlayer.rotation = Quaternion.Euler(childPlayerOldRot);
        statemachineController.core.ropePlayerController.ropeCheck.rotation = Quaternion.Euler(ropeOldRot);
        statemachineController.core.ropePlayerController.ropeCheck.localPosition = ropeOldPos;

        statemachineController.transform.localScale = new Vector3(1, 1, 1);
    }
}
