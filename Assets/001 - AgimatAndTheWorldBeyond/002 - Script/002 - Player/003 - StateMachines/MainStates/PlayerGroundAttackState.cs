using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttackState : PlayerStatesController
{
    protected bool isGrounded;
    protected bool canWalkOnSlope;
    protected bool isFrontFootTouchSlope;

    public PlayerGroundAttackState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //  Environment Checkers
        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        canWalkOnSlope = statemachineController.core.groundPlayerController.canWalkOnSlope;
        isFrontFootTouchSlope = statemachineController.core.groundPlayerController.CheckIfFrontTouchingSlope;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            //  In air not grounded or falling
            if (!isGrounded)
            {
                statemachineController.core.attackComboIndex = 0;
                statemachineChanger.ChangeState(statemachineController.inAirState);
            }

            //  Slope slide
            else if (!canWalkOnSlope && isFrontFootTouchSlope)
            {
                statemachineController.core.attackComboIndex = 0;
                statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
            }

            else if (statemachineController.core.attackComboIndex == 0)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);

                else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                    statemachineChanger.ChangeState(statemachineController.moveState);
            }

            //  For cancel animation gameplay
            if (statemachineController.core.canCancelAnimation)
            {
                if (GameManager.instance.gameplayController.jumpInput)
                {
                    statemachineController.core.canCancelAnimation = false;
                    statemachineController.core.attackComboIndex = 0;
                    statemachineChanger.ChangeState(statemachineController.jumpState);
                    GameManager.instance.gameplayController.UseJumpInput();
                }

                else if (GameManager.instance.gameplayController.dodgeInput)
                {
                    statemachineController.core.canCancelAnimation = false;
                    statemachineController.core.attackComboIndex = 0;
                    statemachineChanger.ChangeState(statemachineController.playerDodgeState);
                }

                else if (statemachineController.playerDashState.CheckIfCanDash() &&
                statemachineController.core.groundPlayerController.canWalkOnSlope &&
                GameManager.instance.gameplayController.dashInput &&
                !GameManager.instance.gameplayController.switchPlayerLeftInput)
                {
                    statemachineController.core.canCancelAnimation = false;
                    statemachineController.core.attackComboIndex = 0;
                    statemachineChanger.ChangeState(statemachineController.playerDashState);
                }
            }
        }
    }
}
