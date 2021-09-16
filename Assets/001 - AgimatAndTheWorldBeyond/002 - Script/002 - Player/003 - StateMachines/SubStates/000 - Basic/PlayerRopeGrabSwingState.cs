using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeGrabSwingState : PlayerRopeState
{
    public PlayerRopeGrabSwingState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.ROPEGRAB;

        statemachineController.core.ropePlayerController.ropeRB.bodyType = RigidbodyType2D.Dynamic;
        statemachineController.core.playerRB.bodyType = RigidbodyType2D.Dynamic;
        statemachineController.core.playerRB.freezeRotation = false;

        //  TODO: This is for storing the rotation and position from rope start grab
        parentPlayerOldRot = statemachineController.ropeStartGrab.parentPlayerOldRot;
        childPlayerOldRot = statemachineController.ropeStartGrab.childPlayerOldRot;
        ropeOldRot = statemachineController.ropeStartGrab.ropeOldRot;
        ropeOldPos = statemachineController.ropeStartGrab.ropeOldPos;

        //  TODO: This is for connecting hinge joint when is transitioning from climb up/down state
        if (!statemachineController.ropeStartGrab.firstGrab)
            statemachineController.core.ropePlayerController.RopePlayerHingeJointConnector();
        else
            statemachineController.ropeStartGrab.firstGrab = false; //  TODO: this is for turning off the first grab checker
    }

    public override void DoChecks()
    {
        base.DoChecks();

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetFloat("xVelocity",
            statemachineController.core.GetCurrentVelocity.x * statemachineController.core.GetFacingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (GameManager.instance.gameplayController.ropeInput)
            {

                if (GameManager.instance.gameplayController.jumpInput)
                {
                    ExitRopeState();
                    statemachineChanger.ChangeState(statemachineController.ropeJumpState);
                }

                else if (statemachineController.core.ropePlayerController.RopeAboveChecker && 
                    GameManager.instance.gameplayController.movementNormalizeY > 0)
                    //  TODO: CLIMB UP
                    statemachineChanger.ChangeState(statemachineController.ropeClimbUp);

                else if (statemachineController.core.ropePlayerController.RopeBelowChecker &&
                    GameManager.instance.gameplayController.movementNormalizeY < 0)
                {
                    //  TODO: CLIMB DOWN

                    statemachineChanger.ChangeState(statemachineController.ropeClimbDown);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Swing();
    }

    private void Swing()
    {
        if (!GameManager.instance.gameplayController.ropeInput)
            return;

        statemachineController.core.playerRB.AddRelativeForce(new Vector2(GameManager.instance.gameplayController.ropeNormalizeMovementX *
        movementData.ropeSwingVelocity, 0f));
    }
}
