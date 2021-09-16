using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeClimbDownState : PlayerRopeState
{
    public PlayerRopeClimbDownState(PlayerStateMachinesController movementController, PlayerStateMachineChanger stateMachine,
        PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }



    public override void Enter()
    {
        base.Enter();

        statemachineController.core.ropePlayerController.ropeRB.bodyType = RigidbodyType2D.Kinematic;
        statemachineController.core.playerRB.bodyType = RigidbodyType2D.Kinematic;
        statemachineController.core.playerRB.freezeRotation = true;

        //  TODO: This is for storing the old rotation and position of player 
        //  and its game objects that will be change
        parentPlayerOldRot = statemachineController.ropeStartGrab.parentPlayerOldRot;
        childPlayerOldRot = statemachineController.ropeStartGrab.childPlayerOldRot;
        ropeOldRot = statemachineController.ropeStartGrab.ropeOldRot;
        ropeOldPos = statemachineController.ropeStartGrab.ropeOldPos;

        statemachineController.core.ropePlayerController.RemoveRopePlayerHingeJointConnector(); //  TODO: removing player from hinge joint
    }

    public override void Exit()
    {
        base.Exit();
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

                else if (GameManager.instance.gameplayController.movementNormalizeY == 0 ||
                    !statemachineController.core.ropePlayerController.RopeBelowChecker)
                {
                    statemachineController.transform.parent = null;
                    statemachineChanger.ChangeState(statemachineController.ropeGrabSwing);
                }
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        ClimbDownStopForce();
        ClimbDown();
    }

    private void ClimbDown()
    {
        if (!GameManager.instance.gameplayController.ropeInput)
            return;

        statemachineController.transform.parent = statemachineController.core.ropePlayerController.RopePosition();

        direction = Mathf.Sign(Vector3.Dot(-statemachineController.core.ropePlayerController.RopePosition().right, Vector3.up));
        statemachineController.transform.localRotation = Quaternion.AngleAxis(direction * 90f, Vector3.forward);

        statemachineController.core.playerRB.MovePosition(new Vector2(statemachineController.core.ropePlayerController.RopePosition().GetComponent<Rigidbody2D>().position.x + (-0.25f *
            statemachineController.core.GetFacingDirection), statemachineController.core.playerRB.position.y));

        statemachineController.transform.Translate(-Vector2.up * movementData.ropeClimDownVelocity * Time.fixedDeltaTime);
    }

    private void ClimbDownStopForce()
    {
        if (!GameManager.instance.gameplayController.ropeInput)
            return;

        statemachineController.core.ropePlayerController.RopePosition().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
