using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeStartGrabState : PlayerRopeState
{
    private bool canSkipStartGrab = false;
    public bool firstGrab;

    public PlayerRopeStartGrabState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        canSkipStartGrab = true;
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.ROPEGRAB;

        firstGrab = true; // TODO: This is for checking if we should call hinge joint connector on rope grab swing state on first grab

        statemachineController.core.ropePlayerController.ropeRB.bodyType = RigidbodyType2D.Dynamic;
        statemachineController.core.playerRB.bodyType = RigidbodyType2D.Dynamic;
        statemachineController.core.playerRB.freezeRotation = false;

        //  TODO: This is for storing the old rotation and position of player 
        //  and its game objects that will be change
        parentPlayerOldRot = statemachineController.core.parentPlayer.eulerAngles;
        childPlayerOldRot = statemachineController.core.childPlayer.eulerAngles;
        ropeOldRot = statemachineController.core.ropePlayerController.ropeCheck.localEulerAngles;
        ropeOldPos = statemachineController.core.ropePlayerController.ropeCheck.localPosition;

        statemachineController.core.ropePlayerController.RopePlayerHingeJointConnector(); //  TODO: removing player from hinge joint

        HingeJointSettingsBasedOnDirection();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityZero();

        if (GameManager.instance.gameplayController.ropeInput)
        {

            if (canSkipStartGrab)
            {
                canSkipStartGrab = false;
                statemachineChanger.ChangeState(statemachineController.ropeGrabSwing);
            }
        }
    }

    private void HingeJointSettingsBasedOnDirection()
    {
        //  TODO: SETTING UP ROTATION BASED ON PLAYER DIRECTION
        //  AND ANGLE LIMITS ON HINGE JOINS

        JointAngleLimits2D limits = statemachineController.core.ropePlayerController.ropeHingeJoint.limits;

        if (statemachineController.core.GetFacingDirection == 1)
        {
            limits.min = movementData.rightAngle;
            limits.max = movementData.rightAngle;
        }
        else
        {
            limits.min = movementData.leftAngle;
            limits.max = movementData.leftAngle;
        }

        statemachineController.core.ropePlayerController.playerHingeJoint.anchor = new Vector2(movementData.anchorX * statemachineController.core.GetFacingDirection,
            statemachineController.core.ropePlayerController.playerHingeJoint.anchor.y);

        statemachineController.core.ropePlayerController.ropeHingeJoint.limits = limits;
    }
}
